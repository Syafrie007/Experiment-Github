using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Timers;
using PetaPoco;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net.Http;

namespace xx
{
    public class TagRecord : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string EPC { get; set; } // ID unik untuk setiap tag
        public string NamaPerangkat { get; set; }
        private int _readCount;

        public int ReadCount
        {
            get => _readCount;
            set
            {
                if (_readCount != value)
                {
                    _readCount = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ReadCount)));
                }
            }
        }
    }

    public class ReadCountMonitor
    {
        private readonly Dictionary<string, DateTime> lastReadTimes = new Dictionary<string, DateTime>();
        private readonly string dbPath = "temp.db";
        private readonly string apiEndpoint;
        private readonly Timer pushTimer;
        private readonly Database db;
        private readonly Dictionary<string, TagRecord> monitoredTags = new Dictionary<string, TagRecord>();
        private readonly Dictionary<string, int> bacaKe = new Dictionary<string, int>();
        private readonly List<Tuple<TagRecord ,DateTime ,int >> ListDataBaca=new List<Tuple<TagRecord, DateTime, int>>();
        public event EventHandler<ReadCountChangedEventArgs> ReadCountChanged;
        public TimeSpan RequiredTimeDiff { get; set; } = TimeSpan.FromMinutes(5);
        public string NamaPerangkat { get; set; }
        private bool isMonitoring = false;

        public ReadCountMonitor(string apiEndpoint, int pushIntervalSeconds)
        {
            this.apiEndpoint = apiEndpoint;
            db = new Database($"Data Source={dbPath};Version=3;", "Sqlite");
            EnsureDatabaseCreated();
            pushTimer = new Timer(pushIntervalSeconds * 1000);
            pushTimer.Elapsed += async (s, e) => await PushDataToServer();
            Debug.WriteLine("ReadCountMonitor initialized.");
        }

        public void Start()
        {
            if (isMonitoring) return;
            isMonitoring = true;
            pushTimer.Start();
            Debug.WriteLine("Monitoring started.");
        }

        public void Stop()
        {
            isMonitoring = false;
            pushTimer.Stop();
            Debug.WriteLine("Monitoring stopped.");
        }

        private void EnsureDatabaseCreated()
        {
            if (!File.Exists(dbPath))
            {
                using (var connection = new SQLiteConnection($"Data Source={dbPath}"))
                {
                    connection.Open();
                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = @"CREATE TABLE IF NOT EXISTS ScanData (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Guid TEXT UNIQUE,
                        Nama_Perangkat TEXT,
                        Epc TEXT,
                        Waktu_Scan TEXT,
                        Baca_Ke INTEGER
                    );";
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            Debug.WriteLine("Database ensured.");
        }

        public void MonitorTag(TagRecord tag)
        {
            if (!monitoredTags.ContainsKey(tag.EPC))
            {
                monitoredTags[tag.EPC] = tag;
                tag.PropertyChanged += HandleReadCountChange;
                Debug.WriteLine($"Monitoring tag {tag.EPC} started.");
            }
        }

        private void HandleReadCountChange(object sender, PropertyChangedEventArgs e)
        {
            if (!isMonitoring || e.PropertyName != nameof(TagRecord.ReadCount)) return;

            var tag = sender as TagRecord;
            if (tag == null) return;

            var now = DateTime.UtcNow;
            var epc = tag.EPC;
            bool isExistingTag = lastReadTimes.ContainsKey(epc);

            if (!isExistingTag || (now - lastReadTimes[epc]) >= RequiredTimeDiff)
            {
                int bacaKe;
                if (this.bacaKe.TryGetValue(epc, out var ke))
                {
                    bacaKe = ke + 1;
                    this.bacaKe[epc] = bacaKe;
                    ListDataBaca.Add(new Tuple<TagRecord, DateTime, int>(tag,now,bacaKe));
                }
                else
                {
                    bacaKe = 1;
                    this.bacaKe.Add(epc, bacaKe);
                    ListDataBaca.Add(new Tuple<TagRecord, DateTime, int>(tag, now, bacaKe));
                }

                lastReadTimes[epc] = now;
                SaveToDatabase(tag, bacaKe);
                ReadCountChanged?.Invoke(this, new ReadCountChangedEventArgs(tag, now, bacaKe));
            }

            Debug.WriteLine(string.Format("Tag {0} ReadCount updated to {1}. Baca Ke {2}.", epc, tag.ReadCount, bacaKe[epc]));
        }


        private void SaveToDatabase(TagRecord tag, int bacaKe)
        {
            db.Insert("ScanData", "Id", new
            {
                Guid = Guid.NewGuid().ToString(),
                Nama_Perangkat = NamaPerangkat,
                Epc = tag.EPC,
                Waktu_Scan = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                Baca_ke = bacaKe
            });
            Debug.WriteLine($"Saved tag {tag.EPC}-baca ke : {bacaKe} to database.");
        }

        private async Task PushDataToServer()
        {
            if (!isMonitoring || !HasInternetConnection()) return;
            var scanDataList = db.Fetch<dynamic>("SELECT * FROM ScanData");
            foreach (var scanData in scanDataList)
            {
                if (await SendToServer(scanData))
                {
                    db.Execute("DELETE FROM ScanData WHERE Id = @0", scanData.Id);
                    Debug.WriteLine($"Pushed tag {scanData.Epc} : {scanData.Baca_Ke} to server.");
                }
                else
                {
                    break;
                }
            }
        }

        private async Task<bool> SendToServer(dynamic scanData)
        {
            try
            {
                //var client = new HttpClient();
                //var request = new HttpRequestMessage(HttpMethod.Post, apiEndpoint + "/tagrecord");
                //var jsonContent = JsonConvert.SerializeObject(scanData);
                //var content = new StringContent(jsonContent, null, "application/json");
                //request.Content = content;

                //var response = await client.SendAsync(request);
                //response.EnsureSuccessStatusCode();

                //Console.WriteLine(await response.Content.ReadAsStringAsync());

                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5000/tagrecord");



                var cs = JsonConvert.SerializeObject(scanData);
                var content = new StringContent(cs, null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                Console.WriteLine(await response.Content.ReadAsStringAsync());


                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        private bool HasInternetConnection()
        {
            try
            {
                using (var ping = new System.Net.NetworkInformation.Ping())
                {
                    var reply = ping.Send("8.8.8.8", 1000);
                    return reply.Status == System.Net.NetworkInformation.IPStatus.Success;
                }
            }
            catch
            {
                return false;
            }
        }
    }

    public class ReadCountChangedEventArgs : EventArgs
    {
        public TagRecord Tag { get; }
        public DateTime ChangeTime { get; }
        public int BacaKe { get; }

        public ReadCountChangedEventArgs(TagRecord tag, DateTime changeTime, int bacaKe)
        {
            Tag = tag;
            ChangeTime = changeTime;
            BacaKe = bacaKe;
        }
    }
}

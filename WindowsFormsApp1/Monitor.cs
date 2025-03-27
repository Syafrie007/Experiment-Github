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
using RestSharp;
using WindowsFormsApp1;

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
        //private readonly Timer pushTimer;
        private readonly Database db;
        private readonly Dictionary<string, TagRecord> monitoredTags = new Dictionary<string, TagRecord>();
        private readonly Dictionary<string, int> bacaKe = new Dictionary<string, int>();
        private readonly List<Tuple<TagRecord ,DateTime ,int >> ListDataBaca=new List<Tuple<TagRecord, DateTime, int>>();


        private TimeSpan apiReqTimeout = TimeSpan.FromSeconds(5);
        
        
        public event EventHandler<ReadCountChangedEventArgs> ReadCountChanged;
        public TimeSpan RequiredTimeDiff { get; set; } = TimeSpan.FromMinutes(5);
        public string NamaPerangkat { get; set; }
        private bool isMonitoring = false;

        public ReadCountMonitor(string apiEndpoint, int pushIntervalSeconds)
        {
            this.apiEndpoint = apiEndpoint;
            db = new Database($"Data Source={dbPath};Version=3;", "Sqlite");
            EnsureDatabaseCreated();
            //pushTimer = new Timer(pushIntervalSeconds * 1000);
            //pushTimer.Elapsed += async (s, e) => await PushDataToServerFromTemp();
            _=Monitor();
            Debug.WriteLine("ReadCountMonitor initialized.");
        }

        public void Start()
        {
            if (isMonitoring) return;
            isMonitoring = true;
            //pushTimer.Start();
            Debug.WriteLine("Monitoring started.");
        }

        public void Stop()
        {
            isMonitoring = false;
            //pushTimer.Stop();
            Debug.WriteLine("Monitoring stopped.");
        }

        private async Task Monitor()
        {
            try
            {
                while (true)
                {
                    if (!isMonitoring)
                    {
                        await Task.Delay(1000);
                        continue;
                    }

                    await PushDataToServerFromTemp();
                    await Task.Delay(1000);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
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
                        Waktu_Scan DATETIME,
                        Baca_ke INTEGER
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

        private async void HandleReadCountChange(object sender, PropertyChangedEventArgs e)
        {
            if (!isMonitoring || e.PropertyName != nameof(TagRecord.ReadCount)) return;

            var tag = sender as TagRecord;
            if (tag == null) return;

            var now = DateTime.UtcNow;
            var epc = tag.EPC;
            bool isExistingTag = lastReadTimes.ContainsKey(epc);

            if (!isExistingTag || (now - lastReadTimes[epc]) >= RequiredTimeDiff)
            {

                lastReadTimes[epc] = now;

                if (await CekServer())
                {
                    var obj = new ApiTagRecord()
                    {
                        Baca_ke = tag.ReadCount,
                        Epc = tag.EPC,
                        Guid = Guid.NewGuid().ToString(),
                        Nama_Perangkat = tag.NamaPerangkat,
                        Waktu_Scan = DateTime.Now
                    };

                    await SendToServer(obj);
                    this.bacaKe[tag.EPC] = obj.Baca_ke;
                    ReadCountChanged?.Invoke(this, new ReadCountChangedEventArgs(tag, now, this.bacaKe[epc]));
                }
                else
                {
                    var last = db.Fetch<DbTagRecord>("SELECT * FROM ScanData where Epc=@0 ORDER BY Waktu_Scan DESC Limit 1", tag.EPC)
                               .FirstOrDefault();

                    //tambah baca ke
                    if (last != null)
                    {
                        this.bacaKe[tag.EPC] = last.Baca_ke + 1;
                    }


                    if (this.bacaKe.TryGetValue(epc, out var ke))
                    {

                        ListDataBaca.Add(new Tuple<TagRecord, DateTime, int>(tag,now,ke));
                    }
                    else
                    {
                        this.bacaKe.Add(epc, 1);
                        ListDataBaca.Add(new Tuple<TagRecord, DateTime, int>(tag, now, 1));
                    }

                    SaveToDatabase(tag);
                    ReadCountChanged?.Invoke(this, new ReadCountChangedEventArgs(tag, now, this.bacaKe[epc]));
                }

            }

            Debug.WriteLine(string.Format("Tag {0} ReadCount updated to {1}. Baca Ke {2}.", epc, tag.ReadCount, bacaKe[epc]));
        }

        private void SaveToDatabase(TagRecord tag)
        {

            db.Insert("ScanData", "Id", new ApiTagRecord()
            {
                Guid = Guid.NewGuid().ToString(),
                Nama_Perangkat = NamaPerangkat,
                Epc = tag.EPC,
                Waktu_Scan = DateTime.UtcNow,
                Baca_ke = this.bacaKe[tag.EPC]
            });


            Debug.WriteLine($"Saved tag {tag.EPC}-baca ke : {bacaKe[tag.EPC]} to database.");
        }

        private async Task PushDataToServerFromTemp()
        {

            //cek server
            if (!isMonitoring || ! (await CekServer())) return;


            var scanDataList = db.Fetch<DbTagRecord>(
                "SELECT * FROM ScanData ORDER BY Waktu_Scan ASC");
            if (scanDataList.Count > 0)
            {
                Debug.WriteLine($"Terdapat {scanDataList.Count} data temporary yang akan dipush ke server.");
            }

            foreach (var scanData in scanDataList)
            {
                var ok = await SendToServer(new ApiTagRecord()
                {
                    Baca_ke = scanData.Baca_ke,
                    Epc = scanData.Epc,
                    Guid = scanData.Guid,
                    Nama_Perangkat = scanData.Nama_Perangkat,
                    Waktu_Scan = scanData.Waktu_Scan

                });

                if (ok)
                {
                    db.Execute("DELETE FROM ScanData WHERE Id = @0", scanData.Id);
                    Debug.WriteLine($"Pushed tag {scanData.Epc} : {scanData.Baca_ke} to server.");
                }
                else
                {
                    break;
                }
            }
        }

        public async Task<bool> SendToServer(ApiTagRecord scanData)
        {
            try
            {

                var lastRecord = await GetTagScanTerakhir(scanData.Epc);

                if (lastRecord.StatusCode != HttpStatusCode.OK)
                    return false;


                if (lastRecord.Data == null)
                {
                    Console.WriteLine($"Belum ada data diserver, baca ke akan diset ke 1 ");

                    scanData.Baca_ke = 1;
                }
                else
                {
                    Console.WriteLine($"Data terakhir epc {scanData.Epc}, baca ke={lastRecord.Data.Baca_ke}.");

                    scanData.Baca_ke = lastRecord.Data.Baca_ke + 1;
                }


                var options = new RestClientOptions(apiEndpoint)
                {
                    Timeout = apiReqTimeout
                };

                var client = new RestClient(options);
                var request = new RestRequest("/tagrecord", Method.Post);
                request.AddHeader("Content-Type", "application/json");
                var body = JsonConvert.SerializeObject(scanData);
                request.AddStringBody(body, DataFormat.Json);
                RestResponse response = await client.ExecuteAsync(request);
                if (response.StatusCode ==HttpStatusCode.OK)
                {
                    Console.WriteLine($"Berhasil push ke server. " + response.Content);
                    return true;
                }
                else
                {
                    Console.WriteLine($"Gagal push data keserver!. Status: {response.StatusCode}");
                    return false;
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                return false;
            }
        }

        public async Task< bool> CekServer()
        {
            try
            {
                var options = new RestClientOptions(apiEndpoint)
                {
                    Timeout = TimeSpan.FromMilliseconds(50),
                };

                var client = new RestClient(options);
                var request = new RestRequest("/cek", Method.Get);
                RestResponse response = await client.ExecuteAsync(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Debug.WriteLine(string.Format($"Server OK"));
                    return true;
                }
                else
                {
                    Debug.WriteLine($"Server Bermasalah, {response.StatusCode}");
                    return false;
                }

            }
            catch
            {
                return false;
            }
        }

        public async Task<RestResponse<ApiTagRecord>> GetTagScanTerakhir(string epc)
        {
            try
            {
                var options = new RestClientOptions(apiEndpoint)
                {
                    Timeout = apiReqTimeout,
                };

                var client = new RestClient(options);
                var request = new RestRequest($"/tagrecord/epcTagTerakhir/{epc}", Method.Get);
                var response = await client.ExecuteAsync<ApiTagRecord>(request);
                return response;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                return null;
            }
        }

        public async Task<RestResponse<List<ApiTagRecord>>> GetAllTagScanFromServer(string epc)
        {
            try
            {
                var options = new RestClientOptions(apiEndpoint)
                {
                    Timeout = apiReqTimeout,
                };

                var client = new RestClient(options);
                var request = new RestRequest($"/tagrecord/epc/{epc}", Method.Get);
                var response = await client.ExecuteAsync<List<ApiTagRecord>>(request);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                return null;
            }
        }

        public List<DbTagRecord> GetAllTagScanFromTemp(string epc)
        {
            var scanDataList = db.Fetch<DbTagRecord>(
                "SELECT * FROM ScanData Where Epc=@0",epc);
            return scanDataList;
        }

        public async void ShowScanTagHistory(string epc)
        {

            if (await CekServer())
            {
                var res = await GetAllTagScanFromServer(epc);
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    using (var f = new FormTagScanViewer())
                    {
                        f.Text = "Data tag scan dari SERVER";
                        f.dgv.DataSource = res.Data;
                        f.dgv.Columns[0].Visible = false;
                        f.ShowDialog();
                    }
                }
            }
            else
            {
                using (var f = new FormTagScanViewer())
                {
                    f.Text = "Data tag scan dari TEMPORARY DATA";

                    f.dgv.DataSource = GetAllTagScanFromTemp(epc);
                    f.dgv.Columns[0].Visible = false;
                    f.dgv.Columns[1].Visible = false;
                    f.ShowDialog();
                }
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

    public class ApiTagRecord
    {
        public string Guid { get; set; }
        public string Nama_Perangkat { get; set; }
        public string Epc { get; set; }
        public DateTime Waktu_Scan { get; set; }
        public int Baca_ke { get; set; }
    }

    public class DbTagRecord
    {

        public int Id { get; set; }
        public string Guid { get; set; }
        public string Nama_Perangkat { get; set; }
        public string Epc { get; set; }
        public DateTime Waktu_Scan { get; set; }
        public int Baca_ke { get; set; }
    }


}

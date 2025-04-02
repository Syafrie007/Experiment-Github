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
using System.Threading;

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

        private List<Tuple<string, DateTime>> logs = new List<Tuple<string, DateTime>>();

        private readonly string dbPath = "temp.db";
        private readonly string apiEndpoint;
        //private readonly Timer pushTimer;
        private readonly Database db;
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        private readonly Dictionary<string, DateTime> lastReadTimes = new Dictionary<string, DateTime>();
        private readonly Dictionary<string, TagRecord> monitoredTags = new Dictionary<string, TagRecord>();
        private readonly Dictionary<string, int> bacaKe = new Dictionary<string, int>();
        private readonly List<Tuple<TagRecord, DateTime, int>> ListDataBaca = new List<Tuple<TagRecord, DateTime, int>>();
        private readonly Dictionary<int, int> jumlahAkumulasiPerPeriodeScan = new Dictionary<int, int>();

        private TimeSpan apiReqTimeout = TimeSpan.FromSeconds(5);


        public event EventHandler<ReadCountChangedEventArgs> ReadCountChanged;


        //public properties
        public TimeSpan RequiredTimeDiff { get; set; } = TimeSpan.FromMinutes(5);
        public string NamaPerangkat { get; set; }

        public Settings Settings { get; set; }


        private string _WaBlastToken;
        private bool isMonitoring = false;

        public ReadCountMonitor(string settingFileName)
        {
            if (!File.Exists(settingFileName))
            {
                throw new Exception("Belum ada pengaturan");
            }

            Settings = new Settings(settingFileName);
            Settings.Reload();

            _WaBlastToken = Settings.WaBlasToken;
            this.apiEndpoint = Settings.ApiEndPoint;
            db = new Database($"Data Source={dbPath};Version=3;", "Sqlite");
            EnsureDatabaseCreated();
            //pushTimer = new Timer(pushIntervalSeconds * 1000);
            //pushTimer.Elapsed += async (s, e) => await PushDataToServerFromTemp();
            _ = MulaiMonitor();
            Log("ReadCountMonitor initialized.");
        }

        public void Start()
        {
            if (isMonitoring) return;
            isMonitoring = true;
            //pushTimer.Start();
            Log("Monitoring started.");
        }

        public void Stop()
        {
            isMonitoring = false;
            //pushTimer.Stop();
            Log("Monitoring stopped.");
        }

        public void Reset()
        {
            bacaKe.Clear();
            ListDataBaca.Clear();
            lastReadTimes.Clear();
            jumlahAkumulasiPerPeriodeScan.Clear();
            Log($"Data direset.");
        }


        private async Task MulaiMonitor()
        {
            try
            {
                Log($"Memulai monitor tag scan.");
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
            catch (Exception ex)
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
                        Baca_ke INTEGER,
                        Akumulasi_Per_Periode INTEGER
                    );";
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            Log("Database ensured.");
        }

        public async void MonitorTag(TagRecord tag)
        {
            if (!monitoredTags.ContainsKey(tag.EPC))
            {
                monitoredTags[tag.EPC] = tag;
                tag.PropertyChanged += HandleReadCountChange;
                Log($"Monitoring tag {tag.EPC} started.");



                //baca 1
                await ValidasiTagScan(tag);
            }
        }

        private async void HandleReadCountChange(object sender, PropertyChangedEventArgs e)
        {
            //if (!isMonitoring || e.PropertyName != nameof(TagRecord.ReadCount)) return;
            if (sender is TagRecord)
            {
                var tag = sender as TagRecord;
                await ValidasiTagScan(tag);
            }
        }

        private async Task ValidasiTagScan(TagRecord tag)
        {
            if (tag == null) return;

            var now = DateTime.Now;
            var epc = tag.EPC;
            bool isExistingTag = lastReadTimes.ContainsKey(epc);

            if (!isExistingTag || (now - lastReadTimes[epc]) > RequiredTimeDiff)
            {

                lastReadTimes[epc] = now;

                if (this.bacaKe.TryGetValue(epc, out var ke))
                {
                    ke += 1;

                    this.bacaKe[tag.EPC] = ke;
                    ListDataBaca.Add(new Tuple<TagRecord, DateTime, int>(tag, now, ke));
                }
                else
                {
                    this.bacaKe[tag.EPC] = 1;
                    ke = 1;
                    ListDataBaca.Add(new Tuple<TagRecord, DateTime, int>(tag, now, 1));
                }


                if (jumlahAkumulasiPerPeriodeScan.ContainsKey(ke))
                {
                    jumlahAkumulasiPerPeriodeScan[ke] = jumlahAkumulasiPerPeriodeScan[ke] + 1;
                }
                else
                {
                    jumlahAkumulasiPerPeriodeScan[ke] = 1;
                }

                Log($"Periode {ke} Jumlah akumulasi tag scan valid = {jumlahAkumulasiPerPeriodeScan[ke]}");

              

                if (await CekServer())
                {
                    var obj = new ApiTagRecord()
                    {
                        Epc = tag.EPC,
                        Guid = Guid.NewGuid().ToString(),
                        Nama_Perangkat = NamaPerangkat,
                        Waktu_Scan = DateTime.Now,
                        Baca_ke = ke
                    };

                    await SendToServer(obj);

                    //kirim WA ke user
                    if (obj.Baca_ke == 1)
                    {
                        var pesan1 = $"Hai, ###nama tercatat pada {obj.Nama_Perangkat} {obj.Waktu_Scan.ToString("dd/MM/yyyy HH:mm:ss")}, Scan Ke {obj.Baca_ke}";
                        Log($"Kirim WA ke User {obj.Epc}: {pesan1}");
                        _ = KirimWAKeUser(epc,pesan1);
                    }

                    //kirim WA ke Admin
                    var pesan2 = $"Periode {ke} Jumlah akumulasi tag scan valid = {jumlahAkumulasiPerPeriodeScan[ke]}";
                    Log($"Kirim WA ke Admin: {pesan2}");
                    _ = KirimWAKeAdmin(pesan2);

                    ReadCountChanged?.Invoke(this, new ReadCountChangedEventArgs(tag, now, this.bacaKe[epc]));
                }
                else
                {
                    SaveToDatabase(tag);
                    ReadCountChanged?.Invoke(this, new ReadCountChangedEventArgs(tag, now, this.bacaKe[epc]));
                }

                Log(string.Format("Tag {0} ReadCount updated to {1}. Baca Ke {2}.", epc, tag.ReadCount, bacaKe[epc]));
            }

        }

        private void SaveToDatabase(TagRecord tag)
        {

            db.Insert("ScanData", "Id", new DbTagRecord()
            {
                Guid = Guid.NewGuid().ToString(),
                Nama_Perangkat = NamaPerangkat,
                Epc = tag.EPC,
                Waktu_Scan = DateTime.Now,
                Baca_ke = this.bacaKe[tag.EPC],
                Akumulasi_Per_Periode=jumlahAkumulasiPerPeriodeScan[bacaKe[tag.EPC]]
            });


            Log($"Saved tag {tag.EPC}-baca ke : {bacaKe[tag.EPC]} to database.");
        }

        private async Task PushDataToServerFromTemp()
        {

            //cek server
            if (!isMonitoring || !(await CekServer())) return;

            await _semaphore.WaitAsync();

            try
            {

                var scanDataList = db.Fetch<DbTagRecord>(
                    "SELECT * FROM ScanData ORDER BY Waktu_Scan ASC");
                if (scanDataList.Count > 0)
                {
                    Log($"Terdapat {scanDataList.Count} data temporary yang akan dipush ke server.");
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
                        Log($"Pushed tag {scanData.Epc} : {scanData.Baca_ke} to server.");

                        if (scanData.Baca_ke == 1)
                        {
                            var pesan1 = $"Hai, ###nama tercatat pada {scanData.Nama_Perangkat} {scanData.Waktu_Scan.ToString("dd/MM/yyyy HH:mm:ss")}, Scan Ke {scanData.Baca_ke}";
                            Log($"Kirim WA ke User {scanData.Epc}: {pesan1}");
                            
                            _ = KirimWAKeUser(scanData.Epc, pesan1);
                        }

                        var pesan2 = $"Periode {scanData.Baca_ke} Jumlah akumulasi tag scan valid = {scanData.Akumulasi_Per_Periode}";
                        Log($"Kirim WA ke Admin: {pesan2}");
                        _ = KirimWAKeAdmin(pesan2);

                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch(Exception ex)
            {
                Log("Terjadi kesalahan.\n" + ex.ToString());
            }
            finally
            {
                _semaphore.Release();
            }

        }

        public async Task<bool> SendToServer(ApiTagRecord scanData)
        {
            try
            {
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
                if (response.StatusCode == HttpStatusCode.OK)
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                return false;
            }
        }

        public async Task<bool> CekServer()
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
                    Log(string.Format($"Server OK"));
                    return true;
                }
                else
                {
                    Log($"Server Bermasalah, {response.StatusCode}");
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
            catch (Exception ex)
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

        public async Task<bool> KirimWAKeUser(string epc, string pesan)
        {
            if (await CekServer())
            {
                var noTelp = await GetNoTelp(epc);
                if (noTelp.Data.Count > 0)
                {
                    pesan = pesan.Replace("###nama", noTelp.Data.First().Nama);
                    return await SendWaMessageCore(_WaBlastToken, noTelp.Data.First().No_hp, pesan);
                }
                else
                {
                    Log("No telp user tidak tersedia.");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> KirimWAKeAdmin(string pesan)
        {
            return await SendWaMessageCore(_WaBlastToken, Settings.HpAdmin, pesan);
        }

        public async Task<RestResponse<List<ApiNoTelp>>> GetNoTelp(string epc)
        {
            try
            {
                var options = new RestClientOptions(apiEndpoint)
                {
                    Timeout = apiReqTimeout,
                };

                var client = new RestClient(options);
                var request = new RestRequest($"/nohp/epc/{epc}", Method.Get);
                var response = await client.ExecuteAsync<List<ApiNoTelp>>(request);
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
                "SELECT * FROM ScanData Where Epc=@0", epc);
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

                        var l = res.Data.OrderByDescending(x => x.Waktu_Scan).ToList();
                        foreach (var item in l)
                        {
                            item.Waktu_Scan = item.Waktu_Scan.ToLocalTime();
                        }

                        f.dgv.DataSource = l;
                        f.dgv.Columns[nameof(ApiTagRecord.Waktu_Scan)].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
                        f.dgv.Columns[0].Visible = false;

                        //remove log tab
                        f.tabControl1.TabPages.RemoveAt(1);
                        f.tabControl1.TabPages.RemoveAt(1);

                        f.ShowDialog();
                    }
                }
            }
            else
            {
                using (var f = new FormTagScanViewer())
                {
                    f.Text = "Data tag scan dari TEMPORARY DATA";

                    var data = GetAllTagScanFromTemp(epc);
                    foreach (var item in data)
                    {
                        item.Waktu_Scan = item.Waktu_Scan.ToLocalTime();
                    }

                    f.dgv.DataSource = data.OrderByDescending(x => x.Waktu_Scan).ToList();
                    f.dgv.Columns[0].Visible = false;
                    f.dgv.Columns[1].Visible = false;
                    f.dgv.Columns[nameof(ApiTagRecord.Waktu_Scan)].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
                    //remove log tab
                    f.tabControl1.TabPages.RemoveAt(1);

                    f.ShowDialog();
                }
            }
        }

        public void ShowLog()
        {
            using (var f = new FormTagScanViewer())
            {
                f.Text = "Data Log";

                f.SetLog(logs);

                //remove log tab
                f.tabControl1.TabPages.RemoveAt(0);
                f.tabControl1.TabPages.RemoveAt(1);

                f.ShowDialog();
            }
        }

        public async Task<bool> SendWaMessageCore(string token, string phone, string message)
        {
            try
            {
                var options = new RestClientOptions(apiEndpoint)
                {
                    Timeout = apiReqTimeout,
                };

                var client = new RestClient(options);
                var request = new RestRequest("/send-wa", Method.Post);
                request.AddHeader("Content-Type", "application/json");
                var body = $@"{{""phone"": ""{phone}"", ""message"": ""{message}""}}";
                request.AddStringBody(body, DataFormat.Json);
                RestResponse response = await client.ExecuteAsync(request);
                Log($"Kirim pesan status kode: {response.Content}");

                return response.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                return false;
            }
        }

        private void Log(string pesan)
        {
            Debug.WriteLine(pesan);
            logs.Add(new Tuple<string, DateTime>(pesan, DateTime.Now));
        }

        public static void ShowSettingsForm(string fname)
        {
            using (var f = new FormTagScanViewer())
            {
                f.Text = "Settings";

                f.Settings = new Settings(fname);
                f.Settings.Reload();
                //remove log tab
                f.tabControl1.TabPages.RemoveAt(0);
                f.tabControl1.TabPages.RemoveAt(0);

                f.ShowDialog();
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
    public class ApiNoTelp
    {
        public int Id { get; set; }

        public string Nama { get; set; }
        public string Epc { get; set; }
        public string No_hp { get; set; }
    }

    public class DbTagRecord
    {

        public int Id { get; set; }
        public string Guid { get; set; }
        public string Nama_Perangkat { get; set; }
        public string Epc { get; set; }
        public DateTime Waktu_Scan { get; set; }
        public int Baca_ke { get; set; }

        public int Akumulasi_Per_Periode { get; set; }
    }

    public class Settings
    {

        public Settings(string file)
        {
            fileName = file;
        }

        public string WaBlasToken { get; set; }

        public string HpAdmin { get; set; }
        
        public string ApiEndPoint { get; set; }


        private string fileName;

        public void Save()
        {
            var str=JsonConvert.SerializeObject(this);
            File.WriteAllText(fileName, str);

        }

        public void Reload()
        {
            var str = File.ReadAllText(fileName);
            var s=JsonConvert.DeserializeObject<Settings>(str);

            this.WaBlasToken = s.WaBlasToken;
            HpAdmin = s.HpAdmin;
            ApiEndPoint = s.ApiEndPoint;

        }
    }

}

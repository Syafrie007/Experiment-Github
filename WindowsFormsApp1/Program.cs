using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(
    "NDY1ODM3QDMxMzkyZTMyMmUzMGRhNVJ0VlEwTEx5VE5yR0pncGlNSzMrVDFDbkdMUm9nbW80UlRmMERzeG89;NDY1ODM4QDMxMzkyZTMyMmUzMGFjQ0pxeXBYWmpDZzZ2d29WRytTV1pkVVNSdTJvK2x5aXZyNWJMYWlWOHM9;NDY1ODM5QDMxMzkyZTMyMmUzMGVVb1lwOGNlNUR2ZHpIQUFubXhvY2VtQ1VNT1Y3akNxenY3WmwxUFE5ZHc9;NDY1ODQwQDMxMzkyZTMyMmUzMGEzREVlMTV5bW1UaHFpdHEyOEM4VUdBN0xWcU9iblQwVUcya1kwQVZHaVU9;NDY1ODQxQDMxMzkyZTMyMmUzMEVLK25aaDJDYXdVWDNrZ21nd3JOc0xLdmx4Y0dpdHAzNWRuZ2hFczFaNDg9;NDY1ODQyQDMxMzkyZTMyMmUzMEF3NTFENER6NG9aRzdqZFlTNTE1cTFLb2RkWGdET3lnZDhpTnVCa2kyb0U9;NDY1ODQzQDMxMzkyZTMyMmUzMFpFQXlQVHhKM2ZzRm1Uc1ZyMnBmU2YweTlFdlNuNEtJZFhNS1crV1lhc2M9;NDY1ODQ0QDMxMzkyZTMyMmUzMGdzSjBjSExtRDZ2ZVlyNGJuK1dxUVMwRW5XYTZIbjl0SUl6dDk3SG5TZmc9;NDY1ODQ1QDMxMzkyZTMyMmUzMEttTXI0d3Y3c1B0NURrTk1zWk9EbjdmelRBWWQwcjlMaHF3L3k0ckI5d3c9;NDY1ODQ2QDMxMzkyZTMyMmUzMExqSVBYbGtlVE5Cb0NOcUJqL1NtYjArZzR6bUZZMENjN0JWZkNPWGs3RjA9");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form2());
        }
    }
}

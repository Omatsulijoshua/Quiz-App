using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quiz_App
{
    static class Program
    {
        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        [STAThread]
        static void Main()
        {

            // Set EPPlus license context once for the whole app
           // ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Make the app DPI aware
            if (Environment.OSVersion.Version.Major >= 6)
                SetProcessDPIAware();

            Application.Run(new Home());
        }
    }
}

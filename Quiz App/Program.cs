using OfficeOpenXml;
using System;
using System.Runtime.InteropServices;
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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (Environment.OSVersion.Version.Major >= 6)
            {
                SetProcessDPIAware();
            }

            connection_class.Initialize();
            TheorySchemaInstaller.TryEnsureTheoryInfrastructure(out _);
            Application.Run(new Home(/*1, 101, 10*/));
        }
    }
}

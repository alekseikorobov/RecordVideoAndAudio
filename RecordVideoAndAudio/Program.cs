using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RecordVideoAndAudio
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var f = new Form1();
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Application.Run(f);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            File.AppendAllText("Log.txt", $"{DateTime.Now:yyyy-MM-dd HH\\:mm\\:ss} Application_ThreadException\t sender - {sender}\t Exception - IsTerminating - {e.IsTerminating} ExceptionObject - {e.ExceptionObject}");
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            File.AppendAllText("Log.txt", $"{DateTime.Now:yyyy-MM-dd HH\\:mm\\:ss} Application_ThreadException\t sender - {sender}\t Exception - {e.Exception}");
            MessageBox.Show(e.Exception.ToString());
        }
    }
}

using System;
using System.Linq;
using System.Windows.Forms;

namespace OpenBots.Updater
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Count() == 0)
            {
                MessageBox.Show("Update Tool requires a package argument!");
                Application.Exit();
            }
            else
                Application.Run(new frmUpdating(args[0]));
        }
    }
}

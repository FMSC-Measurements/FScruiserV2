using System;
using System.Diagnostics;
using System.Windows.Forms;
using FSCruiser.Core;
using FScruiser.Core.Services;

namespace FSCruiser.WinForms
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
#if PocketPC
        [MTAThread]
#else

        [STAThread]
#endif
        static void Main()
        {
            //read command line arguments
            var args = Environment.GetCommandLineArgs();
            string dalPath = null;
            if (args.Length > 1)
            {
                dalPath = args[1];
            }

            AppDomain.CurrentDomain.UnhandledException += FMSC.Utility.ErrorHandling.ErrorHandlers.UnhandledException;


            DialogService.Instance = new WinFormsDialogService();
            using (SoundService.Instance = new WinFormsSoundService())
            using (ViewController viewController = new ViewController())
            using (ApplicationController appController = new ApplicationController(viewController))
            {
                if (dalPath != null)
                {
                    appController.OpenFile(dalPath);
                }

                viewController.Run();
            }
            Debug.Close();
            Application.Exit();// forces any extra forms (splash screen) to close
        }
    }
}
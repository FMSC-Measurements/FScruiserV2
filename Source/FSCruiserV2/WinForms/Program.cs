using System;
using System.Diagnostics;
using System.Windows.Forms;
using FSCruiser.Core;
using FScruiser.Core.Services;

namespace FSCruiser.WinForms
{
    internal static class Program
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

            NBug.Settings.UIMode = NBug.Enums.UIMode.Full;
            NBug.Settings.StoragePath = NBug.Enums.StoragePath.WindowsTemp;
            NBug.Settings.Destinations.Add(new NBug.Core.Submission.Tracker.Redmine()
            {
                ApiKey = "6cf4343091c7509dbf27d6afd84a267189b9d3b9",
                CustomSubject = "CrashReport",
                Url = "http://fmsc-projects.herokuapp.com/projects/fscruiser/",
                ProjectId = "fscruiser",
                TrackerId = "5",
                PriorityId = "1",
                StatusId = "1"
            });

            NBug.Settings.ReleaseMode = true;//only create error reports if debugger not attached
            NBug.Settings.StopReportingAfter = 60;

            AppDomain.CurrentDomain.UnhandledException += NBug.Handler.UnhandledException;
            Application.ThreadException += NBug.Handler.ThreadException;

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
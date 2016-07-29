using System;
using System.Diagnostics;
using System.Windows.Forms;
using FSCruiser.Core;
using OpenNETCF.Threading;

namespace FSCruiser.WinForms
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
#if NetCF

        [MTAThread]
#else
        [STAThread]
#endif
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += FMSC.Utility.ErrorHandling.ErrorHandlers.UnhandledException;

            using (NamedMutex mutex = new NamedMutex(false, "Global\\" + "FScruiser"))
            {
                if (mutex.WaitOne(0, false))
                {
                    //not already running

                    //PreJit();
                    using (ViewController viewController = new ViewController())
                    using (ApplicationController appController = new ApplicationController(viewController))
                    {
                        if (args.Length > 1)
                        {
                            appController.OpenFile(args[1]);
                        }

                        viewController.Run();
                    }
                    Debug.Close();
                }
                else
                {
                    //is already running
                    string message = "FScruiser is already running\r\n";
                    if (ViewController.PlatformType == FMSC.Controls.PlatformType.WM)
                    {
                        message += "To halt or activate background programs, go to Settings->System->Memory";
                    }

                    MessageBox.Show(message);
                    return;
                }
            }
        }
    }
}
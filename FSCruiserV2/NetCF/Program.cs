using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Logger;
using System.Threading;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using FSCruiser.Core;

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
            
            AppDomain.CurrentDomain.UnhandledException += FMSC.Utility.ErrorHandling.ErrorHandlers.UnhandledException;
            //PreJit();
            using (ViewController viewController = new ViewController())
            using (ApplicationController controller = new ApplicationController(viewController))
            {
                controller.Run();
            }
            Debug.Close();
        }

        //http://stackoverflow.com/questions/548915/preloading-assemblies
        //private static void PreJit()
        //{
        //    ThreadPool.QueueUserWorkItem((t) =>
        //    {
        //        Thread.Sleep(1000); // Or whatever reasonable amount of time
        //        try
        //        {
        //            XmlSerializer c = new XmlSerializer(typeof(object));
        //        }
        //        catch (Exception) { }

        //        try
        //        {
        //            FormDataEntry de = new FormDataEntry();
        //        }
        //        catch { }

        //        try
        //        {
        //            LayoutTreeBased l = new LayoutTreeBased();
        //        }
        //        catch { }

        //    });
        //}
    }
}
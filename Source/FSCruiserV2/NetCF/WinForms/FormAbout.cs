using System;
using System.IO;
using System.Reflection;

//using System.Linq;
using System.Windows.Forms;
using FSCruiser.Core;

namespace FSCruiser.WinForms
{
    public partial class FormAbout : Form
    {
        
        private System.Windows.Forms.MainMenu _dontDeleteMe;

        public FormAbout()
        {
            InitializeComponent();

            if (ViewController.PlatformType == FMSC.Controls.PlatformType.WinCE)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else if (ViewController.PlatformType == FMSC.Controls.PlatformType.WM)
            {
                //on windows mobile 6.5 we need to have a menu 
                //because without it the OK button doesn't get displayed
                this.Menu = new System.Windows.Forms.MainMenu();
            }


            this.TopMost = true;
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            this.Activate();
        }

        private void FormAbout_Load(object sender, EventArgs e)
        {
            // Get the version from ApplicationController
            label1.Text = "Version " + Core.Constants.FSCRUISER_VERSION;
            this._exeLBL.Text = AppDomain.CurrentDomain.FriendlyName;
            this._exeDOB_LBL.Text = "Installed: " + File.GetCreationTime(Assembly.GetExecutingAssembly().GetName().CodeBase).ToString();
            bool srFound = System.IO.File.Exists("\\Windows\\mscoree.dll");
            if (!srFound)
            {
                MessageBox.Show("Unable to locate resource file '\\Windows\\mscoree.dll', please reinstall Compact Framework");
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            // Check the device date
            if (DateTime.Now < DateTime.Parse(Core.Constants.FSCRUISER_VERSION))
            {
                MessageBox.Show("The date on your mobile device is not correct. Please update the date and time before using FScruiser.", "Warning");
                // Controller._cDal.LogMessage("FScruiser", "User notified of incorrect date on mobile device.", "W");
            }
        }
    }
}
using System;
using System.Windows.Forms;
using FSCruiser.Core;
using Microsoft.AppCenter.Crashes;

namespace FSCruiser.WinForms
{
    public partial class FormAbout : Form
    {
        int _clickCount = 0;

        public FormAbout()
        {
            InitializeComponent();

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            label1.Text = "Version " + Constants.FSCRUISER_VERSION;
            //this._dob_LBL.Text = "DOB: " + File.GetCreationTime(Assembly.GetExecutingAssembly().GetName().CodeBase).ToString();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            var clickCount = _clickCount++;
            if(clickCount % 6 == 5)
            {
                Crashes.GenerateTestCrash();
            }
        }
    }
}
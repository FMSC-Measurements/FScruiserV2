using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace FSCruiserV2.Forms
{
    public partial class FormAbout : Form
    {
        public FormAbout()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            label1.Text = "Version " + Constants.FSCRUISER_VERSION;
            this._exeLBL.Text = AppDomain.CurrentDomain.FriendlyName;
            //this._dob_LBL.Text = "DOB: " + File.GetCreationTime(Assembly.GetExecutingAssembly().GetName().CodeBase).ToString();
        }
    }
}

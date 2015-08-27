using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace FSCruiserV2.Test
{
    public partial class OnTallyTestForm : Form
    {


        public OnTallyTestForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DataEntryTests tester = new DataEntryTests();

            tester.TestTreeTally();
        }
    }
}
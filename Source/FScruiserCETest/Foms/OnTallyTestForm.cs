using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;


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
            Cursor.Current = Cursors.WaitCursor;
            base.OnLoad(e);
            DataEntryTests tester = new DataEntryTests();


            var outputWriter = new StringWriter();

            tester.TestTreeTally(outputWriter);

            this.textBox1.Text = outputWriter.ToString();
            Cursor.Current = Cursors.Default;
        }
    }
}
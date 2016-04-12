using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FSCruiser.WinForms.Common
{
    public partial class FixCntTallyControl : UserControl
    {
        public FixCntTallyControl()
        {
            InitializeComponent();
        }

        public void AddTallyRows(IFixCntObject[] objs)
        {
            if (this.Controls.Count > 0)
                this.Controls.Clear();

            int rowHeight = this.Height / objs.Count();

            int offsetY = 0;

            foreach (IFixCntObject o in objs)
            {
                FixCntTallyRow row = new FixCntTallyRow(this.Width, rowHeight, o);

                row.Dock = DockStyle.Fill;
                row.Location = new Point(0, offsetY);

                this.Controls.Add(row);

                offsetY += rowHeight;
            }
        }
    }
}

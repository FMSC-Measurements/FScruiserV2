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
    public partial class FixCntTallyRow : UserControl
    {
        public FixCntTallyRow(int width, int height, IFixCntObject obj)
        {
            InitializeComponent(width, height, obj);

            int buttonZoneWidth = pnlHeaders.Width;
            int buttonWidth = buttonZoneWidth / obj.Tallys.Count;

            int buttonHeight = height - pnlHeaders.Height;
            int offsetX = 0;

            foreach (IFixCntTally tally in obj.Tallys)
            {
                Button button = new Button();
                button.Dock = DockStyle.Fill;
                button.Size = new Size(buttonWidth, buttonHeight);
                button.DataBindings.Add(new Binding("Text", tally, "Count"));
                button.Location = new Point(offsetX, 0);
                button.Click += new EventHandler(tally.Clicked);
                button.TextAlign = ContentAlignment.MiddleCenter;
                button.Font = new Font(FontFamily.GenericSansSerif, 25, FontStyle.Bold);
                button.FlatStyle = FlatStyle.Flat;

                Label label = new Label();
                label.Dock = DockStyle.Fill;
                label.Size = new Size(buttonWidth, 25);
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.Text = tally.Title;
                label.Location = new Point(offsetX, 0);
                label.Font = new Font(FontFamily.GenericSansSerif, 18, FontStyle.Bold);

                pnlButtons.Controls.Add(button);
                pnlHeaders.Controls.Add(label);
            }
        }
    }

    public interface IFixCntObject
    {
        string Name { get; }

        List<IFixCntTally> Tallys { get; }
    }

    public interface IFixCntTally : INotifyPropertyChanged
    {
        string Title { get; }
        string Count { get; }

        void Clicked(object sender, EventArgs e);
    }
}

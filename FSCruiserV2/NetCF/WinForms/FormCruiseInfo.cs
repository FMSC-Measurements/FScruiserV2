using System;
using System.Windows.Forms;

namespace FSCruiser.WinForms
{
    public partial class FormCruiseInfo : Form
    {
        public FormCruiseInfo()
        {
            InitializeComponent();
            if (ViewController.PlatformType == FMSC.Controls.PlatformType.WinCE)
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void FormCruiseInfo_Load(object sender, EventArgs e)
        {
            treeView1.BeginUpdate();
            treeView1.Nodes.Clear();

            treeView1.Nodes.Add("Cruise");

            treeView1.Nodes[0].Nodes.Add("Unit 17");
            treeView1.Nodes[0].Nodes.Add("Unit 18A");
            treeView1.Nodes[0].Nodes.Add("Unit 18B");

            treeView1.Nodes[0].Nodes[0].Nodes.Add("Stratum 01");
            treeView1.Nodes[0].Nodes[0].Nodes.Add("Stratum 02");
            treeView1.Nodes[0].Nodes[0].Nodes.Add("Stratum 03");

            treeView1.Nodes[0].Nodes[0].Nodes[0].Nodes.Add("Sample Group PP");
            treeView1.Nodes[0].Nodes[0].Nodes[0].Nodes.Add("Sample Group SP");

            treeView1.Nodes[0].Nodes[0].Nodes[1].Nodes.Add("Sample Group RF");
            treeView1.Nodes[0].Nodes[0].Nodes[1].Nodes.Add("Sample Group WF");

            treeView1.Nodes[0].Nodes[1].Nodes.Add("Stratum 01");
            treeView1.Nodes[0].Nodes[1].Nodes.Add("Stratum 02");
            treeView1.Nodes[0].Nodes[1].Nodes.Add("Stratum 03");

            treeView1.Nodes[0].Nodes[1].Nodes[0].Nodes.Add("Sample Group PP");
            treeView1.Nodes[0].Nodes[1].Nodes[0].Nodes.Add("Sample Group SP");

            treeView1.Nodes[0].Nodes[1].Nodes[1].Nodes.Add("Sample Group RF");
            treeView1.Nodes[0].Nodes[1].Nodes[1].Nodes.Add("Sample Group WF");

            treeView1.Nodes[0].Nodes[2].Nodes.Add("Stratum 01");
            treeView1.Nodes[0].Nodes[2].Nodes.Add("Stratum 02");
            treeView1.Nodes[0].Nodes[2].Nodes.Add("Stratum 03");

            treeView1.Nodes[0].Nodes[2].Nodes[0].Nodes.Add("Sample Group PP");
            treeView1.Nodes[0].Nodes[2].Nodes[0].Nodes.Add("Sample Group SP");

            treeView1.Nodes[0].Nodes[2].Nodes[1].Nodes.Add("Sample Group RF");
            treeView1.Nodes[0].Nodes[2].Nodes[1].Nodes.Add("Sample Group WF");

            treeView1.EndUpdate();

            treeView1.SelectedNode = null; // doesn't work. trying to un-select cruise node when 1st instantiated
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = treeView1.SelectedNode;

            //   MessageBox.Show(string.Format("You selected: {0}", node.Text));

            string messageString;

            switch (node.Text)
            {
                case "Cruise":
                    messageString = node.Text + ": Whistle Punk\r\nUnits with data: 3 \r\nStrata with data: 3\r\nTotal plots: 22\r\nTotal trees: 2349\r\n Measured trees: 271\r\nTime Cruising: 91 hours";
                    MessageBox.Show(messageString);
                    break;
                case "Unit 17":
                    messageString = node.Text + ": Below the rim\r\nStrata with data: 3\r\nTotal trees: 1117\r\nMeasured trees: 62\r\nTime Cruising: 30 hours";
                    MessageBox.Show(messageString);
                    break;
                case "Unit 18A":
                    messageString = node.Text + ": Above the rim\r\nStrata with data: 3\r\nTotal trees: 1091\r\nMeasured trees: 57\r\nTime Cruising: 28 hours";
                    MessageBox.Show(messageString);
                    break;
                case "Unit 18B":
                    messageString = node.Text + ": South of Hwy 78\r\nStrata with data: 3\r\nTotal trees: 141\r\nMeasured trees: 14\r\nTime Cruising: 8 hours";
                    MessageBox.Show(messageString);
                    break;
                case "Stratum 01":
                    messageString = node.Text + ": STR\r\nTotal trees: 320\r\nMeasured trees: 51";
                    MessageBox.Show(messageString);
                    break;
                case "Stratum 02":
                    messageString = node.Text + ": STR\r\nTotal trees: 290\r\nMeasured trees: 43";
                    MessageBox.Show(messageString);
                    break;
                case "Stratum 03":
                    messageString = node.Text + ": PNT, BAF 30\r\nTotal Plots: 22\r\nTotal trees: 157\r\nAvg Trees / plot: 7.1";
                    MessageBox.Show(messageString);
                    break;
                case "Sample Group PP":
                    messageString = node.Text + ": Ponderosa Pine Saw, 1:5\r\nTotal trees: 46\r\nMeasured trees: 9";
                    MessageBox.Show(messageString);
                    break;
                case "Sample Group SP":
                    messageString = node.Text + ": Sugar Pine Saw, 1:5\r\nTotal trees: 32\r\nMeasured trees: 6";
                    MessageBox.Show(messageString);
                    break;
                case "Sample Group RF":
                    messageString = node.Text + ": Red Fir Saw, 1:10\r\nTotal trees: 41\r\nMeasured trees: 4";
                    MessageBox.Show(messageString);
                    break;
                case "Sample Group WF":
                    messageString = node.Text + ": White Fir Saw, 1:10\r\nTotal trees: 48\r\nMeasured trees: 5";
                    MessageBox.Show(messageString);
                    break;

                default:
                    break;
            }
        }
    }
}
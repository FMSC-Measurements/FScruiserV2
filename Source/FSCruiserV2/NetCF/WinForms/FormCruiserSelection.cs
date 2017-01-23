using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FSCruiser.Core.ViewInterfaces;
using FSCruiser.Core;
using FSCruiser.Core.Models;

namespace FSCruiser.WinForms
{
    public partial class FormCruiserSelection : Form, ICruiserSelectionView
    {
        private FormCruiserSelectionLogic _logicController;
        public FormCruiserSelection(IApplicationController controller)
        {
            _logicController = new FormCruiserSelectionLogic(controller, this);
            this.KeyPreview = true;
            InitializeComponent();
        }

        public string TreeNumberText
        {
            set { _treeNumLBL.Text = value; }
        }

        public string StratumText
        {
            set { _stratumLBL.Text = value; }
        }

        public string SampleGroupText
        {
            set { _sampleGroupLBL.Text = value; }
        }


        public DialogResult ShowDialog(TreeVM tree)
        {
            _logicController.Tree = tree;
            
            return base.ShowDialog();
        }

        protected override void OnLoad(EventArgs e)
        {
            _logicController.HandleLoad();
            base.OnLoad(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            char key = (char)e.KeyValue;
            _logicController.HandleKeyDown(key);
        }

        public void UpdateCruiserList(IList<CruiserVM> cruisers)
        {
            this._crusierSelectPanel.Controls.Clear();

            for (int i = cruisers.Count - 1; i >= 0; i--)
            {
                Button b = new Button();
                b.Dock = DockStyle.Top;
                b.Size = new System.Drawing.Size(240, 20);
                if (i < 9)
                {
                    b.Text = String.Format("&{0}: {1}", i + 1, cruisers[i].Initials);
                }
                else
                {
                    b.Text = String.Format(" {0}", cruisers[i].Initials);
                }
                b.Click +=new EventHandler(button_Click);
                b.Tag = cruisers[i];
                FMSC.Controls.DpiHelper.AdjustControl(b);
                b.Parent = this._crusierSelectPanel;
            }
        }

        void  button_Click(object sender, EventArgs e)
        {
 	        Button b = (Button)sender;
            CruiserVM cruiser = (CruiserVM)b.Tag;
            _logicController.HandleCruiserSelected(cruiser);
        }
    }
}
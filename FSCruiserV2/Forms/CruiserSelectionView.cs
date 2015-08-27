using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FSCruiserV2.Logic;

namespace FSCruiserV2.Forms
{
    public partial class FormCruiserSelection : UserControl, ICruiserSelectionView
    {
        private FormCruiserSelectionLogic _logicController; 
        public FormCruiserSelection(IApplicationController controller)
        {
            _logicController = new FormCruiserSelectionLogic(controller, this);
            InitializeComponent();
        }

        #region ICruiserSelectionView Members

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

        public void Close()
        {
            this.Enabled = false;
        }

        public void UpdateCruiserList(IList<FSCruiserV2.Logic.CruiserVM> cruisers)
        {
            throw new NotImplementedException();
        }

        public DialogResult ShowDialog(FSCruiserV2.Logic.TreeVM tree)
        {
            _logicController.Tree = tree;
            this.Enabled = true;

            return DialogResult.OK;
        }

        #endregion
    }
}

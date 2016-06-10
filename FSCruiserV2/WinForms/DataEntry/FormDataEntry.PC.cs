using System;
using System.Windows.Forms;
using FSCruiser.Core;
using FSCruiser.Core.Models;
using FSCruiser.Core.ViewInterfaces;
using FSCruiser.WinForms.Common;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class FormDataEntry
    {
        FormDataEntry()
        {
            InitializeComponent();
        }

        public FormDataEntry(IApplicationController controller
            , CuttingUnitVM unit)
        {
            InitializeComponent();
            InitializeCommon(controller, unit);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyData == Keys.F3)
            {
                this.LogicController.HandleAddTreeClick();
                e.Handled = true;
            }
            else
            {
                OnKeyUpInternal(e);
            }
        }

        protected void OnFocusedLayoutChanged(object sender, EventArgs e)
        {
            OnFocusedLayoutChangedInternal(sender, e);

            var treeView = FocusedLayout as ITreeView;
            _addTreeBTN.Enabled = (treeView != null && treeView.UserCanAddTrees);
            _deleteTreeBTN.Enabled = (treeView != null);
        }
    }
}
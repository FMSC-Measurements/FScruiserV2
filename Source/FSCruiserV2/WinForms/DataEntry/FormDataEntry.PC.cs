using System;
using System.Windows.Forms;
using FSCruiser.Core;
using FSCruiser.Core.Models;
using FSCruiser.Core.ViewInterfaces;
using FSCruiser.WinForms.Common;
using FScruiser.Core.Services;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class FormDataEntry
    {
        FormDataEntry()
        {
            InitializeComponent();
        }

        public FormDataEntry(IApplicationController controller
            , ApplicationSettings appSettings
            , IDataEntryDataService dataService)
        {
            InitializeComponent();
            InitializeCommon(controller, appSettings, dataService);

            UpdateAddTreeButton();
        }

        //protected override void OnKeyUp(KeyEventArgs e)
        //{
        //    if (e.KeyData == Keys.F3)
        //    {
        //        this.LogicController.HandleAddTreeClick();
        //        e.Handled = true;
        //    }
        //    else
        //    {
        //        OnKeyUpInternal(e);
        //    }
        //}

        protected void UpdateAddTreeButton()
        {
            var addTreeKey = AppSettings.AddTreeKeyStr;
            if (!string.IsNullOrWhiteSpace(addTreeKey))
            {
                _addTreeBTN.Text = "Add Tree" + "(" + addTreeKey + ")";
            }
            else
            {
                _addTreeBTN.Text = "Add Tree";
            }
        }

        protected void OnFocusedLayoutChanged(object sender, EventArgs e)
        {
            OnFocusedLayoutChangedInternal(sender, e);

            var treeView = FocusedLayout as ITreeView;
            _addTreeBTN.Enabled = (treeView != null && treeView.UserCanAddTrees);
            _deleteTreeBTN.Enabled = (treeView != null);

            var plotView = FocusedLayout as IPlotLayout;
            _limitingDistanceMI.Enabled = (plotView != null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var point = button1.PointToScreen(new System.Drawing.Point(0, button1.Height));
            contextMenuStrip1.Show(point);
        }

        private void _appSettings_HotKeysChanged()
        {
            UpdateAddTreeButton();
        }
    }
}
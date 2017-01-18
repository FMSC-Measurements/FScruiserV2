using System;
using System.Windows.Forms;
using FMSC.Controls;
using FSCruiser.Core;
using FSCruiser.Core.Models;
using FSCruiser.Core.ViewInterfaces;
using FSCruiser.WinForms.Common;
using FSCruiser.NetCF.WinForms;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class FormDataEntry : FMSC.Controls.CustomForm, IDataEntryView
    {
        public FormDataEntry(IApplicationController controller
            , CuttingUnit unit)
        {
            InitializeComponent();

            if (ViewController.PlatformType == PlatformType.WM)
            {
                this.SIP = new Microsoft.WindowsCE.Forms.InputPanel();
                this.components.Add(SIP);
            }
            else if (ViewController.PlatformType == PlatformType.WinCE)
            {
                this.WindowState = FormWindowState.Maximized;
            }

            InitializeCommon(controller, unit);
        }

        protected FormDataEntry()
            : base()
        {
            InitializeComponent();
        }

        protected void OnFocusedLayoutChanged(object sender, EventArgs e)
        {
            Controller.ViewController.SignalPageChanged();
            OnFocusedLayoutChangedInternal(sender, e);
            var view = FocusedLayout as ITreeView;
            _addTreeMI.Enabled = view != null && view.UserCanAddTrees;

        }

        //protected override void OnKeyUp(KeyEventArgs e)
        //{
        //    OnKeyUpInternal(e);
        //}

        private void _addTreeMI_Click(object sender, EventArgs e)
        {
            this.LogicController.HandleAddTreeClick();
        }

        private void _editCruisersMI_Click(object sender, EventArgs e)
        {
            Controller.ViewController.ShowManageCruisers();
        }

        private void _showHideLogColMI_Click(object sender, EventArgs e)
        {
            this.LogicController.HandleShowHideLogCol();
        }

        private void LimitingDistance_Click(object sender, EventArgs e)
        {
            this.LogicController.HandleDisplayLimitingDistance();
        }

        private void menuItem1_Popup(object sender, EventArgs e)
        {
            this._limitingDistanceMI.Enabled = this.FocusedLayout is LayoutPlot;
            this._showHideErrorColMI.Enabled = this.FocusedLayout is ITreeView;
            this._deleteRowButton.Enabled = this.FocusedLayout is ITreeView;
            this._showHideLogColMI.Enabled = this.FocusedLayout is ITreeView;
        }

        private void showHideErrorMessages_Click(object sender, EventArgs e)
        {
            this.LogicController.HandleShowHideErrorCol();
        }

        private void _settingsMI_Click(object sender, EventArgs e)
        {
            using (var view = new FormSettings())
            {
                view.ShowDialog();
            }
        }

    }
}
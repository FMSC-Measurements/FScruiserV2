using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using CruiseDAL.DataObjects;
using FSCruiser.Core;
using FSCruiser.Core.Models;
using FSCruiser.Core.ViewInterfaces;

namespace FSCruiser.WinForms.DataEntry
{
    public class FormPlotInfoBase : Form, IPlotInfoDialog
    {
        protected BindingSource _BS_Plot;
        private IContainer components;
        protected PlotDO _initialState;

        public IApplicationController Controller { get; set; }
        public PlotVM CurrentPlotInfo { get; protected set; }


        protected FormPlotInfoBase()
        {
            this.InitializeComponent();
        }

        protected virtual void OnShowing(PlotVM plotInfo, bool allowEdit)
        {
            if (allowEdit == true)//only if we allow edits do we need to save the initial state
            {
                _initialState = new PlotDO(plotInfo);
            }
            else
            {
                _initialState = null;
            }
        }


        public DialogResult ShowDialog(PlotVM plotInfo, bool allowEdit)
        {
            if (plotInfo == null) { return DialogResult.None; }
            CurrentPlotInfo = plotInfo;

            this.OnShowing(plotInfo, allowEdit);
            this.DialogResult = DialogResult.OK;

            this._BS_Plot.DataSource = plotInfo;
            if (this._BS_Plot.IsBindingSuspended)
            {
                this._BS_Plot.ResumeBinding();
            }
            
            
            return this.ShowDialog();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            this._BS_Plot.EndEdit();
            if (this.DialogResult == DialogResult.Cancel) 
            {
                if (_initialState != null)
                {
                    CurrentPlotInfo.SetValues(_initialState);
                }
                return; 
            }
            

        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPlotInfoBase));
            this._BS_Plot = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this._BS_Plot)).BeginInit();
            this.SuspendLayout();
            // 
            // FormPlotInfoBase
            // 
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormPlotInfoBase";
            ((System.ComponentModel.ISupportInitialize)(this._BS_Plot)).EndInit();
            this.ResumeLayout(false);

        }
    }
}

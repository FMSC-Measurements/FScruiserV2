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
        PlotStratum _stratum; 
        protected PlotDO _initialState;

        public PlotVM Plot { get; protected set; }

        protected FormPlotInfoBase()
        {
            this.InitializeComponent();
        }

        public virtual DialogResult ShowDialog(PlotVM plot, PlotStratum stratum, bool isNewPlot)
        {
            if (plot == null) { return DialogResult.None; }
            _stratum = stratum;
            Plot = plot;
            if (!isNewPlot)
            {
                _initialState = new PlotDO(plot);
            }
            else
            {
                _initialState = null;
            }

            this.DialogResult = DialogResult.OK;

            this._BS_Plot.DataSource = plot;
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

            bool plotNumberChanged = false;            
            if (_initialState != null) 
            { 
                plotNumberChanged = _initialState.PlotNumber != Plot.PlotNumber;
            }

            if (DialogResult == DialogResult.OK)
            {
                if (Plot.PlotNumber <= 0L)
                {
                    MessageBox.Show("Plot Number Must Be Greater Than 0");
                    e.Cancel = true;
                }
                else if (plotNumberChanged
                    && !_stratum.IsPlotNumberAvailable(Plot.PlotNumber))
                {
                    MessageBox.Show("Plot Number Already Exists");
                    e.Cancel = true;
                }
                else if (this.Plot.IsNull && this.Plot.Trees.Count > 0)
                {
                    MessageBox.Show("Null plot can not contain trees");
                    e.Cancel = true;
                }
            }
            else if (this.DialogResult == DialogResult.Cancel) 
            {
                if (_initialState != null)
                {
                    Plot.SetValues(_initialState);
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

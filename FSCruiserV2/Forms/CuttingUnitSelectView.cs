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
    public partial class CuttingUnitSelectView : UserControl
    {
        public IApplicationController Controller { get; set; }
        public CuttingUnitVM SelectedUnit { get; set; }


        public CuttingUnitSelectView()
        {
            InitializeComponent();
        }

        public CuttingUnitSelectView(IApplicationController controller): this()
        {
            this.Controller = controller;
        }

        public void OnCuttingUnitsChanged()
        {
            this._cuttingUnitCB.DataSource = Controller.CuttingUnits;
            this._cuttingUnitCB.Update();
        }

        private void _cuttingUnitCB_SelectedValueChanged(object sender, EventArgs e)
        {
            CuttingUnitVM unit = _cuttingUnitCB.SelectedValue as CuttingUnitVM;
            SelectedUnit = unit;

            if (SelectedUnit != null)
            {
                SelectedUnit.Strata.Populate();
                String[] stDes = new String[SelectedUnit.Strata.Count];

                for (int i = 0; i < SelectedUnit.Strata.Count; i++)
                {
                    stDes[i] = ApplicationController.GetStratumInfoShort(SelectedUnit.Strata[i]);
                }
                this._strataLB.DataSource = stDes;
            }
        }


    }
}

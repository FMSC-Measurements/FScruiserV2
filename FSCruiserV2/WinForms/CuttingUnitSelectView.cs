using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FSCruiser.Core;
using FSCruiser.Core.Models;
using CruiseDAL.DataObjects;

namespace FSCruiser.WinForms
{
    public partial class CuttingUnitSelectView : UserControl
    {
        public IApplicationController Controller { get; set; }
        public CuttingUnitVM SelectedUnit 
        {
            get
            {
                CuttingUnitVM unitVM = _BS_CuttingUnits.Current as CuttingUnitVM;
                if (unitVM != null && unitVM.Code != null)
                {
                    return unitVM;
                }
                return null; 
            }
             
        }


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
            this._BS_CuttingUnits.DataSource = Controller.CuttingUnits;
            //this._cuttingUnitCB.Update();
        }

        private void _cuttingUnitCB_SelectedValueChanged(object sender, EventArgs e)
        {
            var unit = SelectedUnit;
            if (unit != null)
            {
                unit.Strata.Populate();
                String[] stDes = new String[unit.Strata.Count];

                for (int i = 0; i < unit.Strata.Count; i++)
                {
                    StratumDO stratum = unit.Strata[i];
                    stDes[i] = stratum.GetDescriptionShort();
                }
                this._strataLB.DataSource = stDes;
            }
            else
            {
                this._strataLB.DataSource = null;
            }
        }


    }
}

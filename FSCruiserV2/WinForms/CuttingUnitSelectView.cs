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

        //private void _cuttingUnitCB_SelectedValueChanged(object sender, EventArgs e)
        //{
        //    var unit = SelectedUnit;
        //    if (unit != null)
        //    {
        //        var strata = unit.DAL.From<StratumDO>()
        //           .Join("CuttingUnitStratum", "USING (Stratum_CN)", "CUST")
        //           .Where("CUST.CuttingUnit_CN = ?")
        //           .Query(unit.CuttingUnit_CN);

        //        var strataDescriptions = (from StratumDO st in strata
        //                                  select st.GetDescriptionShort()).ToArray()

        //        this._strataLB.DataSource = strataDescriptions;
        //    }
        //    else
        //    {
        //        this._strataLB.DataSource = null;
        //    }
        //}

        private void _BS_CuttingUnits_CurrentChanged(object sender, EventArgs e)
        {
            var unit = SelectedUnit;
            if (unit != null)
            {
                var strata = unit.DAL.From<StratumDO>()
                   .Join("CuttingUnitStratum", "USING (Stratum_CN)", "CUST")
                   .Where("CUST.CuttingUnit_CN = ?")
                   .Query(unit.CuttingUnit_CN);

                var strataDescriptions = (from StratumDO st in strata
                                          select st.GetDescriptionShort()).ToArray();

                this._strataLB.DataSource = strataDescriptions;
            }
            else
            {
                this._strataLB.DataSource = null;
            }
        }


    }
}

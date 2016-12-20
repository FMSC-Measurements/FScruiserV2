using System;
using System.Linq;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using FSCruiser.Core;
using FSCruiser.Core.Models;

namespace FSCruiser.WinForms
{
    public partial class CuttingUnitSelectView : UserControl
    {
        public IApplicationController Controller { get; set; }

        public CuttingUnit SelectedUnit
        {
            get
            {
                var unitVM = _BS_CuttingUnits.Current as CuttingUnit;
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

        public CuttingUnitSelectView(IApplicationController controller)
            : this()
        {
            this.Controller = controller;
        }

        public void OnCuttingUnitsChanged()
        {
            if (this.Controller.CuttingUnits != null)
            {
                var units = new CuttingUnit[Controller.CuttingUnits.Count + 1];
                Controller.CuttingUnits.CopyTo(units, 1);
                units[0] = new CuttingUnit();
                this._BS_CuttingUnits.DataSource = units;
            }
            else
            {
                this._BS_CuttingUnits.DataSource = new CuttingUnit[0];
            }
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
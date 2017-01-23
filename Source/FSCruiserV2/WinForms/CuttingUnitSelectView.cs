using System;
using System.Linq;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using FSCruiser.Core;
using FSCruiser.Core.Models;
using System.Collections.Generic;

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

        public IEnumerable<CuttingUnit> ReadCuttingUnits()
        {
            if (Controller.DataStore != null)
            {
                yield return new CuttingUnit();
                foreach (var unit in Controller.DataStore.From<CuttingUnit>().Read())
                {
                    yield return unit;
                }
            }
            else
            {
                yield break;
            }
        }

        public void HandleFileStateChanged()
        {
            this._BS_CuttingUnits.DataSource = ReadCuttingUnits().ToArray();
        }

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
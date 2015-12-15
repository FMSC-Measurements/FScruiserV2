using System;

using System.Collections.Generic;
using System.Text;
using System.Threading;
using FSCruiser.Core.Models;
using System.Diagnostics;
using System.ComponentModel;

namespace FSCruiser.Core
{
    public class LoadCuttingUnitWorker
    {
        Thread _loadCuttingUnitDataThread;
        CuttingUnitVM _unit;

        public event EventHandler DoneLoading;

        public LoadCuttingUnitWorker(CuttingUnitVM unit)
        {
            Debug.Assert(unit != null);

            _unit = unit;
        }

        public void AsyncLoadCuttingUnitData()
        {
            if (this._loadCuttingUnitDataThread != null)
            {
                this._loadCuttingUnitDataThread.Abort();
            }
            this._loadCuttingUnitDataThread = new Thread(this.LoadData);
            this._loadCuttingUnitDataThread.IsBackground = true;
            this._loadCuttingUnitDataThread.Priority = Constants.LOAD_CUTTINGUNITDATA_PRIORITY;
            this._loadCuttingUnitDataThread.Start();
        }

        protected void OnDoneLoading()
        {
            if (this.DoneLoading != null)
            {
                this.DoneLoading(this, null);
            }
        }

        public void LoadData()
        {

            InitializeSampleGroups();
            //InitializeUnitTreeNumIndex();
            _unit.TallyHistoryBuffer = new TallyHistoryCollection(_unit, Constants.MAX_TALLY_HISTORY_SIZE);
            _unit.TallyHistoryBuffer.Initialize();


            InitializeNonPlotTrees();
            //create a list of just trees in tree based strata
            List<TreeVM> nonPlotTrees = _unit.DAL.Read<TreeVM>(@"JOIN Stratum ON Tree.Stratum_CN = Stratum.Stratum_CN WHERE Tree.CuttingUnit_CN = ? AND
                        (Stratum.Method = '100' OR Stratum.Method = 'STR' OR Stratum.Method = '3P' OR Stratum.Method = 'S3P') ORDER BY TreeNumber",
                        (object)_unit.CuttingUnit_CN);
            _unit.NonPlotTrees = new BindingList<TreeVM>(nonPlotTrees);

            if (_unit.DAL.GetRowCount("CuttingUnitStratum", "WHERE CuttingUnit_CN = ?", _unit.CuttingUnit_CN) == 1)
            {
                _unit.DefaultStratum = _unit.DAL.ReadSingleRow<StratumVM>("JOIN CuttingUnitStratum USING (Stratum_CN) WHERE CuttingUnit_CN = ?",
                        (object)_unit.CuttingUnit_CN);
            }
            else
            {
                _unit.DefaultStratum = _unit.DAL.ReadSingleRow<StratumVM>(
                        "JOIN CuttingUnitStratum USING (Stratum_CN) WHERE CuttingUnit_CN = ? AND Method = ?",
                        (object)CruiseDAL.Schema.Constants.CruiseMethods.H_PCT,
                        (object)_unit.CuttingUnit_CN);
            }

            this.OnDoneLoading();
            //this.ViewController.HandleCuttingUnitDataLoaded();

        }

        public void InitializeSampleGroups()
        {
            //create a list of all samplegroups in the unit
            _unit.SampleGroups = _unit.DAL.Read<SampleGroupVM>(@"JOIN Stratum ON SampleGroup.Stratum_CN = Stratum.Stratum_CN 
                JOIN CuttingUnitStratum ON CuttingUnitStratum.Stratum_CN = Stratum.Stratum_CN
                WHERE CuttingUnitStratum.CuttingUnit_CN = ?", (object)_unit.CuttingUnit_CN);

            //initialize sample selectors for all sampleGroups
            foreach (SampleGroupVM sg in _unit.SampleGroups)
            {
                //DataEntryMode mode = GetStrataDataEntryMode(sg.Stratum);
                sg.Sampler = sg.MakeSampleSelecter();
            }
        }

        public void InitializeNonPlotTrees()
        {
            //create a list of just trees in tree based strata
            List<TreeVM> nonPlotTrees = _unit.DAL.Read<TreeVM>(@"JOIN Stratum ON Tree.Stratum_CN = Stratum.Stratum_CN WHERE Tree.CuttingUnit_CN = ? AND
                        (Stratum.Method = '100' OR Stratum.Method = 'STR' OR Stratum.Method = '3P' OR Stratum.Method = 'S3P') ORDER BY TreeNumber",
                        (object)_unit.CuttingUnit_CN);
            _unit.NonPlotTrees = new BindingList<TreeVM>(nonPlotTrees);
            _unit.ValidateTreesAsync();
        }
    }
}

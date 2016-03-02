using System;
using System.Linq;
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

            //InitializeCounts();
            //InitializeUnitTreeNumIndex();
            _unit.TallyHistoryBuffer = new TallyHistoryCollection(_unit, Constants.MAX_TALLY_HISTORY_SIZE);
            _unit.TallyHistoryBuffer.Initialize();


            InitializeNonPlotTrees();

            this.OnDoneLoading();
        }

        //public void InitializeCounts()
        //{
        //    _unit.Counts = _unit.DAL.Read<CountTreeVM>((string)null);
        //}

        public void InitializeSampleGroups()
        {
            //create a list of all samplegroups in the unit
            _unit.SampleGroups = _unit.DAL.From<SampleGroupVM>()
                .Join("Stratum", "USING (Stratum_CM)")
                .Join("CuttingUnitStratum", "USING (Stratum_CN)")
                .Where("CuttingUnitStratum.CuttingUnit_CN = ?")
                .Read(_unit.CuttingUnit_CN).ToList();

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
            List<TreeVM> nonPlotTrees = _unit.DAL.From<TreeVM>()
                .Join("Stratum", "USING (Stratum_CN)")
                .Where("Tree.CuttingUnit_CN = ? AND " +
                        "Stratum.Method IN ('100','STR','3P','S3P')")
                .OrderBy("TreeNumber")
                .Read(_unit.CuttingUnit_CN).ToList();
            
            _unit.NonPlotTrees = new BindingList<TreeVM>(nonPlotTrees);
            _unit.ValidateTreesAsync();
        }
    }
}

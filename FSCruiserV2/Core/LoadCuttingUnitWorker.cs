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
            InitializeStrata();
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

        public void InitializeStrata()
        {
            _unit.TreeStrata = _unit.GetTreeBasedStrata();
            _unit.PlotStrata = _unit.GetPlotStrata();

            _unit.DefaultStratum = null;
            foreach (StratumVM stratum in _unit.TreeStrata)
            {
                if (stratum.Method == CruiseDAL.Schema.Constants.CruiseMethods.H_PCT)
                {
                    _unit.DefaultStratum = stratum;
                    break;
                }
            }

            if (_unit.DefaultStratum == null && _unit.TreeStrata.Count > 0)
            {
                _unit.DefaultStratum = _unit.TreeStrata[0];
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

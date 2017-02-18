using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using FSCruiser.Core.Models;

namespace FSCruiser.Core
{
    public class LoadCuttingUnitWorker
    {
        Thread _loadCuttingUnitDataThread;
        CuttingUnit _unit;

        public event EventHandler DoneLoading;

        public LoadCuttingUnitWorker(CuttingUnit unit)
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
            this._loadCuttingUnitDataThread.Priority = System.Threading.ThreadPriority.BelowNormal;
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
            InitializeNonPlotTrees();
            InitializeTallyBuffer();

            this.OnDoneLoading();
        }

        public void InitializeTallyBuffer()
        {
            //_unit.TallyHistoryBuffer = new TallyHistoryCollection(_unit, Constants.MAX_TALLY_HISTORY_SIZE);
            //_unit.TallyHistoryBuffer.Initialize();
        }

        public void InitializeNonPlotTrees()
        {
            ////create a list of just trees in tree based strata
            //List<Tree> nonPlotTrees = _unit.DAL.From<Tree>()
            //    .Join("Stratum", "USING (Stratum_CN)")
            //    .Where("Tree.CuttingUnit_CN = ? AND " +
            //            "Stratum.Method IN ('100','STR','3P','S3P')")
            //    .OrderBy("TreeNumber")
            //    .Read(_unit.CuttingUnit_CN).ToList();

            //_unit.NonPlotTrees = new BindingList<Tree>(nonPlotTrees);
            //_unit.ValidateTreesAsync();
        }
    }
}
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using FSCruiserV2.Logic;
using System.ComponentModel;

namespace FSCruiserV2.Forms
{
    public interface IPlotView : ITreeView, ITallyView 
    {
        PlotVM CurrentPlot { get; set; }
        void UpdateCurrentPlot();
        void ShowNoPlotSelectedMessage();
        void ShowNullPlotMessage();

    }


    public class LayoutPlotController
    {
        IPlotView view;

        private bool _disableCheckPlot = false;
        private DataEntryMode _mode;
        private bool _userCanAddTrees;
        private PlotVM _prevPlot;


        public IApplicationController Controller { get; set; }
        public FormDataEntryLogic DataEntryController { get; protected set; }
        public StratumVM StratumInfo { get; set; }

        public Dictionary<char, CountTreeVM> HotKeyLookup
        {
            get
            {
                if (StratumInfo != null)
                {
                    return StratumInfo.HotKeyLookup;
                }
                return null;
            }

        }

        public bool UserCanAddTrees
        {
            get { return _userCanAddTrees; }
            set { _userCanAddTrees = value; }
        }

        public LayoutPlotController(StratumVM st)
        {
            this._mode = Controller.GetStrataDataEntryMode(st);
        }

        public bool CheckPlot(PlotVM pInfo)
        {
            bool goOn = true;
            if (pInfo != null)
            {
                this.view.EndEdit();
                if (!Controller.ValidateTrees(pInfo.Trees))
                {                    
                    if (!this.Controller.ViewController.AskYesNo("Error(s) found on tree records in current plot, Would you like to continue?", "Continue?", MessageBoxIcon.Question))
                    {
                        goOn = false;
                    }
                }
                Controller.SaveTrees(pInfo.Trees);
            }
            return goOn;
        }

        public bool CheckCurrentPlot()
        {
            return this.CheckPlot(this.view.CurrentPlot);
        }


        public bool HandleHotKey(char key)
        {
            //if (_viewLoading) { return false; }
            return Controller.ProcessHotKey(key, this.view);
        }


        public void HandleTreeNumberChanging(long newTreeNumber, out bool cancel)
        {
            try
            {
                if (!this.Controller.EnsureTreeNumberAvalible(newTreeNumber, this.view.CurrentPlot))
                {
                    cancel = true;
                    return;
                }
            }
            catch
            {
                cancel = true;
                return;
            }
            cancel = false;
        }

        private TreeVM GetNewTree()
        {
            if (this.view.CurrentPlot == null)// if no plot is selected cancel action
            {
                this.view.ShowNoPlotSelectedMessage();
                return null;
            }
            if (this.view.CurrentPlot.IsNull)
            {
                this.view.ShowNullPlotMessage();
                return null;
            }

            if (this.UserCanAddTrees == false) { return null; }

            //adding trees can be allowed in some cases for 3PPNT
            //if (this.StratumInfo.Stratum.Method == "3PPNT")// if this is a 3PPNT stratum users aren't allowed to manualy enter trees
            //{
            //    return null;
            //}

            TreeVM prevTree = null;
            if (this.view.Trees.Count > 0)
            {
                prevTree = (TreeVM)this.view.Trees[this.view.Trees.Count - 1];
            }

            return Controller.UserAddTree(prevTree, this.StratumInfo, this.view.CurrentPlot);
        }

        public void OnCurrentPlotChanged()
        {
            if (!_disableCheckPlot && _prevPlot != null && _prevPlot != this.view.CurrentPlot)
            {
                if (!this.CheckPlot(this._prevPlot))
                {
                    this.view.CurrentPlot = _prevPlot;
                }
            }
            this.view.UpdateCurrentPlot();
            _prevPlot = this.view.CurrentPlot;
        }

        public void Save()
        {
            foreach (PlotVM p in this.StratumInfo.Plots)
            {
                p.Save();
            }
        }

        public TreeVM UserAddTree()
        {
            
            TreeVM t = this.GetNewTree();
            if (t != null)
            {
                this.view.Trees.Add(t);
                this.view.MoveLast();
            }
            return t;
        }


    }
}

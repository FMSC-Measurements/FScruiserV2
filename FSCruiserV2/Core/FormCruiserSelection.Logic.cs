using System;
using FSCruiser.Core.Models;
using FSCruiser.Core.ViewInterfaces;

namespace FSCruiser.Core
{
    public class FormCruiserSelectionLogic
    {
        private CruiserVM[] _cruisers;
        private IApplicationController _controller;

        public ICruiserSelectionView View { get; protected set; }

        public TreeVM Tree { get; set; }

        public FormCruiserSelectionLogic(IApplicationController controller, ICruiserSelectionView view)
        {
            this._controller = controller;
            this.View = view;
        }

        public void HandleLoad()
        {
            if (Tree == null) { throw new InvalidOperationException("Set Tree before calling HandleLoad"); }

            this.View.TreeNumberText = "Tree #:" + Tree.TreeNumber;
            this.View.StratumText = "Stratum: " + Tree.Stratum.GetDescriptionShort();
            this.View.SampleGroupText = "Sg: " + Tree.SampleGroup.GetDescriptionShort();

            _cruisers = _controller.Settings.Cruisers.ToArray();
            this.View.UpdateCruiserList(_cruisers);
        }

        public void HandleKeyDown(char key)
        {
            if (char.IsNumber(key))
            {
                int value = Convert.ToInt32(key.ToString());
                if (value <= this._cruisers.Length && value > 0)
                {
                    this.HandleCruiserSelected(this._cruisers[value - 1]);
                }
            }
        }

        public void HandleCruiserSelected(CruiserVM cruiser)
        {
            this.Tree.Initials = cruiser.Initials;
            //this.Tree.Save();//don't save here, let it be saved by FormDataEntryLogic

            this.View.Close();
        }
    }
}
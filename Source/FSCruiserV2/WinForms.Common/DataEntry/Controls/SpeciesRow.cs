using System;

using System.Windows.Forms;
using FSCruiser.Core.Models;

namespace FSCruiser.WinForms
{
    public class SpeciesRow : Button
    {
        SubPop _subPop;

        public SubPop SubPopulation
        {
            get { return _subPop; }
            set
            {
                _subPop = value;
                this.Text = (_subPop != null && _subPop.TDV != null)
                    ? _subPop.TDV.Species
                    : String.Empty;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FSCruiser.Core.Models;


namespace FSCruiser.Core.ViewInterfaces
{
    public interface ICruiserSelectionView 
    {
        String TreeNumberText { set; }
        String StratumText { set; }
        String SampleGroupText { set; }

        void Close();
        void UpdateCruiserList(IList<CruiserVM> cruisers);
        DialogResult ShowDialog(TreeVM tree);
    }
}

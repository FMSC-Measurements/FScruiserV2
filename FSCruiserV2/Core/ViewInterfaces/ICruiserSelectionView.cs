using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FSCruiserV2.Logic;

namespace FSCruiserV2.Forms
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

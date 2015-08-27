using System.Collections.Generic;
using FSCruiserV2.Logic;

namespace FSCruiserV2.Forms
{
    public interface ITreeView: FSCruiserV2.Forms.IDataEntryPage
    {
        bool UserCanAddTrees { get; set; }
        //String[] VisableFields { get; }
        IList<TreeVM> Trees { get; }

        
        void ShowHideErrorCol();
        
        void HandleEnableLogGradingChanged();
        void HandleCruisersChanged();
        void DeleteRow();
        void EndEdit();
        void MoveLast();
        void MoveHomeField();
        TreeVM UserAddTree();

        void UpdateSpeciesColumn(TreeVM tree);
        void UpdateSampleGroupColumn(TreeVM tree);

        

    }
}

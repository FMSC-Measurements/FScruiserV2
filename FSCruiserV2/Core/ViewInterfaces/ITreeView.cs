using System.Collections.Generic;
using FSCruiser.Core.Models;


namespace FSCruiser.Core.ViewInterfaces
{
    public interface ITreeView: IDataEntryPage
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

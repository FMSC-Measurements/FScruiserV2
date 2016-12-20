using System.Collections.Generic;
using FSCruiser.Core.Models;

namespace FSCruiser.Core.ViewInterfaces
{
    public interface ITreeView : IDataEntryPage
    {
        bool UserCanAddTrees { get; set; }

        //String[] VisableFields { get; }
        IList<Tree> Trees { get; }

        bool ErrorColumnVisable { get; set; }

        bool LogColumnVisable { get; set; }

        void HandleEnableLogGradingChanged();

        void HandleCruisersChanged();

        void DeleteSelectedTree();

        void EndEdit();

        void MoveLastTree();

        void MoveHomeField();

        Tree UserAddTree();

        //void UpdateSpeciesColumn(TreeVM tree);
        //void UpdateSampleGroupColumn(TreeVM tree);
    }

    public static class ITreeViewExtentions
    {
        public static void ToggleErrorColumn(this ITreeView view)
        {
            if (view == null) { return; }
            view.ErrorColumnVisable = !view.ErrorColumnVisable;
        }

        public static void ToggleLogColumn(this ITreeView view)
        {
            if (view == null) { return; }
            view.LogColumnVisable = !view.LogColumnVisable;
        }
    }
}
using System.Windows.Forms;

namespace FSCruiser.Core.ViewInterfaces
{
    public interface IDataEntryPage 
    {
        bool ViewLoading { get; }

        void HandleLoad();

        bool PreviewKeypress(KeyEventArgs ea);

        void NotifyEnter();
    }
}
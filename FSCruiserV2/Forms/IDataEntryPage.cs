
using FSCruiserV2.Logic;
namespace FSCruiserV2.Forms
{
    public interface IDataEntryPage
    {
        bool ViewLoading { get; }
        void HandleLoad();
        bool HandleEscKey();
    }
}


namespace FSCruiser.Core.ViewInterfaces
{
    public interface IDataEntryPage
    {
        bool ViewLoading { get; }
        void HandleLoad();
        bool HandleEscKey();
    }
}

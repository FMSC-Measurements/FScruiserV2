namespace FSCruiser.Core.ViewInterfaces
{
    public interface IDataEntryPage
    {
        string Text { get; set; }

        bool ViewLoading { get; }

        void HandleLoad();

        bool PreviewKeypress(string keyStr);

        void NotifyEnter();
    }
}
namespace MainUI.Presentation
{
    public interface ITab
    {
        TabType TabType { get; }
        void Activate();
        void Deactivate();
    }
}
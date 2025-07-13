namespace PhotoSequenceVideo;

public partial class AppShell : Shell
{
    public AppShell()
    {
        SetNavBarIsVisible(this, false);

        Items.Add(
            new TabBar
            {
                Title = "Photo Sequence Video",
                Items =
                {
                    new ShellContent
                    {
                        ContentTemplate = new DataTemplate(typeof(MainPage)),
                        Route = nameof(MainPage),
                    },
                },
            }
        );
    }
}

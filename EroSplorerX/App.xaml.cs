using Microsoft.UI.Xaml;

namespace EroSplorerX;

public partial class App : Application
{ 
    public static MainWindow MainWindowInstance { get; private set; }

    public App()
    {
        this.InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        MainWindowInstance = new MainWindow();
        MainWindowInstance.Activate();
        MainWindowInstance.ExtendsContentIntoTitleBar = true;
    }
}

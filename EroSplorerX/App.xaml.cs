using Microsoft.UI.Xaml;
using Serilog;

namespace EroSplorerX;

public partial class App : Application
{ 
    public static MainWindow? MainWindowInstance { get; private set; }

    public App()
    {
        this.InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File("Data/logs/esx-.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        MainWindowInstance = new MainWindow();
        MainWindowInstance.Activate();
        MainWindowInstance.ExtendsContentIntoTitleBar = true;
    }
}

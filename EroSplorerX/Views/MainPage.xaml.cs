using EroSplorerX.Data;
using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;

namespace EroSplorerX.Views;

public sealed partial class MainPage : UserControl
{
    public MainPage()
    {
        this.InitializeComponent();

        this.DataContext = new MainPageViewModel();

        CollectionListView.MainPageRef = this;
        CollectionListView.MainListRef = ItemPanelView;
    }

    #region InfoBar Methods

    public void ShowInfoInfoBar(string message, int time = SystemConstants.INFOBAR_DURATION_DEFAULT)
    {
        MainInfoBar.Title = "Info";
        MainInfoBar.Severity = InfoBarSeverity.Informational;
        ShowInfoBarTemporarily(message, time);
    }

    public void ShowSuccessInfoBar(string message, int time = SystemConstants.INFOBAR_DURATION_DEFAULT)
    {
        MainInfoBar.Title = "Success";
        MainInfoBar.Severity = InfoBarSeverity.Success;
        ShowInfoBarTemporarily(message, time);
    }

    public void ShowWarningInfoBar(string message, int time = SystemConstants.INFOBAR_DURATION_DEFAULT)
    {
        MainInfoBar.Title = "Warning";
        MainInfoBar.Severity = InfoBarSeverity.Warning;
        ShowInfoBarTemporarily(message, time);
    }

    public void ShowErrorInfoBar(string message, int time = SystemConstants.INFOBAR_DURATION_DEFAULT)
    {
        MainInfoBar.Title = "Error";
        MainInfoBar.Severity = InfoBarSeverity.Error;
        ShowInfoBarTemporarily(message, time);
    }

    private async void ShowInfoBarTemporarily(string message, int time = SystemConstants.INFOBAR_DURATION_DEFAULT)
    {
        MainInfoBar.Message = message;
        MainInfoBar.IsOpen = true;

        await Task.Delay(time);
        FadeOutInfoBar.Begin();
    }

    private void MainInfoBar_CloseButtonClick(InfoBar sender, object args)
    {
        MainInfoBar.IsOpen = false;
        FadeOutInfoBar.Stop();
    }

    private void FadeOutInfoBar_Completed(object sender, object e)
    {
        MainInfoBar.IsOpen = false;
        MainInfoBar.Opacity = 1;
    }

    #endregion

}

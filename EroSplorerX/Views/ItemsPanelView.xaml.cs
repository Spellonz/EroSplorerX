using EroSplorerX.Data;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using WinUIEx;

namespace EroSplorerX.Views;

public sealed partial class ItemsPanelView : UserControl
{
    private ObservableCollection<EroPath> items { get; set; } = [];
    public ObservableCollection<EroPath> Items
    {
        get => items;
        set => items = value;
    }

    public ItemsPanelView()
    {
        this.InitializeComponent();
    }

    public void SetItems(List<EroPath> eroPaths)
    {
        Items.Clear();
        foreach (var eroPath in eroPaths)
            Items.Add(eroPath);
    }

    /// <summary>
    /// Configures the drag and drop operation for the EroPath item.
    /// </summary>
    private async void Item_DragStarting(UIElement sender, DragStartingEventArgs args)
    {
        var image = sender as Image;

        if (image != null)
        {
            var item = image?.DataContext as EroPath;

            if (item != null)
            {
                var dataPackage = new DataPackage();
                var storageFile = await StorageFile.GetFileFromPathAsync(item.FunScriptPath);
                args.Data.SetStorageItems(new List<StorageFile> { storageFile });
                args.AllowedOperations = DataPackageOperation.Copy;
            }
        }
    }

    private void ShowEroPathInExplorer_Click(object sender, RoutedEventArgs e)
    {
        var pathData = (sender as MenuFlyoutItem);
        if (pathData == null) return;

        var path = pathData.Tag.ToString();
        if (string.IsNullOrEmpty(path)) return;

        Process.Start("explorer.exe", path);
    }

    private void Play_Click(object sender, RoutedEventArgs e)
    {
        var pathData = (sender as MenuFlyoutItem);
        if (pathData == null) return;

        var path = pathData.Tag.ToString();
        if (string.IsNullOrEmpty(path)) return;

        var newPlayer = new VideoPlayerWindow();
        newPlayer.SetVideoPath(path);
        newPlayer.Show();
    }
}

using EroSplorerX.Data;
using EroSplorerX.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Windows.Storage.Pickers;
using Windows.Storage;
using EroSplorerX.Data.DTO;

namespace EroSplorerX.Views;

public sealed partial class CollectionsListView : UserControl
{
    public static readonly DependencyProperty MainPageRefProperty = DependencyProperty.Register("MainPageRef", typeof(MainPage), typeof(CollectionsListView), new PropertyMetadata(default(MainPage)));
    public MainPage MainPageRef
    {
        get => (MainPage)GetValue(MainPageRefProperty);
        set => SetValue(MainPageRefProperty, value);
    }

    public static readonly DependencyProperty MainListRefProperty = DependencyProperty.Register("MainListRef", typeof(ItemsPanelView), typeof(CollectionsListView), new PropertyMetadata(default(ItemsPanelView)));
    public ItemsPanelView MainListRef
    {
        get => (ItemsPanelView)GetValue(MainListRefProperty);
        set => SetValue(MainListRefProperty, value);
    }


    private ObservableCollection<EsxCollection> collections { get; set; } = [];
    public ObservableCollection<EsxCollection> Collections
    {
        get => collections;
        set => collections = value;
    }

    public CollectionsListView()
    {
        this.InitializeComponent();

        ShowCollections();
    }

    private void ShowCollections()
    {
        Collections.Clear();

        // Add collections to the tree view
        foreach (var collection in DatabaseHelper.GetCollections().OrderBy(m => m.Name))
        {
            var tag = collection.Name;
            collection.Tag = tag;
            Collections.Add(collection);

            // Add child collections
            // If the tag includes a '/', the converter will indent it.
            if (collection.ShowChildren)
            {
                var children = DatabaseHelper.GetCollectionChildren(collection).OrderBy(m => m.Name);
                foreach (var child in children)
                    Collections.Add(child);
            }
        }
    }

    /// <summary>
    /// When a collection is double clicked, we'll populate the EroPath items list with thumbnails
    /// </summary>
    /// <param name="sender">The collection that was double clicked</param>
    private void Collection_DoubleTapped(object sender, Microsoft.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
    {
        if ((sender as FrameworkElement)?.DataContext is EsxCollection item)
        {
            var newPath = new EroPath(item.Path);

            var scripts = newPath.GetFunscripts();
            MainPageRef.ShowSuccessInfoBar($"Loaded {scripts.Count} scripts from {item.Name}.", SystemConstants.INFOBAR_DURATION_VERY_SHORT);

            MainListRef.SetItems(scripts);
        }
    }

    /// <summary>
    /// Add a new collection to the tree view and save it to the settings
    /// </summary>
    private async void AddCollection_Click(object sender, RoutedEventArgs e)
    {
        FolderPicker openPicker = new FolderPicker();
        var window = App.MainWindowInstance;
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
        WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);
        openPicker.SuggestedStartLocation = PickerLocationId.Desktop;
        openPicker.FileTypeFilter.Add("*");

        StorageFolder folder = await openPicker.PickSingleFolderAsync();

        if (folder == null)
            return;

        var newCollection = new EsxCollection
        {
            Name = folder.DisplayName,
            Path = folder.Path
        };
        DatabaseHelper.AddCollection(newCollection);

        ShowCollections();
    }

    /// <summary>
    /// Initiate a dialog to confirm the deletion of a collection
    /// </summary>
    /// <param name="sender">The MenuFlyoutItem that was clicked</param>
    private async void DeleteCollection_Clicked(object sender, RoutedEventArgs e)
    {
        var collectionData = (sender as MenuFlyoutItem);
        if (collectionData == null)
            return;

        var tag = collectionData.Tag.ToString();

        // Cannot delete child collections directly
        if (tag.Contains('/'))
        {
            MainPageRef.ShowErrorInfoBar($"You cannot delete a child collection directly. ({tag})");
            return;
        }

        var dialog = new ContentDialog
        {
            Title = "Delete Collection?",
            Content = new TextBlock { Text = "Are you sure you want to delete this collection?" },
            PrimaryButtonText = "OK",
            CloseButtonText = "Cancel"
        };

        dialog.XamlRoot = this.XamlRoot;
        var result = await dialog.ShowAsync();

        if (result == ContentDialogResult.Primary)
        {
            var item = Collections.FirstOrDefault(x => x.Tag == tag);
            DatabaseHelper.RemoveCollection(item.Id);

            Collections.Remove(item);
            DatabaseHelper.RemoveCollection(item.Id);

            MainPageRef.ShowSuccessInfoBar("Collection deleted successfully.");
        }
    }

    /// <summary>
    /// Opens the collection path in windows explorer
    /// </summary>
    /// <param name="sender">The MenuFlyoutItem that was clicked</param>
    private void ShowCollectionInExplorer_Click(object sender, RoutedEventArgs e)
    {
        var collectionData = (sender as MenuFlyoutItem);
        var path = collectionData.Tag.ToString();
        Process.Start("explorer.exe", path);
    }

    private void ToggleShowChildren_Click(object sender, RoutedEventArgs e)
    {
        var collectionData = (sender as MenuFlyoutItem);
        var collection = Collections.FirstOrDefault(x => x.Path == collectionData.Tag.ToString());
        
        if (collection.Tag.Contains('/'))
        {
            MainPageRef.ShowWarningInfoBar("You cannot show children of a child collection.");
            return;
        }
        collection.ShowChildren = ((ToggleMenuFlyoutItem)sender).IsChecked;
        DatabaseHelper.UpdateCollection(collection);

        ShowCollections();
    }
}

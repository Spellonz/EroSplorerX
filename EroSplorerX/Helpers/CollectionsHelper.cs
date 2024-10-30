using EroSplorerX.Data;
using EroSplorerX.Data.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EroSplorerX.Helpers;

internal static class CollectionsHelper
{
    private static CollectionStorageData _collectionStorage;
    public static List<CollectionData> GetCollections()
    {
        if (_collectionStorage == null)
            _collectionStorage = LoadCollections();

        return _collectionStorage.Collections;
    }

    private static CollectionStorageData LoadCollections()
    {
        return CollectionStorageData.LoadFromFile(SystemConstants.COLLECTIONS_PATH) ?? new();
    }

    public static void AddCollection(CollectionData collection)
    {
        _collectionStorage.Collections.Add(collection);
        SaveCollections();
    }

    public static void RemoveCollection(CollectionData collection)
    {
        _collectionStorage.Collections.Remove(collection);
        SaveCollections();
    }

    public static void SaveCollections()
    {
        _collectionStorage.SaveToFile(SystemConstants.COLLECTIONS_PATH);
    }

    public static List<CollectionData> GetChildren(CollectionData collection)
    {
        var children = Directory.GetDirectories(collection.Path).Select(m => new CollectionData()
        {
            Name = Path.GetFileName(m),
            Path = m,
            Tag = $"{collection.Tag}/{Path.GetFileName(m)}",
        }).ToList();

        return children;
    }

    public static void UpdateCollection(CollectionData collection)
    {
        var index = _collectionStorage.Collections.FindIndex(m => m.Path == collection.Path);
        if (index == -1)
            return;

        _collectionStorage.Collections[index] = collection;
        SaveCollections();
    }
}
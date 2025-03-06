using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace EroSplorerX.Data.Collections;

public class CollectionStorageData
{
    public List<CollectionData> Collections { get; set; } = [];

    public string Serialize()
    {
        return JsonConvert.SerializeObject(this);
    }

    public static CollectionStorageData Deserialize(string json)
    {
        return JsonConvert.DeserializeObject<CollectionStorageData>(json);
    }

    public static CollectionStorageData LoadFromFile(string path)
    {
        if (!File.Exists(SystemConstants.COLLECTIONS_PATH))
            return new();

        var json = File.ReadAllText(SystemConstants.COLLECTIONS_PATH);
        return Deserialize(json);
    }

    public void SaveToFile(string path)
    {
        var json = this.Serialize();
        File.WriteAllText(path, json);
    }
}

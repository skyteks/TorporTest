using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager
{
    public static string directory = "/";
    public static string fileName = "saveData.json";

    public static void Save(SaveData data)
    {
        if (data == null)
        {
            return;
        }

        string path = string.Concat(Application.persistentDataPath, directory);

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        path = string.Concat(path, fileName);

        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(path, json);

        Debug.Log(string.Concat("Saved data to: ", path));
    }

    public static SaveData Load()
    {
        string path = string.Concat(Application.persistentDataPath, directory);

        if (!Directory.Exists(path))
        {
            return null;
        }
        path = string.Concat(path, fileName);
        if (!File.Exists(path))
        {
            return null;
        }

        string json = File.ReadAllText(path);

        SaveData data = JsonHelper.FromJson<SaveData>(json)[0];
        return data;
    }
}

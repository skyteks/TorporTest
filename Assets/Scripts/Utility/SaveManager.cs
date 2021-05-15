using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager
{
    private static string directory = "/";
    private static string fileName = "saveData.json";

    public static string saveDirecty { get { return string.Concat(Application.persistentDataPath, directory); } }

    public static void Save(SaveData data)
    {
        if (data == null)
        {
            return;
        }

        string path = saveDirecty;

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
        string path = saveDirecty;

        if (!Directory.Exists(path))
        {
            Debug.LogWarning("Loading data failed, directory does not exist");
            return null;
        }
        path = string.Concat(path, fileName);
        if (!File.Exists(path))
        {
            Debug.LogWarning("Loading data failed, file does not exist ");
            return null;
        }

        string json = File.ReadAllText(path);

        SaveData data;
        try
        {
            data = JsonUtility.FromJson<SaveData>(json);
        }
        catch (System.ArgumentException)
        {
            Debug.LogWarning(string.Concat("Loading data failed, from: ", path));
            data = null;
        }

        if (data != null)
        {
            Debug.Log(string.Concat("Loaded data from: ", path));
        }

        return data;
    }
}

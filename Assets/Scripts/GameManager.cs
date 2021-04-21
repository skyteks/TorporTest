using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SaveData saveData;

    void Awake()
    {
        SetDefaultCodex();
    }

    [ContextMenu("Set Default Codex")]
    public void SetDefaultCodex()
    {
        SaveData.Codex codex = CreateDefaultCodex();
        saveData = new SaveData(codex);

        SetDirty();
    }

    [ContextMenu("Save Data To File")]
    public void SaveDataToFile()
    {
        SaveManager.Save(saveData);
    }

    [ContextMenu("Load Data From File")]
    public void LoadDataFromFile()
    {
        saveData = SaveManager.Load();
        SetDirty();
    }

    private SaveData.Codex CreateDefaultCodex()
    {
        string content1A1Text = string.Concat(
            "Greater Holsord is the largest region of \n",
            "Sordland, bordering the regions of Nargis.\n",
            "Geisland, Bergia and Lorren, as well as the\n",
            "country of Lespia. The region is governed from\n",
            "the capital of Sordland, Holsord. It is the most\n",
            "populous region of Sordlands's seven regions\n");
        SaveData.Codex.Topic.Entry.Content content1A1 = new SaveData.Codex.Topic.Entry.Content("Petr Vectern", null, content1A1Text);

        SaveData.Codex.Topic.Entry.Content content1A2 = new SaveData.Codex.Topic.Entry.Content("Petr Vectern", null, "");
        SaveData.Codex.Topic.Entry.Content content1A3 = new SaveData.Codex.Topic.Entry.Content("Lucian Galade", null, "");
        SaveData.Codex.Topic.Entry.Content content1A4 = new SaveData.Codex.Topic.Entry.Content("Lileas Graf", null, "");
        SaveData.Codex.Topic.Entry.Content content1A5 = new SaveData.Codex.Topic.Entry.Content("Symon Holl", null, "");
        SaveData.Codex.Topic.Entry.Content content1A6 = new SaveData.Codex.Topic.Entry.Content("Iosef Lancea", null, "");
        SaveData.Codex.Topic.Entry.Content content1A7 = new SaveData.Codex.Topic.Entry.Content("Deivid Wisci", null, "");
        SaveData.Codex.Topic.Entry.Content content1A8 = new SaveData.Codex.Topic.Entry.Content("Nia Morgna", null, "");
        SaveData.Codex.Topic.Entry.Content content1A9 = new SaveData.Codex.Topic.Entry.Content("Gus Manger", null, "");
        SaveData.Codex.Topic.Entry.Content content1A10 = new SaveData.Codex.Topic.Entry.Content("Ciara Walda", null, "");
        SaveData.Codex.Topic.Entry.Content content1A11 = new SaveData.Codex.Topic.Entry.Content("Paskal Beniwoll", null, "");


        SaveData.Codex.Topic.Entry entry1A = new SaveData.Codex.Topic.Entry(
            "Cabinet", content1A1, content1A2, content1A3, content1A4, content1A5, content1A6, content1A7, content1A8, content1A9, content1A10, content1A11);

        SaveData.Codex.Topic.Entry entry1B = new SaveData.Codex.Topic.Entry("Family");
        SaveData.Codex.Topic.Entry entry1C = new SaveData.Codex.Topic.Entry("Party Leaders");
        SaveData.Codex.Topic.Entry entry1D = new SaveData.Codex.Topic.Entry("The Assembly");
        SaveData.Codex.Topic.Entry entry1E = new SaveData.Codex.Topic.Entry("The Supreme Court");
        SaveData.Codex.Topic topic1 = new SaveData.Codex.Topic("Characters", entry1A, entry1B, entry1C, entry1D, entry1E);

        SaveData.Codex.Topic topic2 = new SaveData.Codex.Topic("Locations");

        SaveData.Codex.Topic topic3 = new SaveData.Codex.Topic("Organisations");

        SaveData.Codex.Topic topic4 = new SaveData.Codex.Topic("History");

        SaveData.Codex codex = new SaveData.Codex(topic1, topic2, topic3, topic4);
        return codex;
    }

    private void SetDirty()
    {
#if UNITY_EDITOR
        if (Application.isEditor && !Application.isPlaying)
        {
        UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIManager))]
public class GameManager : Singleton<GameManager>
{
    public bool autoLoad;
    public bool autoSave;
    public SaveData saveData;

    [Space]
    [Header("Default Fallback Info")]
    [SerializeField]
    private Sprite entry1A1Image = null;

    private List<Quest> activeQuests = new List<Quest>();

    void Start()
    {
        if (autoLoad)
        {
            bool success = LoadDataFromFile();
            if (!success && autoSave)
            {
                SetDefaultCodex();
                SaveDataToFile();
            }
        }
        UIManager.Instance.PreviewData(saveData);
    }

    void OnEnable()
    {
        EventManager.Instance.AddListener<GameEvent.QuestCompleted>(OnQuestCompleted);
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener<GameEvent.QuestCompleted>(OnQuestCompleted);
    }

    void OnApplicationQuit()
    {
        if (autoSave)
        {
            SaveDataToFile();
        }
    }

    [ContextMenu("Set Default Codex")]
    public void SetDefaultCodex()
    {
        SaveData.Codex codex = CreateDefaultCodex();
        SaveData.Notes notes = CreateDefaultNotes();
        saveData = new SaveData(codex, notes);

        SetDirty();
    }

#if UNITY_EDITOR
    [ContextMenu("Console print Directory")]
    private void PrintSaveDirectory()
    {
        Debug.Log(SaveManager.saveDirecty);
    }
#endif

    [ContextMenu("Save Data To File")]
    public void SaveDataToFile()
    {
        SaveManager.Save(saveData);
    }

    [ContextMenu("Load Data From File")]
    public bool LoadDataFromFile()
    {
        SaveData tmp = SaveManager.Load();
        if (tmp != null)
        {
            saveData = tmp;
            SetDirty();
            return true;
        }
        return false;
    }

    private SaveData.Codex CreateDefaultCodex()
    {
        string entry1A1Text = string.Concat(
            "Greater Holsord is the second largest region of\n",
            "Sordland, bordering the regions of Nargis,\n",
            "Geisland, Bergia and Lorren, as well as the\n",
            "country of Lespia. The region is governed from\n",
            "the capital of Sordland, Holsord. It is the most\n",
            "populous region of Sordlands's seven regions\n");
        SaveData.Codex.Category.Topic.Entry entry1A1 = new SaveData.Codex.Category.Topic.Entry("Petr Vectern", entry1A1Image, entry1A1Text);

        SaveData.Codex.Category.Topic.Entry entry1A2 = new SaveData.Codex.Category.Topic.Entry("Lucian Galade", null, "");
        SaveData.Codex.Category.Topic.Entry entry1A3 = new SaveData.Codex.Category.Topic.Entry("Lileas Graf", null, "");
        SaveData.Codex.Category.Topic.Entry entry1A4 = new SaveData.Codex.Category.Topic.Entry("Symon Holl", null, "");
        SaveData.Codex.Category.Topic.Entry entry1A5 = new SaveData.Codex.Category.Topic.Entry("Iosef Lancea", null, "");
        SaveData.Codex.Category.Topic.Entry entry1A6 = new SaveData.Codex.Category.Topic.Entry("Deivid Wisci", null, "");
        SaveData.Codex.Category.Topic.Entry entry1A7 = new SaveData.Codex.Category.Topic.Entry("Nia Morgna", null, "");
        SaveData.Codex.Category.Topic.Entry entry1A8 = new SaveData.Codex.Category.Topic.Entry("Gus Manger", null, "");
        SaveData.Codex.Category.Topic.Entry entry1A9 = new SaveData.Codex.Category.Topic.Entry("Ciara Walda", null, "");
        SaveData.Codex.Category.Topic.Entry entry1A0 = new SaveData.Codex.Category.Topic.Entry("Paskal Beniwoll", null, "");


        SaveData.Codex.Category.Topic topic1A = new SaveData.Codex.Category.Topic("Cabinet",
            entry1A1, entry1A2, entry1A3, entry1A4, entry1A5, entry1A6, entry1A7, entry1A8, entry1A9, entry1A0);

        SaveData.Codex.Category.Topic topic1B = new SaveData.Codex.Category.Topic("Family");
        SaveData.Codex.Category.Topic topic1C = new SaveData.Codex.Category.Topic("Party Leaders");
        SaveData.Codex.Category.Topic topic1D = new SaveData.Codex.Category.Topic("The Assembly");
        SaveData.Codex.Category.Topic topic1E = new SaveData.Codex.Category.Topic("The Supreme Court");
        SaveData.Codex.Category category1 = new SaveData.Codex.Category("Characters", topic1A, topic1B, topic1C, topic1D, topic1E);

        SaveData.Codex.Category category2 = new SaveData.Codex.Category("Locations");

        SaveData.Codex.Category category3 = new SaveData.Codex.Category("Organisations");

        SaveData.Codex.Category category4 = new SaveData.Codex.Category("History");

        SaveData.Codex codex = new SaveData.Codex(category1, category2, category3, category4);
        return codex;
    }

    private SaveData.Notes CreateDefaultNotes()
    {
        SaveData.Notes.Act.Note note1A = new SaveData.Notes.Act.Note("I was elected 4th President of Sordland.");
        SaveData.Notes.Act.Note note1B = new SaveData.Notes.Act.Note("I have vetoed a new electoral campaign finance bull that was passed by the Assembly.");
        SaveData.Notes.Act.Note note1C = new SaveData.Notes.Act.Note("I decded to invest in Armadine Industries by purchasing 3000 shares.");
        SaveData.Notes.Act.Note note1D = new SaveData.Notes.Act.Note("I declared my intention to work with the reformists for the constitutional reform.");

        SaveData.Notes.Act act1 = new SaveData.Notes.Act(note1A, note1B, note1C, note1D);
        SaveData.Notes.Act act2 = new SaveData.Notes.Act();
        SaveData.Notes.Act act3 = new SaveData.Notes.Act();
        SaveData.Notes.Act act4 = new SaveData.Notes.Act();

        SaveData.Notes notes = new SaveData.Notes(act1, act2, act3, act4);
        return notes;
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

    public void AddNote(string text, int actIndex)
    {
        SaveData.Notes.Act act = saveData.notes.GetChildren()[actIndex] as SaveData.Notes.Act;
        SaveData.Notes.Act.Note newNote = new SaveData.Notes.Act.Note(text);
        act.notes.Add(newNote);
    }

    public void DeleteNote(int index, int actIndex)
    {
        SaveData.Notes.Act act = saveData.notes.GetChildren()[actIndex] as SaveData.Notes.Act;
        act.notes.RemoveAt(index);
    }

    public void SetTimeScale(float value)
    {
        Time.timeScale = value;
    }

    private void OnQuestCompleted(GameEvent.QuestCompleted e)
    {
        Quest quest = QuestManager.Instance.GetQuestWithID(e.questID);
        AddNote(quest.title, 0);
        
        var tmpEntries = saveData.codex.categories[0].topics[0].entries;
        var newEntries = new SaveData.Codex.Category.Topic.Entry[tmpEntries.Length + 1];
        for (int i = 0; i < tmpEntries.Length; i++)
        {
            newEntries[i] = tmpEntries[i];
        }
        newEntries[tmpEntries.Length] = new SaveData.Codex.Category.Topic.Entry(quest.title, null, quest.description);
        saveData.codex.categories[0].topics[0].entries = newEntries;

        UIManager.Instance.ClearPreview();
        UIManager.Instance.PreviewData(saveData);
    }
}

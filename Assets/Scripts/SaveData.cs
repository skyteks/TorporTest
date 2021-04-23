using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public interface ITab
    {
        ITab[] GetChildren();
        string GetName();
    }

    public enum Levels
    {
        Categories,
        Topics,
        Entries,
        Acts,
        Notes,
    }

    [System.Serializable]
    public class Codex :ITab
    {
        [System.Serializable]
        public class Category : ITab
        {
            [System.Serializable]
            public class Topic : ITab
            {
                [System.Serializable]
                public class Entry : ITab
                {
                    public string name;
                    public Sprite image;
                    [TextArea]
                    public string text;

                    public Entry(string _name, Sprite _image, string _text)
                    {
                        name = _name;
                        image = _image;
                        text = _text;
                    }

                    public ITab[] GetChildren()
                    {
                        return new ITab[0];
                    }

                    public string GetName()
                    {
                        return name;
                    }
                }

                public string name;
                public Entry[] entries;

                public Topic(string _name, params Entry[] _entries)
                {
                    name = _name;
                    entries = _entries;
                }

                public ITab[] GetChildren()
                {
                    return entries;
                }

                public string GetName()
                {
                    return name;
                }
            }

            public string name;
            public Topic[] topics;

            public Category(string _name, params Topic[] _topics)
            {
                name = _name;
                topics = _topics;
            }

            public ITab[] GetChildren()
            {
                return topics;
            }

            public string GetName()
            {
                return name;
            }
        }

        public Category[] categories;

        public Codex(params Category[] _categories)
        {
            categories = _categories;
        }

        public ITab[] GetChildren()
        {
            return categories;
        }

        public string GetName()
        {
            return GetType().ToString();
        }
    }

    [System.Serializable]
    public class Notes : ITab
    {
        [System.Serializable]
        public class Act : ITab
        {
            [System.Serializable]
            public class Note : ITab
            {
                public string text;

                public Note(string _text)
                {
                    text = _text;
                }

                public ITab[] GetChildren()
                {
                    return new ITab[0];
                }

                public string GetName()
                {
                    return text;
                }
            }

            public List<Note> notes = new List<Note>();

            public Act(params Note[] _notes)
            {
                notes = new List<Note>(_notes);
            }

            public ITab[] GetChildren()
            {
                return notes.ToArray();
            }

            public string GetName()
            {
                return GetType().ToString();
            }
        }

        public Act act1;
        public Act act2;
        public Act act3;
        public Act act4;

        public Notes(Act _act1, Act _act2, Act _act3, Act _act4)
        {
            act1 = _act1;
            act2 = _act2;
            act3 = _act3;
            act4 = _act4;
        }

        public ITab[] GetChildren()
        {
            return new ITab[4] { act1, act2, act3, act4 };
        }

        public string GetName()
        {
            return GetType().ToString();
        }
    }

    public Codex codex;
    public Notes notes;

    public SaveData(Codex _codex, Notes _Notes)
    {
        codex = _codex;
        notes = _Notes;
    }
}

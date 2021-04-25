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
            return null;
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
                [TextArea]
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
                return notes == null ? new ITab[0] : notes.ToArray();
            }

            public string GetName()
            {
                return "Act";
            }

            public static string ToRoman(int number)
            {
                if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");
                if (number < 1) return string.Empty;
                if (number >= 1000) return "M" + ToRoman(number - 1000);
                if (number >= 900) return "CM" + ToRoman(number - 900);
                if (number >= 500) return "D" + ToRoman(number - 500);
                if (number >= 400) return "CD" + ToRoman(number - 400);
                if (number >= 100) return "C" + ToRoman(number - 100);
                if (number >= 90) return "XC" + ToRoman(number - 90);
                if (number >= 50) return "L" + ToRoman(number - 50);
                if (number >= 40) return "XL" + ToRoman(number - 40);
                if (number >= 10) return "X" + ToRoman(number - 10);
                if (number >= 9) return "IX" + ToRoman(number - 9);
                if (number >= 5) return "V" + ToRoman(number - 5);
                if (number >= 4) return "IV" + ToRoman(number - 4);
                if (number >= 1) return "I" + ToRoman(number - 1);
                throw new ArgumentOutOfRangeException("something bad happened");
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
            return null;
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

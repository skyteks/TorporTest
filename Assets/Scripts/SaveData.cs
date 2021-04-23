using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    [System.Serializable]
    public class Codex
    {
        [System.Serializable]
        public class Category
        {
            [System.Serializable]
            public class Topic
            {
                [System.Serializable]
                public class Entry
                {
                    public string name;
                    public Sprite image;
                    public string text;

                    public Entry(string _name, Sprite _image, string _text)
                    {
                        name = _name;
                        image = _image;
                        text = _text;
                    }
                }

                public string name;
                public Entry[] entries;

                public Topic(string _name, params Entry[] _entries)
                {
                    name = _name;
                    entries = _entries;
                }
            }

            public string name;
            public Topic[] topics;

            public Category(string _name, params Topic[] _topics)
            {
                name = _name;
                topics = _topics;
            }
        }

        public Category[] categories;

        public Codex(params Category[] _categories)
        {
            categories = _categories;
        }
    }

    [System.Serializable]
    public class Notes
    {
        [System.Serializable]
        public class Act
        {
        }

        public Act act1;
        public Act act2;
        public Act act3;
        public Act act4;
    }

    public Codex codex;

    public SaveData(Codex _codex)
    {
        codex = _codex;
    }
}

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
        public class Topic
        {
            [System.Serializable]
            public class Entry
            {
                [System.Serializable]
                public class Content
                {
                    public string title;
                    public Sprite image;
                    public string text;

                    public Content(string _title, Sprite _image, string _text)
                    {
                        title = _title;
                        image = _image;
                        text = _text;
                    }
                }

                public string name;
                public Content[] contents;

                public Entry(string _name, params Content[] _contents)
                {
                    name = _name;
                    contents = _contents;
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

        public Topic[] topics;

        public Codex(params Topic[] _topics)
        {
            topics = _topics;
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

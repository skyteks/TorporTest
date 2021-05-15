using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent
{
    public class QuestAccepted : GameEvent
    {
        public uint questID;
        public bool accepted;

        public QuestAccepted(uint id, bool status)
        {
            questID = id;
            accepted = status;
        }
    }

    public class QuestCompleted : GameEvent
    {
        public uint questID;

        public QuestCompleted(uint id)
        {
            questID = id;
        }
    }
}
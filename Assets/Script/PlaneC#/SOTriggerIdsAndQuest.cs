using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New TriggerIdsAndQuests", menuName = "SO/TriggerIdsAndQuests")]
public class SOTriggerIdsAndQuest : ScriptableObject
{
    public string test;
    public TriggerID[] Triggers;
    public SOQuest[] Quests;
    [Serializable]
    public struct TriggerID {
        public bool IsTriggered;
        public string TriggerDescription;

    }
}
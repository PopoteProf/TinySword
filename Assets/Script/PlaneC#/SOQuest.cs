using System;
using UnityEngine;
[Serializable]
[CreateAssetMenu(fileName = "New QuestData", menuName = "SO/QuestData")]
public class SOQuest : ScriptableObject {
    public SimpleConditionTrigger[] Triggers;
    public int CompletID;
    public string QuestTitle;
    [TextArea]public string QuestDescription;

    public bool IsCompleted() {
         return TriggerAndQuest.IsTriggered(CompletID);
    }
    
    public bool IsConditionsRequier() {
        if (IsCompleted()) return false;
        foreach (var conditionTrigger in Triggers) {
            if (!conditionTrigger.IsValid()) return false;
        }
        return true;
    }
}
using System;

[Serializable]
public class SimpleConditionTrigger {
    public int TriggerID;
    public bool IsTriggered;

    public bool IsValid() {
        return IsTriggered == TriggerAndQuest.IsTriggered(TriggerID);
    }
}
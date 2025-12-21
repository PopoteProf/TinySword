using System;
using UnityEngine;

public static class TriggerAndQuest {
    public static event Action<Tuple<int,bool>> OnTriggerIdChanged;
    public static event Action<SOQuest> OnQuestStart;
    public static event Action<SOQuest> OnQuestEnd;
    
    private static SOTriggerIdsAndQuest _soTriggerAndQuest;
    private static bool[] _triggers;
    public static SOTriggerIdsAndQuest SoTriggerIdsAndQuest => _soTriggerAndQuest;

    public static void SetUpSOTriggerIdsAndQuest(SOTriggerIdsAndQuest soTriggerIdsAndQuest) { 
        _soTriggerAndQuest = soTriggerIdsAndQuest;
        _triggers = new bool[_soTriggerAndQuest.Triggers.Length];
        for (int i = 0; i < _soTriggerAndQuest.Triggers.Length; i++) {
            _triggers[i] = _soTriggerAndQuest.Triggers[i].IsTriggered;
        }
    }

    public static void SetTriggerID(int id, bool value) {
        Debug.Log("Changer ID:"+id+ "to"+value );
        if (id < 0 || id >= _soTriggerAndQuest.Triggers.Length) return;
        if (_triggers[id]== value)return;
        _triggers[id]=value;
        CheckForQuestStart(id);
        OnTriggerIdChanged?.Invoke(new Tuple<int, bool>(id, value));
    }
    public static bool IsTriggered(int id) {
        if (id < 0 || id >= _soTriggerAndQuest.Triggers.Length)return false;
        return _triggers[id];
    }

    private static void CheckForQuestStart(int id) {
        foreach (var quest in _soTriggerAndQuest.Quests) {
            if (quest.IsConditionsRequier()) {
                OnQuestStart?.Invoke(quest);
            }
        }
        foreach (var quest in _soTriggerAndQuest.Quests) {
            if( quest.CompletID == id) OnQuestEnd?.Invoke(quest);
        }
        
    }
    
    

}
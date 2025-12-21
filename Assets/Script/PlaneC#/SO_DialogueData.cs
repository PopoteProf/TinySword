using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New DialogueData", menuName = "SO/DialogueData")]
public class SO_DialogueData : ScriptableObject {
    public DialogueElements[] DialoguePanels;
    [Header("IsQuest")]
    public bool IsQuest;

    [Header("ConditionsToDialogue")] 
    public ConditionTrigger[] ConditionsToDialogue;
    [Header("CompletID")]
    public bool UsCompletID; 
    public int CompletID;
    [Space(10)][Header("Trigger Index On Complet")]
    public bool ChangeTriggerIndexOnComplet;
    public int TriggerId;
    public bool ValueOnComplet;
    
    private bool IsDialogueComplet() {
        if (!UsCompletID) return false;
        return TriggerAndQuest.IsTriggered(CompletID);
    }

    public bool IsConditionsRequier() {
        if (IsDialogueComplet()) return false;
        foreach (var conditionTrigger in ConditionsToDialogue) {
            if (!conditionTrigger.IsValide()) return false;
        }
        return true;
    }
}

[Serializable]
public struct  DialogueElements
{
    public string Header;
    [TextArea]public string Text;
}


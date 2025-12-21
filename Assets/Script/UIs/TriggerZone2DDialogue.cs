using UnityEngine;

public class TriggerZone2DDialogue : TriggerZone2D
{
    [SerializeField] private SO_DialogueData _dialogueData;
    
    protected override void EnterTriggerValide(Collider2D other) {
        if (!_dialogueData.IsConditionsRequier()) return;
        StaticData.StartPlayingDialogue?.Invoke(_dialogueData);
    }
}
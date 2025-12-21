using System;
using UnityEngine;
using UnityEngine.Events;

public class TriggerIdsEvent : MonoBehaviour
{
    [SerializeField] private int _triggerId;
    [SerializeField] private UnityEvent _onTriggeredChangeToTrue;
    [SerializeField] private UnityEvent _onTriggeredChangeToFalse;

    private void Awake() {
        TriggerAndQuest.OnTriggerIdChanged+= TriggerAndQuestOnOnTriggerIdChanged;
    }

    private void OnDestroy() {
        TriggerAndQuest.OnTriggerIdChanged-= TriggerAndQuestOnOnTriggerIdChanged;
    }

    private void TriggerAndQuestOnOnTriggerIdChanged(Tuple<int, bool> obj) {
        if (obj.Item1 == _triggerId)
        {
            if (obj.Item2) _onTriggeredChangeToTrue?.Invoke();
            else _onTriggeredChangeToFalse?.Invoke();
        }
    }
}
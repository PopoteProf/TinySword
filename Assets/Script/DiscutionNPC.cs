using System;
using UnityEngine;

public class DiscutionNPC : MonoBehaviour, IInteractable
{
    public event EventHandler<IInteractable> OnInteractableAutoRemove;
    [SerializeField] private bool _isInteractable = true;
    [SerializeField] private GameObject _interactableUI;
    [SerializeField] private GameObject _currentInteractableUI;
    [SerializeField] private GameObject _QuestUI;

    [SerializeField] private SO_DialogueData[] _soDialogueDatas;

    private SO_DialogueData _currenDialoguedata;

    private void Awake() {
        TriggerAndQuest.OnTriggerIdChanged+= TriggerAndQuestOnOnTriggerIdChanged;
        StaticData.OnRessourcesChanged+= OnRessourcesChanged;
    }

    private void OnDestroy()
    {
        TriggerAndQuest.OnTriggerIdChanged-= TriggerAndQuestOnOnTriggerIdChanged;
        StaticData.OnRessourcesChanged-= OnRessourcesChanged;
    }

    private void Start() {
        CheckForCurrentDialogue();
    }

    private void OnRessourcesChanged()=> CheckForCurrentDialogue();
    private void TriggerAndQuestOnOnTriggerIdChanged(Tuple<int, bool> obj)=> CheckForCurrentDialogue();
    public bool CanInteract() => _isInteractable;
    public float GetToDistance(Vector2 playerPosition) => Vector2.Distance(transform.position, playerPosition);
    public void SetIsPrimaryInteractable(bool value)=> _currentInteractableUI.SetActive(value);
    

    public void SetInteractable(bool value) {
        _interactableUI.SetActive(value);
        if( !value)_currentInteractableUI.SetActive(false);
    }

    
    public void Interact() {
        Debug.Log("Interact");
        if( _currenDialoguedata!=null) StaticData.StartPlayingDialogue.Invoke(_currenDialoguedata);
    }

    private void CheckForCurrentDialogue() {
        foreach (var dialogueData in _soDialogueDatas) {
            if (dialogueData.IsConditionsRequier()) {
                SetUpCurrenDialogue(dialogueData);
                return;
            }
        }

        _isInteractable = false;
        _interactableUI.SetActive(false);
        _currentInteractableUI.SetActive(false);
        _QuestUI.SetActive(false);
    }

    private void SetUpCurrenDialogue(SO_DialogueData dialogueData) {
        if( dialogueData==null) return;
        if (_currenDialoguedata == dialogueData) return;
        _currenDialoguedata = dialogueData;
        _QuestUI.SetActive(_currenDialoguedata.IsQuest);
        
    }
}
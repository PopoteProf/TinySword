using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialoguePanel : MonoBehaviour {
    [SerializeField] private RectTransform _holderPanel;
    [SerializeField] private TMP_Text _txtHeader;
    [SerializeField] private TMP_Text _txtDialogue;
    
    [SerializeField] private float _charcterPerSecond =10;
    [SerializeField] private SO_DialogueData _currentDialogueData;
    
    [Header("Tweening")]
    [SerializeField] private float _openningAnimationTime = 0.75f;
    [SerializeField] private AnimationCurve _openingPivotanimationCurve  = AnimationCurve.EaseInOut(0,0,1,1);
    [SerializeField] private AnimationCurve _openingScaltanimationCurve  = AnimationCurve.EaseInOut(0,0,1,1);
    [Space (5)]
    [SerializeField] private float _closeningAnimationTime = 0.25f;
    [SerializeField] private AnimationCurve _closingPivotanimationCurve  = AnimationCurve.EaseInOut(0,0,1,1);
    [SerializeField] private AnimationCurve _closingScaltanimationCurve  = AnimationCurve.EaseInOut(0,0,1,1);
    private int _currentIndex;
    private PopoteTimer _textTimer;
    private bool _isInDialogue;

    private void Awake() {
        StaticData.StartPlayingDialogue += StartNewDialogue;
    }

    private void OnDestroy() {
        StaticData.StartPlayingDialogue -= StartNewDialogue;
    }

    void Start()
    {
        _textTimer = new PopoteTimer(0);
        _textTimer.OnTimerEnd += OnTimerEnd;
        InputSystem.actions.FindAction("Interact").started += OnButtonInteractstarted;

    }

    private void OnButtonInteractstarted(InputAction.CallbackContext obj) {
        if( !_isInDialogue) return;
        if (_textTimer.IsPlaying)CompletCurrentDialogueElement();
        else NextDialogueElement();
    }

    private void OnTimerEnd(object sender, EventArgs e) {
        _txtDialogue.maxVisibleCharacters =_currentDialogueData.DialoguePanels[_currentIndex].Text.Length ;
    }

    // Update is called once per frame
    void Update() {
        if (_textTimer.IsPlaying) {
            _textTimer.UpdateTimer();
            ManagerTextDisplay();
        }
    }

    public void StartNewDialogue(SO_DialogueData dialogueData)
    {
        _currentDialogueData = dialogueData;
        _currentIndex = 0; 
        StartPlayingNewDialogueElement();
        OpenPanel();
    }

    private void CompletCurrentDialogueElement() {
        _textTimer.Pause();
        _txtDialogue.maxVisibleCharacters =_currentDialogueData.DialoguePanels[_currentIndex].Text.Length;
    }

    private void NextDialogueElement() {
        _currentIndex++;
        if( _currentIndex <_currentDialogueData.DialoguePanels.Length) StartPlayingNewDialogueElement();
        else {
            SetTriggerOnComplete();
            ClosePanel();
            StaticData.EndPlayingDialogue?.Invoke();
        }
    }

    private void SetTriggerOnComplete()
    {
        if (_currentDialogueData.ChangeTriggerIndexOnComplet)
        {
            TriggerAndQuest.SetTriggerID(_currentDialogueData.TriggerId, _currentDialogueData.ValueOnComplet);
        }
    }

    private void StartPlayingNewDialogueElement() {
        _textTimer.Play(_currentDialogueData.DialoguePanels[_currentIndex].Text.Length/_charcterPerSecond);
        _txtHeader.text = _currentDialogueData.DialoguePanels[_currentIndex].Header;
        _txtDialogue.text = _currentDialogueData.DialoguePanels[_currentIndex].Text;
    }

    private void ManagerTextDisplay() {
        _txtDialogue.maxVisibleCharacters =Mathf.RoundToInt(
            _currentDialogueData.DialoguePanels[_currentIndex].Text.Length * _textTimer.T);
    }

    public void OpenPanel() {
        _holderPanel.anchoredPosition = new Vector2(0.5f, 1.5f);
        //_holderPanel.localScale = new Vector3(1, 0);
        _holderPanel.DOPivot(new Vector2(0.5f, 0), _openningAnimationTime).SetEase(_openingPivotanimationCurve);
        //_holderPanel.DOScale(new Vector2(1, 1), _openningAnimationTime).SetEase(_openingScaltanimationCurve);
        _isInDialogue = true;
    }
    private void ClosePanel() {
        _holderPanel.anchoredPosition = new Vector2(0.5f, 0);
        //_holderPanel.localScale = new Vector3(1, 1);
        _holderPanel.DOPivot(new Vector2(0.5f, 1.5f), _closeningAnimationTime).SetEase(_closingPivotanimationCurve);
        //_holderPanel.DOScale(new Vector2(1, 0), _closeningAnimationTime).SetEase(_closingScaltanimationCurve);
        _isInDialogue = false;
    }
}
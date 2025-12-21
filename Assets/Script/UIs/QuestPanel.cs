using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestPanel : MonoBehaviour
{

    [SerializeField] private TMP_Text _txtQuestName;
    [SerializeField] private TMP_Text _txtQuestDescription;

    [SerializeField] private Button _bpNextQuest;
    [SerializeField] private Button _bpPreviewsQuest;
    [SerializeField] private Button _bpQuestLogButton;
    [Header("Tweening")]
    [SerializeField] private float _openningAnimationTime = 0.5f;
    [SerializeField] private AnimationCurve _openingPivotanimationCurve  = AnimationCurve.EaseInOut(0,0,1,1);
    [Space (5)]
    [SerializeField] private float _closeningAnimationTime = 0.25f;
    [SerializeField] private AnimationCurve _closingPivotanimationCurve  = AnimationCurve.EaseInOut(0,0,1,1);
    
    private List<SOQuest> _quests= new List<SOQuest>();
    private int _currentQuestIndex;
    private bool _panelClose =true;
    void Start() {
        TriggerAndQuest.OnQuestStart+=TriggerAndQuestOnOnQuestStart;
        TriggerAndQuest.OnQuestEnd+=TriggerAndQuestOnOnQuestEnd;
        _bpNextQuest.onClick.AddListener(UINextQuest);
        _bpPreviewsQuest.onClick.AddListener(UIPreviewsQuest);
        _bpQuestLogButton.onClick.AddListener(UIClickQuestLogButton);
    }

    private void TriggerAndQuestOnOnQuestEnd(SOQuest obj)
    {
        if (!_quests.Contains(obj)) return;
        _quests.Remove(obj);
        ChangeQuestList();
        if (_quests.Count == 0) {
            ClosePanel();
        }
    }

    private void TriggerAndQuestOnOnQuestStart(SOQuest obj)
    {
        if (_quests.Contains(obj)) return;
        _quests.Add(obj);
        ChangeQuestList();
        if( _panelClose)OpenPanel();
        
    }

    private void ChangeQuestList() {
        if (_quests.Count == 0) {
            _txtQuestName.text = "";
            _txtQuestDescription.text = "";
            _bpNextQuest.gameObject.SetActive(false);
            _bpPreviewsQuest.gameObject.SetActive(false);
        }
        else {
            _currentQuestIndex = _quests.Count - 1;
            DisplayQuestInfos(_quests[_currentQuestIndex]);
        }

        if (_quests.Count <= 1) {
            _bpNextQuest.gameObject.SetActive(false);
            _bpPreviewsQuest.gameObject.SetActive(false);
        }
        else {
            _bpNextQuest.gameObject.SetActive(true);
            _bpPreviewsQuest.gameObject.SetActive(true);
        }
    }

    private void UINextQuest()
    {
        _currentQuestIndex++;
        if( _currentQuestIndex >= _quests.Count) _currentQuestIndex = 0;
        DisplayQuestInfos(_quests[_currentQuestIndex]);
    }

    private void UIPreviewsQuest() {
        _currentQuestIndex--;
        if( _currentQuestIndex <0) _currentQuestIndex = _quests.Count-1;
        DisplayQuestInfos(_quests[_currentQuestIndex]);
    }
    
    private void DisplayQuestInfos(SOQuest quest) {
        _txtQuestName.text = quest.QuestTitle;
        _txtQuestDescription.text = quest.QuestDescription;
    }

    private void UIClickQuestLogButton() {
        if (_panelClose)
        {
            if (_quests.Count == 0) return;
            OpenPanel();
            
        }
        else {
            ClosePanel();
        }
    }

    private void ClosePanel()
    {
        transform.GetComponent<RectTransform>().DOPivotX(1,_closeningAnimationTime).SetEase(_closingPivotanimationCurve);
        _panelClose = true;
    }

    private void OpenPanel()
    {
        transform.GetComponent<RectTransform>().DOPivotX(0,_openningAnimationTime).SetEase(_openingPivotanimationCurve);
        _panelClose = false;
    }
}

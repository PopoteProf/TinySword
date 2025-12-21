using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestPanel : MonoBehaviour
{

    [SerializeField] private TMP_Text _txtQuestName;
    [SerializeField] private TMP_Text _txtQuestDescription;

    [SerializeField] private Button _bpNextQuest;
    [SerializeField] private Button _bpPreviewsQuest;
    
    private List<SOQuest> _quests= new List<SOQuest>();
    private int _currentQuestIndex;
    void Start() {
        TriggerAndQuest.OnQuestStart+=TriggerAndQuestOnOnQuestStart;
        TriggerAndQuest.OnQuestEnd+=TriggerAndQuestOnOnQuestEnd;
        _bpNextQuest.onClick.AddListener(UINextQuest);
        _bpPreviewsQuest.onClick.AddListener(UIPreviewsQuest);
    }

    private void TriggerAndQuestOnOnQuestEnd(SOQuest obj)
    {
        if (!_quests.Contains(obj)) return;
        _quests.Remove(obj);
        ChangeQuestList();
    }

    private void TriggerAndQuestOnOnQuestStart(SOQuest obj)
    {
        if (_quests.Contains(obj)) return;
        _quests.Add(obj);
        ChangeQuestList();
        
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
}

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectionLevelPanelMainMenu : SlidingPanelMainMenu
{
    [Space(5)]
    [SerializeField] private Button _bpReturn;
    [SerializeField] private Button _prfButton;
    [SerializeField] private Transform _transformButtonHolder;
    [Space(5)]
    [SerializeField] private LevelInfo[] _levelInfos;
    
    private List<Button> _levelButtons = new List<Button>();
    
    

    private void Start() {
        _bpReturn.onClick.AddListener(ClosePanel);
        SetUpLevelButtons();
    }

    private void SetUpLevelButtons() {
        foreach (var levelInfo in _levelInfos) {
            if (String.IsNullOrEmpty(levelInfo.LevelName)) continue;
            Button bp = Instantiate(_prfButton, _transformButtonHolder);
            bp.GetComponentInChildren<TMP_Text>().text = levelInfo.LevelName;
            bp.onClick.AddListener(delegate{UIButtonLoadScene(levelInfo.SceneName);});
            _levelButtons.Add(bp);
        }
    }

    private void UIButtonLoadScene(string sceneName) {
        if (SceneManager.GetSceneByName(sceneName) == null) {
            Debug.LogWarning("Scene with the name "+sceneName+" notFound" );
        }
        
        SceneManager.LoadScene(sceneName);
    }

    private void SetButtonInteractable(bool b)
    {
        foreach (var botton in _levelButtons)
        {
            if (botton == null) continue;
            botton.interactable = b;
        }
    }
    protected override void ClosePanel() {
        
        SetButtonInteractable(false);
        _bpReturn.interactable = false;
        base.ClosePanel();
    }

    protected override void OpenPanelFinish() {
        SetButtonInteractable(true);
        _bpReturn.interactable = true;
        if( _levelButtons.Count == 0 ) _bpReturn.Select();
        else _levelButtons[0].Select();
        base.OpenPanelFinish();
    }
    [Serializable]
    private class LevelInfo {
        [Tooltip("Name that will display on the button")]public string LevelName;
        [Tooltip("Name of the Scene Asset in the project, Remember to add the scene to the project in the Scene Manager")]public string SceneName;
    }
}
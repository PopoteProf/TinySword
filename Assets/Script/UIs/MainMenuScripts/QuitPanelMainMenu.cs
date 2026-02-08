using UnityEngine;
using UnityEngine.UI;

public class QuitPanelMainMenu : SlidingPanelMainMenu
{
    
    [SerializeField] private Button _bpReturn;
    [SerializeField] private Button _bpQuit;
    private void Start() {
        _bpReturn.onClick.AddListener(ClosePanel);
        _bpQuit.onClick.AddListener(UIQuite);
    }
    private void UIQuite() {
        Application.Quit();
    }
    protected override void ClosePanel() {
        
        _bpReturn.interactable = false;
        _bpQuit.interactable = false;
        base.ClosePanel();
    }

    protected override void OpenPanelFinish() {
        _bpReturn.interactable = true;
        _bpQuit.interactable = true;
        _bpReturn.Select();
        base.OpenPanelFinish();
    }
}
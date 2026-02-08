using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : SlidingPanelMainMenu
{
    [Space(5)]
    [SerializeField] private Button _bpSelectLevel;
    [SerializeField] private Button _bpOption;
    [SerializeField] private Button _bpCredit;
    [SerializeField] private Button _bpQuit;
    [Space(5)]
    [SerializeField] private SelectionLevelPanelMainMenu _panelLevelSelection;
    [SerializeField] private OptionsPanelMainMenu _panelOptions;
    [SerializeField] private CreditPanelMainMenu _panelCredits;
    [SerializeField] private QuitPanelMainMenu _panelQuit;
    

    private void Start() {
        if( _panelClose) ClosePanel();
        else OpenPanel();
        
        _bpSelectLevel.onClick.AddListener(UISelectionLevel);
        _bpOption.onClick.AddListener(UIOptions);
        _bpCredit.onClick.AddListener(UICredit);
        _bpQuit.onClick.AddListener(UIQuit);
        
        _panelLevelSelection.OnPanelClose+= OtherPanelClose;
        _panelOptions.OnPanelClose+= OtherPanelClose;
        _panelCredits.OnPanelClose+= OtherPanelClose;
        _panelQuit.OnPanelClose+= OtherPanelClose;
    }

    private void OnDestroy() {
        _panelLevelSelection.OnPanelClose-= OtherPanelClose;
        _panelOptions.OnPanelClose-= OtherPanelClose;
        _panelCredits.OnPanelClose-= OtherPanelClose;
        _panelQuit.OnPanelClose -= OtherPanelClose;
    }

    private void UIQuit()
    {
        _panelQuit.OpenPanel();
        ClosePanel();
    }

    private void UICredit()
    {
        _panelCredits.OpenPanel();
        ClosePanel();
    }

    private void UIOptions()
    {
        _panelOptions.OpenPanel();
        ClosePanel();
    }

    private void UISelectionLevel() {
        _panelLevelSelection.OpenPanel();
        ClosePanel();
    }

    private void OtherPanelClose() {
        OpenPanel();
    }

    protected override void ClosePanel() {
        _bpSelectLevel.interactable = false;
        _bpOption.interactable = false;
        _bpCredit.interactable = false;
        _bpQuit.interactable = false;
        base.ClosePanel();
    }

    protected override void OpenPanelFinish() {
        _bpSelectLevel.interactable = true;
        _bpOption.interactable = true;
        _bpCredit.interactable = true;
        _bpQuit.interactable = true;
        _bpSelectLevel.Select();
        base.OpenPanelFinish();
    }
}
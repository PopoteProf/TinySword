using UnityEngine;
using UnityEngine.UI;

public class CreditPanelMainMenu : SlidingPanelMainMenu
{
    [SerializeField] private Button _bpReturn;

    private void Start()
    {
        _bpReturn.onClick.AddListener(ClosePanel);
    }
    protected override void ClosePanel() {
        
        _bpReturn.interactable = false;
        base.ClosePanel();
    }

    protected override void OpenPanelFinish() {
        _bpReturn.interactable = true;
        _bpReturn.Select();
        base.OpenPanelFinish();
    }
}
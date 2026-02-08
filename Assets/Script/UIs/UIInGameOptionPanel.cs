using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIInGameOptionPanel : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Button _buttonOption;
    [SerializeField] private Button _bpMainMenu;
    [SerializeField] private Slider _sliderGeneral;
    [SerializeField] private Slider _sliderMusic;
    [SerializeField] private Slider _sliderAmbiance;
    [SerializeField] private Slider _sliderSFX;
    [Header("Tweening")]
    [SerializeField] private float _openningAnimationTime = 0.5f;
    [SerializeField] private AnimationCurve _openingPivotanimationCurve  = AnimationCurve.EaseInOut(0,0,1,1);
    [Space (5)]
    [SerializeField] private float _closeningAnimationTime = 0.25f;
    [SerializeField] private AnimationCurve _closingPivotanimationCurve  = AnimationCurve.EaseInOut(0,0,1,1);
    [Space (5), Header("Audio")]
    [SerializeField] private AudioElement _audioOnOpen;
    [SerializeField] private AudioElement _audioOnClose;
    
    private bool _panelClose=true ;

    private void Start() {
        _buttonOption.onClick.AddListener(UIOpenClosePanel);
        _bpMainMenu.onClick.AddListener(UIReturnToMainMenu);
        _sliderGeneral.onValueChanged.AddListener(ChangeGenrealValue);
        _sliderMusic.onValueChanged.AddListener(ChangeMusicValue);
        _sliderAmbiance.onValueChanged.AddListener(ChangeAmbianceValue);
        _sliderSFX.onValueChanged.AddListener(ChangeSfxValue);
        SetUpVolumes();
    }
    
    private void SetUpVolumes() {
        _audioMixer.GetFloat("VolumeGeneral",out float masterValue);
        _audioMixer.GetFloat("VolumeMusic",out float musicValue);
        _audioMixer.GetFloat("VolumeAmbiance",out float ambianceValue);
        _audioMixer.GetFloat("VolumeSFX",out float sfxValue);
        _sliderGeneral.value = Mathf.Exp(masterValue / 20);
        _sliderSFX.value = Mathf.Exp(sfxValue / 20);
        _sliderMusic.value = Mathf.Exp(musicValue / 20);
        _sliderAmbiance.value = Mathf.Exp(ambianceValue / 20);
    }

    public void UIReturnToMainMenu() {
        SceneManager.LoadScene(0);
    }

    private void UIOpenClosePanel() {
        if (!_panelClose) OpenPanel();
        else ClosePanel();
    }
    
    private void ChangeGenrealValue(float value) {
        _audioMixer.SetFloat("VolumeGeneral", Mathf.Log10(value) * 20);
    }
    private void ChangeMusicValue(float value) {
        _audioMixer.SetFloat("VolumeMusic", Mathf.Log10(value) * 20);
    }
    private void ChangeAmbianceValue(float value) {
        _audioMixer.SetFloat("VolumeAmbiance", Mathf.Log10(value) * 20);
    }
    private void ChangeSfxValue(float value) {
        _audioMixer.SetFloat("VolumeSFX", Mathf.Log10(value) * 20);
    }
    
    
    private void ClosePanel()
    {
        transform.GetComponent<RectTransform>().DOPivotX(1,_closeningAnimationTime).SetEase(_closingPivotanimationCurve);
        _audioOnClose.PlayAsSFX();
        _panelClose = false;
    }

    private void OpenPanel()
    {
        transform.GetComponent<RectTransform>().DOPivotX(0,_openningAnimationTime).SetEase(_openingPivotanimationCurve);
        _audioOnOpen.PlayAsSFX();
        _panelClose = true;
    }
}
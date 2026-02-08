using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsPanelMainMenu : SlidingPanelMainMenu
{
    [Space(5)]
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Slider _sliderGeneral;
    [SerializeField] private Slider _sliderMusic;
    [SerializeField] private Slider _sliderAmbiance;
    [SerializeField] private Slider _sliderSFX;
    [SerializeField] private Button _bpReturn;
    
    private void Start() {
        _bpReturn.onClick.AddListener(ClosePanel);
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
    protected override void ClosePanel() {
        _sliderGeneral.interactable = false;
        _sliderMusic.interactable = false;
        _sliderAmbiance.interactable = false;
        _sliderSFX.interactable = false;
        _bpReturn.interactable = false;
        base.ClosePanel();
    }

    protected override void OpenPanelFinish() {
        _sliderGeneral.interactable = true;
        _sliderMusic.interactable = true;   
        _sliderAmbiance.interactable =    true;
        _sliderSFX.interactable = true;
        _bpReturn.interactable = true;
        _bpReturn.Select();
        base.OpenPanelFinish();
    }
}
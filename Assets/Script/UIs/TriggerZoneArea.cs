using UnityEngine;

public class TriggerZoneArea : TriggerZone2D
{
    [SerializeField] private string _areaName;
    [Header("AreaAudio")] 
    [SerializeField] private bool _changeMusic;
    [SerializeField] private AudioElement _areaMusic;
    [SerializeField] private bool _changeAmbiance;
    [SerializeField] private AudioElement _areaAmbiance;
    protected override void EnterTriggerValide(Collider2D other) {
        Debug.Log("Trigger Area");
        StaticData.OnEnterArea?.Invoke(_areaName);
        if (_changeMusic) AudioBus.OnPlayAudioElementMusic?.Invoke(_areaMusic);
        if (_changeAmbiance) AudioBus.OnPlayAudioElementAmbiance?.Invoke(_areaAmbiance);
    }
}
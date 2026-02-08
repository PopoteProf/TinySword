using System;
using UnityEngine;

public class PlayMusicAndAmbianceAtStart : MonoBehaviour {
    [SerializeField] private AudioElement _music;
    [SerializeField] private AudioElement _ambiance;

    private void Start() {
        AudioBus.OnPlayAudioElementMusic?.Invoke(_music);
        AudioBus.OnPlayAudioElementAmbiance?.Invoke(_ambiance);
    }
}


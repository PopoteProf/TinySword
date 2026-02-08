using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerAudioPart : MonoBehaviour {
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private AudioMixerGroup _sfxAudioMixer;
    [SerializeField] private AudioElement _attack;
    [SerializeField] private AudioElement _damaged;
    [SerializeField] private AudioElement _kill;
    [SerializeField] private AudioElement _footStep;
    [SerializeField] private float _footStepDuration=0.5f;
    private float _timer;
    private bool _iswalking = false;
    

    private void Start()
    {
        _playerController.OnAttack+= OnAttack;
        _playerController.OnDamaged+= OnDamaged;
        _playerController.OnDie+= OnKill;
        _playerController.OnWalkingChangeStat+= OnWalkingChangeStat;
    }

    private void Update() { 
        if (_iswalking) {
            _timer += Time.deltaTime;
            if (_timer >= _footStepDuration) {
                _footStep.PlaySFXOnGameObject(gameObject, _sfxAudioMixer);
                _timer = 0;
            }
        }
    }

    private void OnWalkingChangeStat(bool obj) {
        _iswalking = obj;
    }

    private void OnKill(IDamagable obj) {
        _kill.PlayAsSFX();
    }

    private void OnDamaged() {
        _damaged.PlaySFXOnGameObject(gameObject, _sfxAudioMixer);
    }

    private void OnAttack() {
        _attack.PlaySFXOnGameObject(gameObject, _sfxAudioMixer);
    }
}
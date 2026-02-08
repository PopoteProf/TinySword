using UnityEngine;
using UnityEngine.Audio;

public class ArcherAudioPart : MonoBehaviour
{
    [SerializeField] private ArcherEnemy _ennemi;
    [SerializeField] private SpriteRenderer _targetRender;
    [SerializeField] private AudioMixerGroup _sfxAudioMixer;
    [SerializeField] private AudioElement _attack;
    [SerializeField] private AudioElement _damaged;
    [SerializeField] private AudioElement _kill;
    [SerializeField] private AudioElement _trigger;
    [SerializeField] private AudioElement _footStep;
    [SerializeField] private float _footStepDuration=0.2f;
    private float _timer;
    private bool _iswalking = false;
    private bool _isVisible =true;
    

    private void Start()
    {
        _ennemi.OnEnnemiAttack+= OnAttack;
        _ennemi.OnTakeDamage+= OnDamaged;
        _ennemi.OnKill+= OnKill;
        _ennemi.OnChangeWalkingStat+= OnWalkingChangeStat;
        _ennemi.OnEnnemiTrigger += OnEnnemiTrigger;
    }

    private void OnEnnemiTrigger() {
        _trigger.PlaySFXOnGameObject(gameObject, _sfxAudioMixer);
    }

    private void Update()
    {
        if (_targetRender != null) _isVisible = _targetRender.isVisible;
        if (_iswalking&& _isVisible) {
            _timer += Time.deltaTime;
            if (_timer >= _footStepDuration) {
                _footStep.PlaySFXOnGameObject(gameObject, _sfxAudioMixer);
                _timer = 0;
            }
        }
    }

    private void OnWalkingChangeStat(bool obj) {
        if (!_isVisible) return;
        _iswalking = obj;
    }

    private void OnKill(IDamagable obj)
    {
        if (!_isVisible) return;
        _kill.PlayAsSFX();
    }

    private void OnDamaged() {
        if (!_isVisible) return;
        _damaged.PlaySFXOnGameObject(gameObject, _sfxAudioMixer);
    }

    private void OnAttack() {
        if (!_isVisible) return;
        _attack.PlaySFXOnGameObject(gameObject, _sfxAudioMixer);
    }
}
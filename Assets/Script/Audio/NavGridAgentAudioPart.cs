using UnityEngine;
using UnityEngine.Audio;

public class NavGridAgentAudioPart : MonoBehaviour
{
    [SerializeField] private NavGridAgent _NavGridAgent;
    [SerializeField] private SpriteRenderer _targetRender;
    [SerializeField] private AudioMixerGroup _sfxAudioMixer;
    [SerializeField] private AudioElement _damaged;
    [SerializeField] private AudioElement _kill;
    [SerializeField] private AudioElement _footStep;
    [SerializeField] private float _footStepDuration=0.2f;
    private float _timer;
    private bool _iswalking = false;
    private bool _isVisible =true;
    

    private void Start() {
        _NavGridAgent.OnTakeDamage+= OnDamaged;
        _NavGridAgent.OnKill+= OnKill;
        _NavGridAgent.OnChangeWalkingStat+= OnWalkingChangeStat;
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
}
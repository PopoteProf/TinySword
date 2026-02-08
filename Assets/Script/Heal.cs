using UnityEngine;

public class Heal : MonoBehaviour
{
    [SerializeField] private int _healthAmout;
    [SerializeField] private bool _fullHealthSecurity =true;
    [SerializeField] private float _pickableLifeTime = -1;
    [SerializeField] private PickableSpriteFeedBack _pickableSpriteFeedBack;
    
    
    private bool _wasTriggered = false;

    private void Start()
    {
        if (_pickableLifeTime != -1) {
            Invoke("StartFadeOut", _pickableLifeTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if( _wasTriggered ) return;
        Debug.Log("other.name");
        if( other.CompareTag("Player")) {
            if( _fullHealthSecurity&&other.GetComponent<PlayerController>().NormalizeHealth==1)return;
            other.GetComponent<PlayerController>().HealPlayer(_healthAmout);
            _pickableSpriteFeedBack.DoPickup();
            _wasTriggered = true;
            Destroy(gameObject, 0.5f);
        }
    }

    private void StartFadeOut() {
        _pickableSpriteFeedBack.DoFadeOut();
        Destroy(gameObject, 5f);
    }
}
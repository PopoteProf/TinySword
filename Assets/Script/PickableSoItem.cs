using UnityEngine;

[SelectionBase]
public class PickableSoItem : MonoBehaviour {
    [SerializeField] private SOItem _soItem;
    [SerializeField] private float _pickableLifeTime = -1;
    [SerializeField] private PickableSpriteFeedBack _pickableSpriteFeedBack;
    
    
    private bool _wasTriggered = false;

    private void Start() {
        if (_pickableLifeTime != -1) {
            Invoke("StartFadeOut", _pickableLifeTime);
        }
        if(_soItem!=null) _pickableSpriteFeedBack.SetUpSprite(_soItem.Sprite);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if( _wasTriggered ) return;
        Debug.Log("other.name");
        if( other.CompareTag("Player"))
        {
            if (_soItem == null) return;
            StaticData.AddItemToInventory(_soItem);
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
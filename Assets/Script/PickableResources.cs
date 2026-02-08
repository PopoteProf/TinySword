using System;
using UnityEngine;
[SelectionBase]
public class PickableResources : MonoBehaviour {
    [SerializeField] private StaticData.RessourcesType _ressourcesType;
    [SerializeField] private int _resourcesAmout = 5;
    [SerializeField] private float _pickableLifeTime = -1;
    [SerializeField] private PickableSpriteFeedBack _pickableSpriteFeedBack;
    
    
    private bool _wasTriggered = false;

    private void Start() {
        if (_pickableLifeTime != -1) {
            Invoke("StartFadeOut", _pickableLifeTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if( _wasTriggered ) return;
        Debug.Log("other.name");
        if( other.CompareTag("Player")) {
            switch (_ressourcesType) {
                case StaticData.RessourcesType.Gold:StaticData.ChangeGold(_resourcesAmout); break;
                case StaticData.RessourcesType.Wood:StaticData.ChangeWood(_resourcesAmout); break;
                case StaticData.RessourcesType.Food:StaticData.ChangeFood(_resourcesAmout); break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
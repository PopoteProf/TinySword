using System;
using UnityEngine;

public class TestInteractor : MonoBehaviour, IInteractable
{
    [SerializeField] private bool _isInteractable = true;
    [SerializeField] private GameObject _interactableUI;
    [SerializeField] private GameObject _currentInteractableUI;
    [SerializeField] private GameObject _prfParticuleSistem;
    [SerializeField] private bool _destroyOnInteraction;
    
    public event EventHandler<IInteractable> OnInteractableAutoRemove;
    public float GetToDistance(Vector2 playerPosition) => Vector2.Distance(transform.position, playerPosition);
    public bool CanInteract() => _isInteractable;

    public void SetInteractable(bool value) {
        _interactableUI.SetActive(value);
        if( !value)_currentInteractableUI.SetActive(false);
    }

    public void SetIsPrimaryInteractable(bool value)=> _currentInteractableUI.SetActive(value);
    public void Interact() {
        Instantiate(_prfParticuleSistem, transform.position, Quaternion.identity);
        if( _destroyOnInteraction) OnInteractableAutoRemove?.Invoke(this, this);
    }

    
    
    
}
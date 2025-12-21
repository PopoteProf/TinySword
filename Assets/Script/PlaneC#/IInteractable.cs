using System;
using UnityEngine;

public interface IInteractable
{
    public event EventHandler<IInteractable> OnInteractableAutoRemove;
    
    public void Interact();
    public bool CanInteract();
    public void SetInteractable(bool value);
    public void SetIsPrimaryInteractable(bool value);
    public float GetToDistance(Vector2 playerPosition);
}
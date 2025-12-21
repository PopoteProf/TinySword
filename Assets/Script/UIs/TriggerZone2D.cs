using UnityEngine;

public class TriggerZone2D : MonoBehaviour {
    [SerializeField] private string _playerTag;
    [SerializeField] private bool _deactivateAfterTrigger;
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == _playerTag)
        {
            EnterTriggerValide(other);
            if( _deactivateAfterTrigger)gameObject.SetActive(false);
        }
    }

    protected virtual void EnterTriggerValide(Collider2D other)
    {
        
    }
}
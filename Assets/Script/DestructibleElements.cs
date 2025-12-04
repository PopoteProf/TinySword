using System;
using UnityEngine;

[SelectionBase]
public class DestructibleElements : MonoBehaviour {
    public event EventHandler<DestructibleElements> OnElementsDestruction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("DestroyElement")]
    public void DestroyElement() {
        OnElementsDestruction?.Invoke(this, this);
        Destroy(gameObject);
    }
}

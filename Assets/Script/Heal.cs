using System;
using UnityEngine;

public class Heal : MonoBehaviour
{
    [SerializeField] private int _healthAmout;
    [SerializeField] private bool _fullHealthSecurity =true;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("other.name");
        if( other.CompareTag("Player")) {
            if( _fullHealthSecurity&&other.GetComponent<PlayerController>().NormalizeHealth==1)return;
            other.GetComponent<PlayerController>().HealPlayer(_healthAmout);
            Destroy(gameObject);
        }
    }
}

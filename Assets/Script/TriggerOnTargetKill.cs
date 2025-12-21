using System.Collections.Generic;
using UnityEngine;

public class TriggerOnTargetKill : MonoBehaviour {
    [Header("TRigger On Kills Complets")]
    [SerializeField]private SimpleConditionTrigger _triggerOnComplet;
    [SerializeField]private bool _destroyOnComplet = true;
    [SerializeField]public List<NavGridAgent> _targets = new List<NavGridAgent>();
    

    private void Start() {
        foreach (var target in _targets) {
            if( target==null) continue;
            target.OnKill+= OnKill;
        }
    }

    private void OnDestroy()
    {
        foreach (var target in _targets) {
            if( target==null) continue;
            target.OnKill-= OnKill;
        }
    }

    private void OnKill(NavGridAgent obj) {
        if (!_targets.Contains(obj)) return;
        _targets.Remove(obj);
        CheckIfAllTargetKill();
    }

    private void CheckIfAllTargetKill() {
        foreach (var target in _targets) {
            if( target!=null) return;
        }
        TriggerAndQuest.SetTriggerID(_triggerOnComplet.TriggerID, _triggerOnComplet.IsTriggered);
        if( _destroyOnComplet) Destroy(gameObject);
    }
    
    private void OnDrawGizmosSelected() {
        foreach (var target in _targets) {
            if( target==null) continue;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(target.transform.position, Vector3.one);
            Gizmos.DrawLine(target.transform.position, transform.position);
        }
    }
}
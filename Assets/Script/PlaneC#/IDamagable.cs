using System;
using UnityEngine;

public interface IDamagable
{
    
    public enum AttackerType {
        Player, Enemy, Explosive
    }
    public void TakeDamage(int damage, Vector2 direction, AttackerType attackerType);
}
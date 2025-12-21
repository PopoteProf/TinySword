using System;
using UnityEngine;
using UnityEngine.TextCore.Text;

public static class StaticData
{
    public static Vector2 PlayerPos;
    
    public static Action<float> PlayerHealthChanged;
    public static Action<SO_DialogueData> StartPlayingDialogue;
    public static Action EndPlayingDialogue;
    public static Action OnRessourcesChanged;

    public enum RessourcesType { Gold, Wood, Food }
    public static int Gold;
    public static int Wood;
    public static int Food;

    public static void SetUpInitialRessources(int gold, int wood, int food) {
        Gold = gold;
        Wood = wood;
        Food = food;
    }

    public static void ChangeGold(int value)
    {
        Gold = Mathf.Clamp(Gold - value, 0, Gold + value);
        OnRessourcesChanged?.Invoke();
    }

    public static void ChangeWood(int value) {
        Wood = Mathf.Clamp(Wood - value, 0,Wood + value );
        OnRessourcesChanged?.Invoke();
    }
    
    public static void ChangeFood(int value) {
        Food = Mathf.Clamp(Food - value, 0,Food + value );
        OnRessourcesChanged?.Invoke();
    }
    
}
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SOTriggerIdsAndQuest _soTriggerIdsAndQuest;

    [Space(10), Header("InitialRessources")] 
    [SerializeField] private int _initialGold;
    [SerializeField] private int _initialFood;
    [SerializeField] private int _initialWood;

    private void Awake() {
        TriggerAndQuest.SetUpSOTriggerIdsAndQuest(_soTriggerIdsAndQuest);
        StaticData.SetUpInitialRessources(_initialGold, _initialWood, _initialFood);
    }
}
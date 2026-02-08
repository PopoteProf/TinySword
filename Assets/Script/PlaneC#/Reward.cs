using System;
[Serializable]
public class Reward {
    public enum RewardType {
        Resource, Item
    }
    public RewardType Type;
   
    public StaticData.RessourcesType ResourceType;
    public int ResourceAmount;
   
    public SOItem SoItem;

    public void GiveReward() {
        if (Type == RewardType.Item) {
            StaticData.AddItemToInventory(SoItem);
        }
        if( Type == RewardType.Resource) {
            switch (ResourceType) {
                case StaticData.RessourcesType.Gold:StaticData.ChangeGold(ResourceAmount); break;
                case StaticData.RessourcesType.Wood:StaticData.ChangeWood(ResourceAmount); break;
                case StaticData.RessourcesType.Food:StaticData.ChangeFood(ResourceAmount); break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}
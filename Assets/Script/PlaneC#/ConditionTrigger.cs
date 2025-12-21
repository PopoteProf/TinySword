using System;

[Serializable]
public struct ConditionTrigger {
   public enum ConditionType {
      TriggerID,Ressourses
   }
   public enum ComparatorType {
      Greater,
      Less,
      Equal,
      NotEqual
   }
   
   public ConditionType Type;
   
   public int TriggerID;
   public bool IsTriggered;
   
   public StaticData.RessourcesType Ressource;
   public ComparatorType Comparator;
   public int RessourceValue;

   public bool IsValide() {
      if (Type == ConditionType.TriggerID) {
         return IsTriggered==TriggerAndQuest.IsTriggered(TriggerID);
      }

      if (Type == ConditionType.Ressourses) {
         switch (Ressource) {
            case StaticData.RessourcesType.Gold:return IsValueTrue(StaticData.Gold);
            case StaticData.RessourcesType.Wood:return IsValueTrue(StaticData.Wood);
            case StaticData.RessourcesType.Food:return IsValueTrue(StaticData.Food);
            default:
               throw new ArgumentOutOfRangeException();
         }
      }
      return false;
   }

   private bool IsValueTrue(int compareValue) {
      switch (Comparator) {
         case ComparatorType.Greater: return compareValue > RessourceValue;
         case ComparatorType.Less:return compareValue < RessourceValue;
         case ComparatorType.Equal:return compareValue == RessourceValue;
         case ComparatorType.NotEqual:return compareValue != RessourceValue;
         default:
            throw new ArgumentOutOfRangeException();
      }
   }
}
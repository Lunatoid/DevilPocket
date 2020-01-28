using UnityEngine;


public class Item : MonoBehaviour {

    public enum ItemType {
        None,
        Aas,
        Paracetamol,
        Ibuprofen,
        Morphine,
        BattlePass
    }

    //item pricees 
    public static int GetCost(ItemType itemType) {
        switch (itemType) {
            default:
            case ItemType.None:         return 0;
            case ItemType.Aas:          return 5;
            case ItemType.Paracetamol:  return 20;
            case ItemType.Ibuprofen:    return 50;
            case ItemType.Morphine:     return 100;
            case ItemType.BattlePass:   return 1500;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShopCostumer {
    void BoughtItem(Item.ItemType itemType);
    bool TrySpendGoldAmount(int goldAmount);

}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCostumerShop : MonoBehaviour {

    [SerializeField]
    private Shop_UI shop_UI;

    IShopCostumer shopCostumer;

    private void Start() {
        shopCostumer = GameObject.Find("PlayerInventory").GetComponent<PlayerInventory>();
    }

    public void EnterShop() {
        shop_UI.Show(shopCostumer);
    }

    public void ExitShop() {
        shop_UI.Hide();
    }

}

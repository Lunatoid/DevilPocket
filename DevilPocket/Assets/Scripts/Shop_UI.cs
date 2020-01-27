using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class Shop_UI : MonoBehaviour {
    private Transform container;
    private Transform shopItemTemplate;
    private IShopCostumer shopCostumer;


    public Sprite aas;
    public Sprite paracetamol;
    public Sprite ibuprofen;
    public Sprite morphine;
    public Sprite battlePass;

    private void Awake() {
        container = transform.Find("container");
        shopItemTemplate = container.Find("shopItemTemplate");
        shopItemTemplate.gameObject.SetActive(false);
    }


    private void Start() {
        CreateItemButton(Item.ItemType.Aas, aas, "Aas", Item.GetCost(Item.ItemType.Aas), 0);
        CreateItemButton(Item.ItemType.Paracetamol, paracetamol, "Paracetamol", Item.GetCost(Item.ItemType.Paracetamol), 1);
        CreateItemButton(Item.ItemType.Ibuprofen, ibuprofen, "Ibuprofen", Item.GetCost(Item.ItemType.Ibuprofen), 2);
        CreateItemButton(Item.ItemType.Morphine, morphine, "Morphine", Item.GetCost(Item.ItemType.Morphine), 3);
        CreateItemButton(Item.ItemType.BattlePass, battlePass, "Battle pass", Item.GetCost(Item.ItemType.BattlePass), 4);

        Hide();
    }

    private void CreateItemButton(Item.ItemType itemType, Sprite itemSprite, string itemName, int itemCost, int positionIndex) {
        Transform shopItemTransform = Instantiate(shopItemTemplate, container);
        RectTransform shopItemRectTransform = shopItemTransform.GetComponent<RectTransform>();

        float shopItemHeight = 200.0f;
        shopItemRectTransform.anchoredPosition = new Vector2(0, -shopItemHeight * positionIndex);

        shopItemTransform.Find("ItemText").GetComponent<TextMeshProUGUI>().SetText(itemName);
        shopItemTransform.Find("valueText").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());

        shopItemTransform.Find("ImageItem").GetComponent<Image>().sprite = itemSprite;

        shopItemTransform.GetComponent<Button>().onClick.AddListener(delegate { TryBuyItem(itemType); });

    }

    public void TryBuyItem(Item.ItemType itemType) {
        if (shopCostumer.TrySpendGoldAmount(Item.GetCost(itemType))) {
            shopCostumer.BoughtItem(itemType);
        }
    }

    public void Show(IShopCostumer shopCostumer) {
        this.shopCostumer = shopCostumer;
        gameObject.SetActive(true);
    }

    public void Hide() {
        //gameObject.SetActive(false);
    }

}

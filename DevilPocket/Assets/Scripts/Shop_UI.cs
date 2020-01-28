using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

using UnityStandardAssets.Characters.FirstPerson;

public class Shop_UI : MonoBehaviour {
    private Transform container;
    private Transform shopItemTemplate;
    private IShopCostumer shopCostumer;


    public Sprite aas;
    public Sprite paracetamol;
    public Sprite ibuprofen;
    public Sprite morphine;
    public Sprite battlePass;

    PlayerInventory playerInventory;
    FirstPersonController fps;

    private void Awake() {
        container = transform.Find("container");
        shopItemTemplate = container.Find("shopItemTemplate");

        playerInventory = GameObject.Find("PlayerInventory").GetComponent<PlayerInventory>();
    }


    private void Start() {
        CreateItemButton(Item.ItemType.Aas, aas, "Aas", Item.GetCost(Item.ItemType.Aas), 0);
        CreateItemButton(Item.ItemType.Paracetamol, paracetamol, "Paracetamol", Item.GetCost(Item.ItemType.Paracetamol), 1);
        CreateItemButton(Item.ItemType.Ibuprofen, ibuprofen, "Ibuprofen", Item.GetCost(Item.ItemType.Ibuprofen), 2);
        CreateItemButton(Item.ItemType.Morphine, morphine, "Morphine", Item.GetCost(Item.ItemType.Morphine), 3);
        CreateItemButton(Item.ItemType.BattlePass, battlePass, "Battle pass", Item.GetCost(Item.ItemType.BattlePass), 4);

        // We don't need the template anymore
        Destroy(shopItemTemplate.gameObject);

        fps = GameObject.Find("Player").GetComponent<FirstPersonController>();

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
            // Update any quests that want to buy items
            playerInventory.UpdateCompletion(GoalType.BuyItems, 1, itemType);

            shopCostumer.BoughtItem(itemType);
        }
    }

    public void Show(IShopCostumer shopCostumer) {
        if (!fps) {
            fps = GameObject.Find("Player").GetComponent<FirstPersonController>();
        }
        fps.enabled = false;


        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        this.shopCostumer = shopCostumer;
        container.gameObject.SetActive(true);
    }

    public void Hide() {
        if (fps) {
            fps.enabled = true;
        }

        container.gameObject.SetActive(false);
    }

}

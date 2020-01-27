using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop_UI : MonoBehaviour
{
    private Transform container;
    private Transform shopItemTemplate;

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
        CreateItemButton(aas, "Aas", Item.GetCost(Item.ItemType.Aas),0);
        CreateItemButton(paracetamol, "Paracetamol", Item.GetCost(Item.ItemType.Paracetamol), 1);
        CreateItemButton(ibuprofen, "Ibuprofen", Item.GetCost(Item.ItemType.Ibuprofen), 2);
        CreateItemButton(morphine, "Morphine", Item.GetCost(Item.ItemType.Morphine), 3);
        CreateItemButton(battlePass, "Battle pass", Item.GetCost(Item.ItemType.BattlePass),4);
    }

    private void CreateItemButton(Sprite itemSprite, string itemName, int itemCost, int positionIndex) {
        Transform shopItemTransform = Instantiate(shopItemTemplate, container);
        RectTransform shopItemRectTransform = shopItemTransform.GetComponent<RectTransform>();

        float shopItemHeight = 30.0f;
        shopItemRectTransform.anchoredPosition = new Vector2(0, -shopItemHeight * positionIndex);

        shopItemTransform.Find("ItemText").GetComponent<TextMeshProUGUI>().SetText(itemName);
        shopItemTransform.Find("valueText").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());

        shopItemTransform.Find("").GetComponent<Image>().sprite = itemSprite;
    }


}

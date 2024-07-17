using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryBox : MonoBehaviour {

    private int key;
    // 인벤 박스 -> 버튼
    public Button invenBox;
    [SerializeField] private Text itemText;
    [SerializeField] private Image itemIcon;

    // 아이템을 사용하는지 확인
    public bool isItemUse = false;
    // 아이템이 들어가있는지 확인
    public bool isItemIn = false;

    private Item currentItem;
    public Item CurrentItem { get { return currentItem; } }

    private PlayerItemUseControll playerItemUse;

    private void Awake() {
        invenBox = transform.GetComponent<Button>();
        itemText = transform.GetChild(0).GetComponent<Text>();
        itemIcon = transform.GetChild(1).GetComponent<Image>();
        playerItemUse = FindObjectOfType<PlayerItemUseControll>();
        //Inven_Text.text = Item_count.ToString();
        invenBox.onClick.AddListener(OnBoxClicked);
    }
    public void setKey(int key) {
        this.key = key;
    }

    public void UpdateBox(Item item) {
        currentItem = item;

        if (currentItem is CountableItem countItem) {
            itemText.text = countItem.CurrStackCount.ToString();
            itemIcon.sprite = countItem.itemData.Icon;
            itemIcon.enabled = true;
            itemIcon.gameObject.SetActive(true);
            isItemIn = true;
        }
        else if (currentItem is EquipItem eqItem) {
            itemText.text = "";
            itemIcon.sprite = eqItem.itemData.Icon;
            itemIcon.enabled = true;
            itemIcon.gameObject.SetActive(true);
            isItemIn = true;
        }
        else {
            if (currentItem != null) {
                itemText.text = "";
                itemIcon.sprite = currentItem.itemData.Icon;
                itemIcon.enabled = true;
                itemIcon.gameObject.SetActive(true);
                isItemIn = true;
            }
            else {
                itemText.text = "0";
                itemIcon.sprite = null;
                itemIcon.enabled = false;
                itemIcon.gameObject.SetActive(false);
                isItemIn = false;
            }
        }
    }

    public void OnBoxClicked() {
        playerItemUse.SetSelectedBoxKey(key);
    }

    //TODO: 꾹 누르는 게이지 추가하기
    //TODO: 꾹 누르거나 드래그 - 다떨어짐
    //TODO: 그냥 f 누르면 나무, 광석 8개씩 나머지 1개씩
    //TODO: 제련 돌 30 -> 철광석 1개

}
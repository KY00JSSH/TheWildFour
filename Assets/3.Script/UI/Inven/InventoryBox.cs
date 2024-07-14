using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryBox : MonoBehaviour {

    // 인벤 박스 -> 버튼
    private Button Inven_Box;
    [SerializeField] private Text itemText;
    [SerializeField] private Image itemIcon;

    // 아이템을 사용하는지 확인
    public bool isItemUse = false;
    // 아이템이 들어가있는지 확인
    public bool isItemIn = false;

    private Item currentItem;
    public Item CurrentItem { get { return currentItem; } }

    private void Awake() {
        Inven_Box = transform.GetComponent<Button>();
        itemText = transform.GetChild(0).GetComponent<Text>();
        itemIcon = transform.GetChild(1).GetComponent<Image>();
        //Inven_Text.text = Item_count.ToString();
    }

    public void UpdateBox(Item item) {
        currentItem = item;

        if (currentItem != null) {
            if (currentItem is CountableItem countItem) {
                itemText.text = countItem.countableData.CurrStackCount.ToString();
            }
            else {
                itemText.text = "";
            }
            itemIcon.sprite = currentItem.itemData.Icon;
            itemIcon.enabled = true;
            itemIcon.gameObject.SetActive(true);
            isItemIn = true;
        }
        else {
            itemText.text = "";
            itemIcon.sprite = null;
            itemIcon.enabled = false;
            itemIcon.gameObject.SetActive(false);
            isItemIn = false;
        }
    }
    // item box 클릭시 현재 클릭한 box index로 item 뭉텅이 찾기
    // 
    //private void Update() {
    //    if (Input.GetMouseButtonDown(0)) {
    //        isMouseClick = true;
    //        if (!IsCusorOutButton(Inven_Box)) {
    //            Click_count = 0;
    //            isMouseClick = false;
    //        }

    //    }
    //    if (isMouseClick) {
    //        if (isItemIn && Input.GetKeyDown(KeyCode.E)) {
    //            //invenCon.useItem();
    //        }
    //    }
    //}

    //TODO: 꾹 누르는 게이지 추가하기
    //TODO: 꾹 누르거나 드래그 - 다떨어짐
    //TODO: 그냥 f 누르면 나무, 광석 8개씩 나머지 1개씩
    //TODO: 제련 돌 30 -> 철광석 1개

    //public void ItemUse_Button() {
    //    Click_count++;
    //    if (Click_count >= 2 && isItemIn) {
    //        ItemUse();
    //    }
    //}

    //// 초기화 혹은 버리기
    //private void Init_InvenBox() {
    //    Item_count = 0;
    //    isItemUse = false;
    //    isItemIn = false;
    //    isInvenBoxAvailable = true;
    //    Inven_Item = null;
    //    //itemImgOj = transform.GetComponent<Image>().sprite;
    //    Inven_Sprite.enabled = false;
    //}


    private bool IsCusorOutButton(Button button) {
        Vector2 MCursorPosition = button.GetComponent<RectTransform>().InverseTransformPoint(Input.mousePosition);
        return button.GetComponent<RectTransform>().rect.Contains(MCursorPosition);
    }
}
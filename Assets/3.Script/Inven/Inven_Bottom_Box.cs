using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inven_Bottom_Box : MonoBehaviour {

    /*
     Inven_Bottom_Controll 에서 받은 아이템 저장
     
     
     */

    // 인벤 박스 -> 버튼
    private Button Inven_Box;
    [SerializeField] private Text Inven_Text;
    [SerializeField] private Image Inven_Sprite;

    private int Item_count = 0;

    private int Click_count = 0;
    private bool isMouseClick = false;

    // 아이템을 사용하는지 확인
    public bool isItemUse = false;
    // 아이템이 들어가있는지 확인
    public bool isItemIn = false;
    // 아이템이 80개 찼다면
    public bool isInvenBoxAvailable = true;

    // 인벤 내부 아이템 
    public GameObject Inven_Item { get; private set; }


    private void Awake() {
        Inven_Box = transform.GetComponent<Button>();
        Inven_Text = transform.GetChild(0).GetComponent<Text>();
        Inven_Text.text = Item_count.ToString();

        //TODO: 아이템 스프라이트가 없음 받아올때 확인
        Inven_Sprite = transform.GetChild(1).GetComponent<Image>();
        Inven_Sprite.enabled = false;
    }

    private void Update() {

        if (Input.GetMouseButtonDown(0)) {
            isMouseClick = true;
            if (!IsCusorOutButton(Inven_Box)) {
                Click_count = 0;
                isMouseClick = false;
            }

        }
        if (isMouseClick) {
            if (isItemIn && Input.GetKeyDown(KeyCode.E)) {
                ItemUse();
            }
        }
    }


    //  아이템이 추가될 경우 아이템 저장
    public void ItemIn(GameObject item) {
        if (Inven_Item != null) {
            Item_count++;
            ItemCountCheck();
        }
        else {
            Inven_Item = item;
            isItemIn = true;
            Item_count++;
            ItemCountCheck();
        }
        Debug.Log("아이템 이름 확인" + item.name);
        Debug.Log("아이템 count 확인" + Item_count);
        Inven_Text.text = Item_count.ToString();
        Inven_Sprite.enabled = true;
    }

    // 아이템 사용
    private void ItemUse() {
        if (Item_count == 0) return;
        isItemUse = true;
        Item_count--;
        Inven_Text.text = Item_count.ToString();
        //TODO: 아이템 사용해야합니다
        if (Item_count <= 0) {
            // 초기화
            Init_InvenBox();
        }
        isItemUse = false;
    }

    public void ItemUse_Button() {
        Click_count++;
        if (Click_count >= 2 && isItemIn) {
            ItemUse();
        }
    }

    private void ItemCountCheck() {
        if (Inven_Item.tag == "Item_Food" && Item_count >= 80) {
            isInvenBoxAvailable = false;
        }
        else if (Inven_Item.tag == "Item_Weapon" && Item_count >= 1) {
            isInvenBoxAvailable = false;
        }
        else if (Inven_Item.tag == "Item_Ingre" && Item_count >= 80) {
            isInvenBoxAvailable = false;
        }
        else if (Inven_Item.tag == "Item_Etc" && Item_count >= 1) {
            isInvenBoxAvailable = false;
        }
    }


    // 초기화 혹은 버리기
    private void Init_InvenBox() {
        Item_count = 0;
        isItemUse = false;
        isItemIn = false;
        isInvenBoxAvailable = true;
        Inven_Item = null;
        //itemImgOj = transform.GetComponent<Image>().sprite;
        Inven_Sprite.enabled = false;
    }

    private bool IsCusorOutButton(Button button) {
        Vector2 CursorPosition = button.GetComponent<RectTransform>().InverseTransformPoint(Input.mousePosition);
        return button.GetComponent<RectTransform>().rect.Contains(CursorPosition);
    }

}

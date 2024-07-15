using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;



public class Menu_Tooltip : MonoBehaviour {

    /*
     * 각 버튼(인벤토리 or menu )에 달릴 스크립트
     1. 메뉴 튤팁
        - 각 메뉴 버튼에 있는 데이터를 넣으면 됨
     2. 아이템 툴팁
        - 아이템이 들어오고 나서 찾아야함      
     */


    [SerializeField]private Menu_Controll menuControll;

    private Button button;
    [SerializeField] private ItemData itemData;

    public GameObject tooltipbox;
    [SerializeField] private Text _titleText;   // 아이템 이름 텍스트
    [SerializeField] private Text _contentText; // 아이템 설명 텍스트


    private void Awake() {
        button = GetComponent<Button>();
        menuControll = FindObjectOfType<Menu_Controll>();

    }

    private void Update() {
        if (MCursor.Instance.IsCusorOnButton(button)) {
            // 툴팁 표시
            if (itemData != null) {
                // 메뉴용 
                MenuTooltip();
            }
            else {
                // 인벤토리용
                if (MCursor.Instance.IsCusorOnButton(button)) {
                    InvenItemTooltip();
                }
            }
        }
    }

    private void MenuTooltip() {
        menuControll.ButtonMove(400, false);
        tooltipbox.gameObject.SetActive(true);
        _titleText.text = itemData.ItemName;
        _contentText.text = itemData.Description;
    }


    private void InvenItemTooltip() {
        Transform _inventoryItem = transform.GetChild(1);
        GameObject inventoryItem = _inventoryItem.gameObject;
        if (inventoryItem != null && inventoryItem.GetComponent<Image>().enabled) {
            Debug.Log("Item_Input 아이템이 있음" + inventoryItem.name);
            tooltipbox.gameObject.SetActive(true);
            _titleText.text = itemData.ItemName;
            _contentText.text = itemData.Description;
        }
        else {
            Debug.Log("Item_Input 아이템이 없음");
            return;
        }
    }



}

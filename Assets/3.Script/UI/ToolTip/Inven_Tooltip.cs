using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inven_Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    /*
     * 인벤토리 아이템 Tooltip 
     * inventory box button의 component -> itemdata가 있으면 Tooltip 표시 없으면 리턴
     //TODO: 아이템 신선함 표시, 아이템 기본 이미지 추가 필요함 + 시간 남으면 전반적인 리모델링 

     */

    [SerializeField] private ItemData itemData;

    public GameObject tooltipbox;
    [SerializeField] private Text _titleText;   // 아이템 이름 텍스트
    [SerializeField] private Text _contentText; // 아이템 설명 텍스트


    private void InvenItemTooltip(Button btn) {
        ItemData itemData = btn.GetComponent<InventoryBox>().currentItem;
        if (itemData != null) {
            Debug.Log("Item_Input 아이템이 있음" + itemData.name);
            tooltipbox.gameObject.SetActive(true);
            _titleText.text = itemData.ItemName;
            _contentText.text = itemData.Description;
        }
        else {
            Debug.Log("Item_Input 아이템이 없음");
            return;
        }
    }


    // inventory box button의 component -> itemdata가 있으면 Tooltip 표시 없으면 리턴
    public void OnPointerEnter(PointerEventData eventData) {
        if (eventData.pointerEnter != null && Input.mousePosition.y <= 100) {
            Button btn = eventData.pointerEnter.GetComponent<Button>();
            if (btn != null) {
                Debug.Log(btn.gameObject.name + " - Mouse enter");
                InvenItemTooltip(btn);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (eventData.pointerEnter != null || Input.mousePosition.y > 100) {
            Button btn = eventData.pointerEnter.GetComponent<Button>();
            if (btn != null) {
                Debug.Log(btn.gameObject.name + " - Mouse enter");

                tooltipbox.gameObject.SetActive(false);
                _titleText.text = "";
                _contentText.text = "";
            }
        }
    }
}

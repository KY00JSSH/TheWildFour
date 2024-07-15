using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inven_Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    /*
     * �κ��丮 ������ Tooltip 
     * inventory box button�� component -> itemdata�� ������ Tooltip ǥ�� ������ ����
     //TODO: ������ �ż��� ǥ��, ������ �⺻ �̹��� �߰� �ʿ��� + �ð� ������ �������� ���𵨸� 

     */

    [SerializeField] private ItemData itemData;

    public GameObject tooltipbox;
    [SerializeField] private Text _titleText;   // ������ �̸� �ؽ�Ʈ
    [SerializeField] private Text _contentText; // ������ ���� �ؽ�Ʈ


    private void InvenItemTooltip(Button btn) {
        ItemData itemData = btn.GetComponent<InventoryBox>().currentItem;
        if (itemData != null) {
            Debug.Log("Item_Input �������� ����" + itemData.name);
            tooltipbox.gameObject.SetActive(true);
            _titleText.text = itemData.ItemName;
            _contentText.text = itemData.Description;
        }
        else {
            Debug.Log("Item_Input �������� ����");
            return;
        }
    }


    // inventory box button�� component -> itemdata�� ������ Tooltip ǥ�� ������ ����
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

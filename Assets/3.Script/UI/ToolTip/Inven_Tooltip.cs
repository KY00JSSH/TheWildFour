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

    [SerializeField] private Item item;

    public GameObject tooltipbox;
    [SerializeField] private Text _titleText;   // ������ �̸� �ؽ�Ʈ
    [SerializeField] private Text _contentText; // ������ ���� �ؽ�Ʈ


    private void InvenItemTooltip(Button btn) {
        item = btn.GetComponent<InventoryBox>().CurrentItem;
        if (item != null) {
            Debug.Log("Item_Input �������� ����" + item.name);
            tooltipbox.gameObject.SetActive(true);
            _titleText.text = item.name;
            _contentText.text = item.itemData.Description;
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
                tooltipbox.gameObject.SetActive(false);
                _titleText.text = "";
                _contentText.text = "";
            }
        }
    }
}

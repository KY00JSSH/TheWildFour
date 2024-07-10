using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Tooltip : MonoBehaviour {

    /*
     * �� ��ư(�κ��丮 or menu )�� �޸� ��ũ��Ʈ
     1. �޴� ƫ��
        - �� �޴� ��ư�� �ִ� �����͸� ������ ��
     2. ������ ����
        - �������� ������ ���� ã�ƾ���      
     */
    private Button button;
    [SerializeField] private ItemData itemData;

    public GameObject tooltipbox;
    [SerializeField] private Text _titleText;   // ������ �̸� �ؽ�Ʈ
    [SerializeField] private Text _contentText; // ������ ���� �ؽ�Ʈ

    private void Awake() {
        button = GetComponent<Button>();
    }

    private void Update() {
        if (MCursor.Instance.IsCusorOnButton(button)) {
            // ���� ǥ��
            if (itemData != null) {
                // �޴��� 
                tooltipbox.gameObject.SetActive(true);
                _titleText.text = itemData.ItemName;
                _contentText.text = itemData.Description;
            }
            else {
                // �κ��丮��
                if (MCursor.Instance.IsCusorOnButton(button)) {
                    InventoryItem();
                }
            }
        }
    }

    private void InventoryItem() {
        Transform _inventoryItem = transform.GetChild(1);
        GameObject inventoryItem = _inventoryItem.gameObject;
        if (inventoryItem != null && inventoryItem.GetComponent<Image>().enabled) {
            Debug.Log("Item_Input �������� ����" + inventoryItem.name);
            tooltipbox.gameObject.SetActive(true);
            _titleText.text = itemData.ItemName;
            _contentText.text = itemData.Description;
        }
        else {
            Debug.Log("Item_Input �������� ����");
            return;
        }
    }

}

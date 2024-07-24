using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonInvenBox : MonoBehaviour {
    protected int key;
    // 인벤 박스 -> 버튼
    protected Button invenBox;
    [SerializeField] protected Text itemText;
    [SerializeField] protected Image itemIcon;

    // 아이템이 들어가있는지 확인
    public bool isItemIn = false;

    private GameObject currentItem;
    public GameObject CurrentItem { get { return currentItem; } }

    public void setKey(int key) {
        this.key = key;
    }

    public void UpdateBox(GameObject item) {
        currentItem = item;

        if (currentItem != null) {
            if (currentItem.GetComponent<CountableItem>() != null) {
                CountableItem countItem = currentItem.GetComponent<CountableItem>();
                itemIcon.sprite = countItem.itemData.Icon;
                itemText.enabled = true;
                itemText.text = countItem.CurrStackCount.ToString();
                itemIcon.enabled = true;
                itemIcon.gameObject.SetActive(true);
                isItemIn = true;
                itemText.transform.SetAsLastSibling();
            }
            else if (currentItem.GetComponent<EquipItem>() != null) {
                EquipItem eqItem = currentItem.GetComponent<EquipItem>();
                itemIcon.sprite = eqItem.itemData.Icon;
                itemText.enabled = false;
                itemText.text = "";
                itemIcon.enabled = true;
                itemIcon.gameObject.SetActive(true);
                isItemIn = true;
            }
            else if (currentItem.GetComponent<Item>() != null) {
                itemIcon.sprite = currentItem.GetComponent<Item>().itemData.Icon;
                itemText.enabled = false;
                itemText.text = "";
                itemIcon.enabled = true;
                itemIcon.gameObject.SetActive(true);
                isItemIn = true;
            }
            else {
                itemIcon.sprite = null;
                itemText.text = "";
                itemText.enabled = false;
                itemIcon.enabled = false;
                itemIcon.gameObject.SetActive(false);
                isItemIn = false;
            }
        }
        else {
            itemIcon.sprite = null;
            itemText.text = "";
            itemText.enabled = false;
            itemIcon.enabled = false;
            itemIcon.gameObject.SetActive(false);
            isItemIn = false;
        }
    }
}

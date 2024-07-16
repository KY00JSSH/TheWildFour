using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryBox : MonoBehaviour {

    private int key;
    // �κ� �ڽ� -> ��ư
    public Button invenBox;
    [SerializeField] private Text itemText;
    [SerializeField] private Image itemIcon;

    // �������� ����ϴ��� Ȯ��
    public bool isItemUse = false;
    // �������� ���ִ��� Ȯ��
    public bool isItemIn = false;

    private Item currentItem;
    public Item CurrentItem { get { return currentItem; } }

    private PlayerItemUseControll playerItemUse;

    private void Awake() {
        invenBox = transform.GetComponent<Button>();
        itemText = transform.GetChild(0).GetComponent<Text>();
        itemIcon = transform.GetChild(1).GetComponent<Image>();
        playerItemUse = GetComponent<PlayerItemUseControll>();
        //Inven_Text.text = Item_count.ToString();
        invenBox.onClick.AddListener(onBoxClicked);
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
        else {
            if (currentItem != null) {
                itemText.text = "";
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
    }

    //TODO: �� ������ ������ �߰��ϱ�
    //TODO: �� �����ų� �巡�� - �ٶ�����
    //TODO: �׳� f ������ ����, ���� 8���� ������ 1����
    //TODO: ���� �� 30 -> ö���� 1��

    public void onBoxClicked() {
        Debug.Log("click"+key);
        playerItemUse.SetSelectedBoxKey(key);
    }
}
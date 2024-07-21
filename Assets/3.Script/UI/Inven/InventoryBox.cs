using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryBox : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {

    private int key;
    // �κ� �ڽ� -> ��ư
    public Button invenBox;
    [SerializeField] private Text itemText;
    [SerializeField] private Image itemIcon;

    // �������� ���ִ��� Ȯ��
    public bool isItemIn = false;

    private Item currentItem;
    public Item CurrentItem { get { return currentItem; } }

    private PlayerItemUseControll playerItemUse;
    private InvenDrop invenDrop;
    private InvenController invenControll;
    private InvenUIController invenUI;
    private MenuWeapon menuWeapon;

    private Canvas canvas;
    private RectTransform originalParent;
    private Vector2 originalPosition;

    private void Awake() {
        invenBox = transform.GetComponent<Button>();
        playerItemUse = FindObjectOfType<PlayerItemUseControll>();
        invenControll = FindObjectOfType<InvenController>();
        menuWeapon = FindObjectOfType<MenuWeapon>();
        invenUI = FindObjectOfType<InvenUIController>();
        invenDrop = FindObjectOfType<InvenDrop>();
        canvas = FindObjectOfType<Canvas>();
    }
    public void setKey(int key) {
        this.key = key;
    }

    public void UpdateBox(Item item) {
        currentItem = item;

        if (currentItem is CountableItem countItem) {
            itemIcon.sprite = countItem.itemData.Icon;
            itemText.enabled = true;
            itemText.text = countItem.CurrStackCount.ToString();
            itemIcon.enabled = true;
            itemIcon.gameObject.SetActive(true);
            isItemIn = true;
            itemText.transform.SetAsLastSibling();
        }
        else if (currentItem is EquipItem eqItem) {
            itemIcon.sprite = eqItem.itemData.Icon;
            itemText.enabled = false;
            itemText.text = "";
            itemIcon.enabled = true;
            itemIcon.gameObject.SetActive(true);
            isItemIn = true;
        }
        else {
            if (currentItem != null) {
                itemIcon.sprite = currentItem.itemData.Icon;
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
    }

    //TODO: �� ������ ������ �߰��ϱ�
    //TODO: ���� �� 30 -> ö���� 1��

    public void OnPointerClick(PointerEventData pointerEventData) {
        playerItemUse.SetSelectedBoxKey(key);
    }

    public void OnBeginDrag(PointerEventData eventData) {
        playerItemUse.SetSelectedBoxKey(key);
        if (isItemIn) {
            originalParent = itemIcon.rectTransform.parent as RectTransform;
            originalPosition = itemIcon.rectTransform.anchoredPosition;
            itemIcon.transform.SetParent(canvas.transform, true);
            itemIcon.transform.SetAsLastSibling();
        }
    }

    public void OnDrag(PointerEventData data) {
        if (isItemIn) {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, data.position, data.pressEventCamera, out position);
            itemIcon.rectTransform.anchoredPosition = position;
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (isItemIn) {
            itemIcon.transform.SetParent(originalParent, true);
            itemIcon.rectTransform.anchoredPosition = originalPosition;

            bool isChangeInven = false;
            int targetIndex = 99;

            for (int i = 0; i < invenUI.InvenTotalList.Count; i++) {
                RectTransform boxRectTransform = invenUI.InvenTotalList[i].GetComponent<RectTransform>();
                if (RectTransformUtility.RectangleContainsScreenPoint(boxRectTransform, eventData.position, eventData.pressEventCamera)) {
                    isChangeInven = true;
                    targetIndex = i;
                    break;
                }
            }

            if (isChangeInven) {
                //�κ� ��ġ ����
                //���� �κ� index �� �ٲ� �κ� üũ
                invenControll.changeInvenIndex(key, targetIndex);
            }
            else if (RectTransformUtility.RectangleContainsScreenPoint(menuWeapon.WeapFirstBoxPos, eventData.position, eventData.pressEventCamera)) {
                //���� 1�� �����϶�
                WeaponItemData invenWeapItemData = invenControll.getIndexItem(key);
                if (invenWeapItemData) {
                    WeaponItemData weapPrevItem = menuWeapon?.addItemBox(1, invenWeapItemData);
                    invenControll.changeItemIntoWeapSlot(weapPrevItem, key);
                }
            }
            else if (RectTransformUtility.RectangleContainsScreenPoint(menuWeapon.WeapSecondBoxPos, eventData.position, eventData.pressEventCamera)) {
                //���� 2�� �����ϋ�
                WeaponItemData invenWeapItemData = invenControll.getIndexItem(key);
                if (invenWeapItemData) {
                    WeaponItemData weapPrevItem = menuWeapon?.addItemBox(2, invenWeapItemData);
                    invenControll.changeItemIntoWeapSlot(weapPrevItem, key);
                }
            }
            else {
                //������ ���
                invenDrop.dropItemAll(key);
            }
        }
    }
}
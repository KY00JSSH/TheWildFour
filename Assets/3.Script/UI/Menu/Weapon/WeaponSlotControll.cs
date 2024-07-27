using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponSlotControll : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {

    [SerializeField]
    public Image itemIcon;

    [SerializeField]
    public Image cursorIcon;

    public int key;

    private GameObject currentItem;
    public GameObject CurrentItem { get { return currentItem; } }

    private MenuWeapon menuWeapon;
    private InvenUIController invenUI;

    private Canvas canvas;
    private RectTransform originalParent;
    private Vector2 originalPosition;

    private PlayerAttack playerAttack;

    private GameObject player;

    private void Awake() {
        invenUI = FindObjectOfType<InvenUIController>();
        menuWeapon = FindObjectOfType<MenuWeapon>();
        canvas = FindObjectOfType<Canvas>(); 
        playerAttack = FindObjectOfType<PlayerAttack>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void enableCursor() {
        cursorIcon.gameObject.SetActive(true);
    }

    public void disableCursor() {
        cursorIcon.gameObject.SetActive(false);
    }

    //���Կ� ���� �߰�
    public void setWeaponSlot(GameObject item = null) {
        if (item != null) {
            currentItem = item;
            itemIcon.sprite = currentItem.GetComponent<Item>().itemData.Icon;
            itemIcon.enabled = true;
            //������ �����̴� �߰�
        }
        else {
            currentItem = null;
            itemIcon.sprite = null;
            itemIcon.enabled = false;
        }
    }

    public GameObject returnItem() {
        if (currentItem) {
            return currentItem;
        }
        else {
            return null;
        }
    }

    public void OnPointerClick(PointerEventData pointerEventData) {
        if (PlayerStatus.isDead) return;

        //Ŭ�������� ���â slot key ����
        menuWeapon.setCurrSelectSlot(key);
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if (PlayerStatus.isDead) return;

        if (currentItem) {
            originalParent = itemIcon.rectTransform.parent as RectTransform;
            originalPosition = itemIcon.rectTransform.anchoredPosition;
            itemIcon.transform.SetParent(canvas.transform, true);
            itemIcon.transform.SetAsLastSibling();
        }
    }

    public void OnDrag(PointerEventData data) {
        playerAttack.isNowDrag = true;

        if (PlayerStatus.isDead) return;

        if (currentItem) {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, data.position, data.pressEventCamera, out position);
            itemIcon.rectTransform.anchoredPosition = position;
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        playerAttack.isNowDrag = false;

        if (PlayerStatus.isDead) return;

        if (currentItem) {
            itemIcon.transform.SetParent(originalParent, true);
            itemIcon.rectTransform.anchoredPosition = originalPosition;

            bool isChangeSlot;
            int targetIndex = 99;
            bool isInvenAdd = false;

            //�κ�â position�� �巡�� �ߴ��� üũ
            for (int i = 0; i < invenUI.InvenTotalList.Count; i++) {
                RectTransform boxRectTransform = invenUI.InvenTotalList[i].GetComponent<RectTransform>();
                if (RectTransformUtility.RectangleContainsScreenPoint(boxRectTransform, eventData.position, eventData.pressEventCamera)) {
                    isInvenAdd = true;
                    targetIndex = i;
                    break;
                }
            }

            if (key == 1) {
                if (RectTransformUtility.RectangleContainsScreenPoint(menuWeapon.WeapSecondBoxPos, eventData.position, eventData.pressEventCamera)) {
                    //���� inven�� 1�� + �巡���� �κ��� 2�� �������̸� 1��-2�� ���� ����Ī
                    isChangeSlot = true;
                }
                else {
                    isChangeSlot = false;
                }
            }
            else {
                if (RectTransformUtility.RectangleContainsScreenPoint(menuWeapon.WeapFirstBoxPos, eventData.position, eventData.pressEventCamera)) {
                    isChangeSlot = true;
                }
                else {
                    isChangeSlot = false;
                }
            }

            if (isChangeSlot) {
                //��� ���� ��ġ ����
                menuWeapon.switchingSlot(key);
            }
            else if (isInvenAdd) {
                //���â�� �߰�
                //���â�� ������ ���� ����� ����Ī
                menuWeapon.addToInventory(key, targetIndex);
            }
            else {
                //������ ���
                DropItem();
            }
            FindObjectOfType<PlayerWeaponEquip>().ChangeEquipWeapon();
        }
    }

    public void DropItem() {
        if (PlayerStatus.isDead) return;

        Vector3 itemDropPosition = new Vector3(player.transform.position.x - 0.1f, player.transform.position.y + 1.5f, player.transform.position.z - 0.1f);
        currentItem.transform.position = itemDropPosition;
        currentItem.SetActive(true);
        //Instantiate(currentItem, itemDropPosition, Quaternion.identity);
        setWeaponSlot(null);
    }
}

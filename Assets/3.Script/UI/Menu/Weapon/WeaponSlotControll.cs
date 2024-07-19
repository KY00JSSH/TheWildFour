using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponSlotControll : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {

    [SerializeField]
    public Image itemIcon;

    public int key;

    private Item currentItem;
    public Item CurrentItem { get { return currentItem; } }

    private MenuWeapon menuWeapon;
    private InvenUIController invenUI;

    private Canvas canvas;
    private RectTransform originalParent;
    private Vector2 originalPosition;

    [SerializeField]
    private GameObject player;

    private void Awake() {
        invenUI = FindObjectOfType<InvenUIController>();
        menuWeapon = FindObjectOfType<MenuWeapon>();
        canvas = FindObjectOfType<Canvas>();
    }

    public void setWeaponSlot(WeaponItemData item = null) {
        if (item != null) {
            WeaponItem newItem = new WeaponItem();
            newItem.WeaponItemData = item;
            newItem.equipItemData = item;
            newItem.itemData = item;
            currentItem = newItem;
            itemIcon.sprite = currentItem.itemData.Icon;
            itemIcon.enabled = true;
            //내구도 슬라이더 추가
        }
        else {
            currentItem = null;
            itemIcon.sprite = null;
            itemIcon.enabled = false;
        }
    }

    public WeaponItemData returnItem() {
        if (currentItem?.itemData is WeaponItemData weapItem) {
            return weapItem;
        }
        else {
            return null;
        }
    }

    public void OnPointerClick(PointerEventData pointerEventData) {
        //클릭했을때 장비창 slot key 선택
        menuWeapon.setCurrSelectSlot(key);
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if (currentItem is WeaponItem) {
            originalParent = itemIcon.rectTransform.parent as RectTransform;
            originalPosition = itemIcon.rectTransform.anchoredPosition;
            itemIcon.transform.SetParent(canvas.transform, true);
            itemIcon.transform.SetAsLastSibling();
        }
    }

    public void OnDrag(PointerEventData data) {
        if (currentItem is WeaponItem) {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, data.position, data.pressEventCamera, out position);
            itemIcon.rectTransform.anchoredPosition = position;
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (currentItem is WeaponItem) {
            itemIcon.transform.SetParent(originalParent, true);
            itemIcon.rectTransform.anchoredPosition = originalPosition;

            bool isChangeSlot;
            int targetIndex = 99;
            bool isInvenAdd = false;

            //인벤창 position에 드래그 했는지 체크
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
                    //현재 inven이 1번 + 드래그한 인벤이 2번 포지션이면 1번-2번 무기 스위칭
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
                //장비 슬롯 위치 변경
                menuWeapon.switchingSlot(key);
            }
            else if (isInvenAdd) {
                //장비창에 추가
                //장비창에 있으면 현재 무기와 스위칭
                menuWeapon.addToInventory(key, targetIndex);
            }
            else {
                //아이템 드랍
                DropItem();
            }
        }
    }

    public void DropItem() {
        Vector3 itemDropPosition = new Vector3(player.transform.position.x - 0.1f, player.transform.position.y + 1.5f, player.transform.position.z - 0.1f);
        Instantiate(currentItem.itemData.DropItemPrefab, itemDropPosition, Quaternion.identity);
        setWeaponSlot(null);
    }
}

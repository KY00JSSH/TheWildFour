using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryBox : CommonInvenBox, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {

    private PlayerItemUseControll playerItemUse;
    private InvenDrop invenDrop;
    private InvenController invenControll;
    private InvenUIController invenUI;

    private MenuWeapon menuWeapon;                  //장비창
    private ShelterInvenUI shelterInvenUI;          //거처

    private Canvas canvas;
    private RectTransform originalParent;
    private Vector2 originalPosition;

    private void Awake() {
        invenBox = transform.GetComponent<Button>();
        playerItemUse = FindObjectOfType<PlayerItemUseControll>();
        invenControll = FindObjectOfType<InvenController>();
        shelterInvenUI = FindObjectOfType<ShelterInvenUI>();
        menuWeapon = FindObjectOfType<MenuWeapon>();
        invenUI = FindObjectOfType<InvenUIController>();
        invenDrop = FindObjectOfType<InvenDrop>();
        canvas = FindObjectOfType<Canvas>();
    }

    //TODO: 꾹 누르는 게이지 추가하기
    //TODO: 제련 돌 30 -> 철광석 1개

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
                //인벤 위치 변경
                //원래 인벤 index 랑 바꿀 인벤 체크
                invenControll.changeInvenIndex(key, targetIndex);
            }
            else {
                bool isWorkshopOpen = WorkShopUI.isWorkshopUIOpen;
                bool isShelterOpen = ShelterUI.isShelterUIOpen;

                if (!isShelterOpen && !isWorkshopOpen) {    //거처, 작업장 오픈 안됬을때
                    if (RectTransformUtility.RectangleContainsScreenPoint(menuWeapon.WeapFirstBoxPos, eventData.position, eventData.pressEventCamera)) {
                        //무기 1번 슬롯일때
                        WeaponItemData invenWeapItemData = invenControll.getIndexItem(key);
                        if (invenWeapItemData) {
                            WeaponItemData weapPrevItem = menuWeapon?.addItemBox(1, invenWeapItemData);
                            invenControll.changeItemIntoWeapSlot(weapPrevItem, key);
                        }
                    }
                    else if (RectTransformUtility.RectangleContainsScreenPoint(menuWeapon.WeapSecondBoxPos, eventData.position, eventData.pressEventCamera)) {
                        //무기 2번 슬롯일떄
                        WeaponItemData invenWeapItemData = invenControll.getIndexItem(key);
                        if (invenWeapItemData) {
                            WeaponItemData weapPrevItem = menuWeapon?.addItemBox(2, invenWeapItemData);
                            invenControll.changeItemIntoWeapSlot(weapPrevItem, key);
                        }
                    }
                }
                else if (isWorkshopOpen) {  //작업장 오픈시
                    for (int i = 0; i < invenUI.InvenTotalList.Count; i++) {
                        RectTransform boxRectTransform = shelterInvenUI.InvenTotalList[i].GetComponent<RectTransform>();
                        if (RectTransformUtility.RectangleContainsScreenPoint(boxRectTransform, eventData.position, eventData.pressEventCamera)) {
                            targetIndex = i;
                            break;
                        }
                    }
                    //작업장 인벤에 아이템 추가

                }
                else if (isShelterOpen) {   //거처 오픈
                    for (int i = 0; i < invenUI.InvenTotalList.Count; i++) {
                        RectTransform boxRectTransform = shelterInvenUI.InvenTotalList[i].GetComponent<RectTransform>();
                        if (RectTransformUtility.RectangleContainsScreenPoint(boxRectTransform, eventData.position, eventData.pressEventCamera)) {
                            targetIndex = i;
                            break;
                        }
                    }
                    //거처 인벤에 아이템 추가

                }
                else {
                    //아이템 드랍
                    invenDrop.dropItemAll(key);
                }
            }
        }
    }
}
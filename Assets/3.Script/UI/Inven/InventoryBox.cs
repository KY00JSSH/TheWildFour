using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryBox : CommonInvenBox, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {

    private PlayerItemUseControll playerItemUse;
    private InvenDrop invenDrop;
    private InvenController invenControll;
    private InvenUIController invenUI;

    private MenuWeapon menuWeapon;                  //장비창

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

        playerItemUse = FindObjectOfType<PlayerItemUseControll>();
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
                        GameObject invenWeapItem = invenControll.getIndexItem(key);
                        if (invenWeapItem.GetComponent<WeaponItem>() != null) {
                            GameObject weapPrevItem = menuWeapon?.addItemBox(1, invenWeapItem);
                            invenControll.changeItemIntoWeapSlot(key, weapPrevItem);
                        }
                        invenControll.updateInvenInvoke();
                    }
                    else if (RectTransformUtility.RectangleContainsScreenPoint(menuWeapon.WeapSecondBoxPos, eventData.position, eventData.pressEventCamera)) {
                        //무기 2번 슬롯일떄
                        GameObject invenWeapItem = invenControll.getIndexItem(key);
                        if (invenWeapItem.GetComponent<WeaponItem>() != null) {
                            GameObject weapPrevItem = menuWeapon?.addItemBox(2, invenWeapItem);
                            invenControll.changeItemIntoWeapSlot(key, weapPrevItem);
                        }
                        invenControll.updateInvenInvoke();
                    }
                    else {
                        //아이템 드랍
                        invenDrop.dropItemAll(key);
                    }
                }
                else if (isWorkshopOpen) {  //작업장 오픈시
                    WorkshopInvenControll workshopInven = FindObjectOfType<WorkshopInvenControll>();
                    WorkshopInvenUI workshopInvenUI = FindObjectOfType<WorkshopInvenUI>();

                    for (int i = 0; i < workshopInvenUI.CurrInvenCount; i++) {
                        RectTransform boxRectTransform = workshopInvenUI.InvenTotalList[i].GetComponent<RectTransform>();
                        if (RectTransformUtility.RectangleContainsScreenPoint(boxRectTransform, eventData.position, eventData.pressEventCamera)) {
                            targetIndex = i;
                            break;
                        }
                    }
                    //작업장 인벤에 아이템 추가
                    if (workshopInven.checkItemType(targetIndex) != 0) {
                        //해당 위치에 아이템 있으면 스위칭
                        invenControll.switchingInvenItem(targetIndex, true);
                    }
                    else {
                        //없으면 아이템 추가만
                        invenControll.addItemBuildInven(targetIndex, true);
                        invenControll.removeItem(key);
                    }

                }
                else if (isShelterOpen) {   //거처 오픈
                    ShelterInvenControll shelterInven = FindObjectOfType<ShelterInvenControll>();   
                    ShelterInvenUI shelterInvenUI = FindObjectOfType<ShelterInvenUI>();

                    for (int i = 0; i < shelterInvenUI.CurrInvenCount; i++) {
                        RectTransform boxRectTransform = shelterInvenUI.InvenTotalList[i].GetComponent<RectTransform>();
                        if (RectTransformUtility.RectangleContainsScreenPoint(boxRectTransform, eventData.position, eventData.pressEventCamera)) {
                            targetIndex = i;
                            break;
                        }
                    }
                    //거처 인벤에 아이템 추가
                    if (shelterInven.checkItemType(targetIndex) != 0) {
                        //해당 위치에 아이템 있으면 스위칭
                        invenControll.switchingInvenItem(targetIndex, false);
                    }
                    else {
                        //없으면 아이템 추가만
                        invenControll.addItemBuildInven(targetIndex, false);
                        invenControll.removeItem(key);
                    }
                }
                
            }
        }
    }


}
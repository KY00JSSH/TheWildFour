using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class InventoryBox : CommonInvenBox, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {
    private PlayerItemUseControll playerItemUse;
    private InvenDrop invenDrop;
    private InvenController invenControll;
    private InvenUIController invenUI;

    private MenuWeapon menuWeapon;                  //���â

    private Canvas canvas;
    private RectTransform originalParent;
    private Vector2 originalPosition;

    private PlayerAttack playerAttack;

    private void Awake() {
        invenBox = transform.GetComponent<Button>();
        playerItemUse = FindObjectOfType<PlayerItemUseControll>();
        invenControll = FindObjectOfType<InvenController>();
        menuWeapon = FindObjectOfType<MenuWeapon>();
        invenUI = FindObjectOfType<InvenUIController>();
        invenDrop = FindObjectOfType<InvenDrop>();
        canvas = FindObjectOfType<Canvas>();

        playerItemUse = FindObjectOfType<PlayerItemUseControll>();
        playerAttack = FindObjectOfType<PlayerAttack>();
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
        playerAttack.isNowDrag = true;
        if (isItemIn) {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, data.position, data.pressEventCamera, out position);
            itemIcon.rectTransform.anchoredPosition = position;
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        playerAttack.isNowDrag = false;
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
            else {
                bool isWorkshopOpen = WorkShopUI.isWorkshopUIOpen;
                bool isShelterOpen = ShelterUI.isShelterUIOpen;

                if (!isShelterOpen && !isWorkshopOpen) {    //��ó, �۾��� ���� �ȉ�����
                    if (RectTransformUtility.RectangleContainsScreenPoint(menuWeapon.WeapFirstBoxPos, eventData.position, eventData.pressEventCamera)) {
                        //���� 1�� �����϶�
                        GameObject invenWeapItem = invenControll.getIndexItem(key);
                        if (invenWeapItem.GetComponent<WeaponItem>() != null) {
                            GameObject weapPrevItem = menuWeapon?.addItemBox(1, invenWeapItem);
                            invenControll.changeItemIntoWeapSlot(key, weapPrevItem);
                        }
                        invenControll.updateInvenInvoke();
                        FindObjectOfType<PlayerWeaponEquip>().ChangeEquipWeapon();
                    }
                    else if (RectTransformUtility.RectangleContainsScreenPoint(menuWeapon.WeapSecondBoxPos, eventData.position, eventData.pressEventCamera)) {
                        //���� 2�� �����ϋ�
                        GameObject invenWeapItem = invenControll.getIndexItem(key);
                        if (invenWeapItem.GetComponent<WeaponItem>() != null) {
                            GameObject weapPrevItem = menuWeapon?.addItemBox(2, invenWeapItem);
                            invenControll.changeItemIntoWeapSlot(key, weapPrevItem);
                        }
                        invenControll.updateInvenInvoke();
                        FindObjectOfType<PlayerWeaponEquip>().ChangeEquipWeapon();
                    }
                    else {
                        //������ ���
                        invenDrop.dropItemAll(key);
                    }
                }
                else if (isWorkshopOpen) {  //�۾��� ���½�
                    WorkshopInvenControll workshopInven = FindObjectOfType<WorkshopInvenControll>();
                    WorkshopInvenUI workshopInvenUI = FindObjectOfType<WorkshopInvenUI>();

                    for (int i = 0; i < workshopInvenUI.CurrInvenCount; i++) {
                        RectTransform boxRectTransform = workshopInvenUI.InvenTotalList[i].GetComponent<RectTransform>();
                        if (RectTransformUtility.RectangleContainsScreenPoint(boxRectTransform, eventData.position, eventData.pressEventCamera)) {
                            targetIndex = i;
                            break;
                        }
                    }
                    //�۾��� �κ��� ������ �߰�
                    if (workshopInven.checkItemType(targetIndex) != 0) {
                        //�ش� ��ġ�� ������ ������ ����Ī
                        GameObject item = workshopInven.Inventory[targetIndex];
                        workshopInven.addIndexItem(targetIndex, invenControll.getIndexItem(playerItemUse.selectBoxKey));
                        invenControll.addIndexItem(targetIndex, item);
                    }
                    else {
                        //������ ������ �߰���
                        workshopInven.addIndexItem(targetIndex, invenControll.getIndexItem(playerItemUse.selectBoxKey));
                        invenControll.removeItem(key);
                    }
                    workshopInven.printInven();
                }
                else if (isShelterOpen) {   //��ó ����
                    ShelterInvenControll shelterInven = FindObjectOfType<ShelterInvenControll>();
                    ShelterInvenUI shelterInvenUI = FindObjectOfType<ShelterInvenUI>();

                    for (int i = 0; i < shelterInvenUI.CurrInvenCount; i++) {
                        RectTransform boxRectTransform = shelterInvenUI.InvenTotalList[i].GetComponent<RectTransform>();
                        if (RectTransformUtility.RectangleContainsScreenPoint(boxRectTransform, eventData.position, eventData.pressEventCamera)) {
                            targetIndex = i;
                            break;
                        }
                    }
                    //��ó �κ��� ������ �߰�
                    if (shelterInven.checkItemType(targetIndex) != 0) {
                        GameObject item = shelterInven.Inventory[targetIndex];
                        shelterInven.addIndexItem(targetIndex, invenControll.getIndexItem(playerItemUse.selectBoxKey));
                        invenControll.addIndexItem(targetIndex, item);
                    }
                    else {
                        //������ ������ �߰���
                        shelterInven.addIndexItem(targetIndex, invenControll.getIndexItem(playerItemUse.selectBoxKey));
                        invenControll.removeItem(key);
                    }
                }
            }
        }
    }
}
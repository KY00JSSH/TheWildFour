using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryBox : CommonInvenBox, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {

    private PlayerItemUseControll playerItemUse;
    private InvenDrop invenDrop;
    private InvenController invenControll;
    private InvenUIController invenUI;

    private MenuWeapon menuWeapon;                  //���â

    private WorkshopInvenUI workshopInvenUI;        //�۾���
    private WorkshopInvenControll workshopInven;

    private ShelterInvenUI shelterInvenUI;          //��ó
    private ShelterInvenControll shelterInven;

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

        shelterInvenUI = FindObjectOfType<ShelterInvenUI>();
        shelterInven = FindObjectOfType<ShelterInvenControll>();
        workshopInvenUI = FindObjectOfType<WorkshopInvenUI>();
        playerItemUse = FindObjectOfType<PlayerItemUseControll>();
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
            else {
                bool isWorkshopOpen = WorkShopUI.isWorkshopUIOpen;
                bool isShelterOpen = ShelterUI.isShelterUIOpen;

                if (!isShelterOpen && !isWorkshopOpen) {    //��ó, �۾��� ���� �ȉ�����
                    if (RectTransformUtility.RectangleContainsScreenPoint(menuWeapon.WeapFirstBoxPos, eventData.position, eventData.pressEventCamera)) {
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
                }
                else if (isWorkshopOpen) {  //�۾��� ���½�
                    for (int i = 0; i < invenUI.InvenTotalList.Count; i++) {
                        RectTransform boxRectTransform = workshopInvenUI.InvenTotalList[i].GetComponent<RectTransform>();
                        if (RectTransformUtility.RectangleContainsScreenPoint(boxRectTransform, eventData.position, eventData.pressEventCamera)) {
                            targetIndex = i;
                            break;
                        }
                    }
                    //�۾��� �κ��� ������ �߰�
                    if (workshopInven.checkItemType(targetIndex) != 0) {
                        //�ش� ��ġ�� ������ ������ ����Ī
                        invenControll.switchingInvenItem(targetIndex, true);
                    }
                    else {
                        //������ ������ �߰���
                        invenControll.addItemBuildInven(targetIndex, true);
                        invenControll.removeItem(key);
                    }

                }
                else if (isShelterOpen) {   //��ó ����
                    for (int i = 0; i < invenUI.InvenTotalList.Count; i++) {
                        RectTransform boxRectTransform = shelterInvenUI.InvenTotalList[i].GetComponent<RectTransform>();
                        if (RectTransformUtility.RectangleContainsScreenPoint(boxRectTransform, eventData.position, eventData.pressEventCamera)) {
                            targetIndex = i;
                            break;
                        }
                    }
                    //��ó �κ��� ������ �߰�
                    if (shelterInven.checkItemType(targetIndex) != 0) {
                        //�ش� ��ġ�� ������ ������ ����Ī
                        invenControll.switchingInvenItem(targetIndex, false);
                    }
                    else {
                        //������ ������ �߰���
                        invenControll.addItemBuildInven(targetIndex, false);
                        invenControll.removeItem(key);
                    }
                }
                else {
                    //������ ���
                    invenDrop.dropItemAll(key);
                }
            }
        }
    }


}
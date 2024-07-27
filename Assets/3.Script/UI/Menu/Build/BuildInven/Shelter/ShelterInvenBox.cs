using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShelterInvenBox : CommonInvenBox, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {

    private Canvas canvas;
    private RectTransform originalParent;
    private Vector2 originalPosition;

    private InvenUIController invenUI;
    private InvenController invenControll;

    private PlayerItemUseControll playerItemUse;

    private ShelterInvenControll shelterInvenCont;
    private ShelterInvenUI shelterInvenUI;

    private void Awake() {
        invenBox = transform.GetComponent<Button>();
        invenUI = FindObjectOfType<InvenUIController>();
        invenControll = FindObjectOfType<InvenController>();
        playerItemUse = FindObjectOfType<PlayerItemUseControll>();
        canvas = FindObjectOfType<Canvas>();
        shelterInvenCont = FindObjectOfType<ShelterInvenControll>();
        shelterInvenUI = FindObjectOfType<ShelterInvenUI>();
    }

    public void OnPointerClick(PointerEventData pointerEventData) {
        shelterInvenCont.setCurrSelectSlot(key);
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if (isItemIn) {
            shelterInvenCont.setCurrSelectSlot(key);
            originalParent = itemIcon.rectTransform.parent as RectTransform;
            originalPosition = itemIcon.rectTransform.anchoredPosition;
            itemIcon.transform.SetParent(canvas.transform, true);
            itemIcon.transform.SetAsLastSibling();
        }
    }

    public void OnDrag(PointerEventData eventData) {
        if (isItemIn) {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out position);
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
                if (invenControll.checkItemType(targetIndex) != 0) {
                    //해당 인벤에 아이템 있으면 스위칭
                    shelterInvenCont.switchingInvenItem(targetIndex);
                }
                else {
                    //없으면 현재 아이템박스 비우고 인벤 해당 위치에 추가
                    shelterInvenCont.addItemPlayerInven(targetIndex);
                    shelterInvenCont.removeItem(key);
                }
            }
            else {
                for (int i = 0; i < shelterInvenUI.CurrInvenCount; i++) {
                    RectTransform boxRectTransform = shelterInvenUI.InvenTotalList[i].GetComponent<RectTransform>();
                    if (RectTransformUtility.RectangleContainsScreenPoint(boxRectTransform, eventData.position, eventData.pressEventCamera)) {
                        targetIndex = i;
                        break;
                    }
                }

                if (targetIndex != 99) {
                    shelterInvenCont.changeInvenIndex(key, targetIndex);
                }
                else {
                    return;
                }
            }
        }
    }
}
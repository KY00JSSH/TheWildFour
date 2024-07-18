using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tooltip_Workshop : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    /*
     1.CurrentupgradeDetail�� WorkshopManager���� Ȯ��
     2. ������
     */

    private WorkShopUI workShopUI;
    [SerializeField] private GameObject WorkshopTooltip;
    private void Awake() {
        workShopUI = GetComponent<WorkShopUI>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (eventData.pointerEnter != null) {
            Button btn = eventData.pointerEnter.GetComponent<Button>();
            if (btn != null && !btn.name.Contains("Exit")) {
                // ��ư -> content�ȿ� �ִ� ��ư or function ��ư
                if (eventData.position.y >= 520) {
                    WorkshopTooltip.SetActive(true);


                }
                else {

                }



            }
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (eventData.pointerCurrentRaycast.gameObject != null) {
            Button btn = eventData.pointerCurrentRaycast.gameObject.GetComponent<Button>();
            if (btn != null) {
                WorkshopTooltip.SetActive(false);
            }
        }
    }
}

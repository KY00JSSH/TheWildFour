using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipNum : MonoBehaviour {
    /*
     1. 버튼 정보 (Dictionary num) 
     2. 인벤토리 필요한 아이템 키 확인 => 아이템 합산
        - 아이템 키별로 int값 저장할 수 있게 
     2. 아이템 개수 비교
     
     */

    private TooltipDetail tooltipDetail;
    private InvenController invenController;

    private void Awake() {
        tooltipDetail = GetComponent<TooltipDetail>();
        invenController = FindObjectOfType<InvenController>();
    }

    // 
    public void BuildItemCheck(int DictionaryKey) {

        foreach (BuildDetail each in tooltipDetail.buildDetailList.buildDetails) {
            if (each.buttonNum == DictionaryKey) {
                //
                InvenItemGet(each.needItems);
            }
        }

    }

    private void InvenItemGet(NeedItem[] items) {

    }

}

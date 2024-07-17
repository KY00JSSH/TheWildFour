using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipNum : MonoBehaviour {
    /*
     1. ��ư ���� (Dictionary num) 
     2. �κ��丮 �ʿ��� ������ Ű Ȯ�� => ������ �ջ�
        - ������ Ű���� int�� ������ �� �ְ� 
     2. ������ ���� ��
     
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

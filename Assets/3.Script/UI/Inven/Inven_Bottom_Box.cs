using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inven_Bottom_Box : Inven_Bottom_Controll {

    /*
     Inven_Bottom_Controll ���� ���� ������ ����    
     
     */

    // �κ� �ڽ� -> ��ư
    private Button Inven_Box;
    [SerializeField] private Text Inven_Text;

    private int Item_count = 0;

    private int Click_count = 0;
    private bool isMouseClick = false;

    // �������� ����ϴ��� Ȯ��
    public bool isItemUse = false;
    // �������� ���ִ��� Ȯ��
    public bool isItemIn = false;

    private InvenController invenCon;

    private void Awake() {
        invenCon = GetComponent<InvenController>();
        Inven_Box = transform.GetComponent<Button>();
        Inven_Text = transform.GetChild(0).GetComponent<Text>();
        Inven_Text.text = Item_count.ToString();
    }

    // item box Ŭ���� ���� Ŭ���� box index�� item ������ ã��
    // 
    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            isMouseClick = true;
            if (!IsCusorOutButton(Inven_Box)) {
                Click_count = 0;
                isMouseClick = false;
            }

        }
        if (isMouseClick) {
            if (isItemIn && Input.GetKeyDown(KeyCode.E)) {
                //invenCon.useItem();
            }
        }
    }

    //TODO: �� ������ ������ �߰��ϱ�
    //TODO: �� �����ų� �巡�� - �ٶ�����
    //TODO: �׳� f ������ ����, ���� 8���� ������ 1����
    //TODO: ���� �� 30 -> ö���� 1��

    //public void ItemUse_Button() {
    //    Click_count++;
    //    if (Click_count >= 2 && isItemIn) {
    //        ItemUse();
    //    }
    //}

    //// �ʱ�ȭ Ȥ�� ������
    //private void Init_InvenBox() {
    //    Item_count = 0;
    //    isItemUse = false;
    //    isItemIn = false;
    //    isInvenBoxAvailable = true;
    //    Inven_Item = null;
    //    //itemImgOj = transform.GetComponent<Image>().sprite;
    //    Inven_Sprite.enabled = false;
    //}


    private bool IsCusorOutButton(Button button) {
        Vector2 MCursorPosition = button.GetComponent<RectTransform>().InverseTransformPoint(Input.mousePosition);
        return button.GetComponent<RectTransform>().rect.Contains(MCursorPosition);
    }
}
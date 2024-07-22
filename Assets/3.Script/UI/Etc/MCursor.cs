using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MCursor : MonoBehaviour {


    public RectTransform transform_cursor;
    public RectTransform transform_icon;    

    // default mouse sprite
    private Sprite defaultM;
    public Sprite[] MouseCursorOff;
    public Sprite[] MouseCursorOn;

    // ������ �� ��������Ʈ ǥ��
    public Sprite[] spaceEnter;
    public Text spaceText;

    // ����� ������ ĵ���� ��ġ
    private Vector2 itemCanvasPosition;
    // ����� �����۰� ���콺 �Ÿ�
    [SerializeField] private float distance;

    // ���콺 ��������Ʈ ���濩��
    private bool isChangeSprite;

    // �������� ǥ�õ� ��ġ ������
    private float iconDistance;

    private void Awake() {
        defaultM = transform_cursor.gameObject.GetComponent<Image>().sprite;
        distance = 100f;
        iconDistance = 50f;
    }

    private void Start() {
        Init_Cursor();
    }

    private void Update() {
        Update_MousePosition();
        if (CheckCol()) {
            // �Ÿ� ���
            ItemPosition();
            // ���̾ �Ÿ� ��� -> ��������Ʈ ���� ���� Ȯ��
            ChangeSprite();
            if (isChangeSprite) {
                ChangeSprite();
                IconPosition(iconDistance);
            }
            else {
                // �־����� default�� ���� + ������ ���ֱ�
                DefaultMouseSprite();
                IconPositionOff();
            }

        }
        else {
            DefaultMouseSprite();
        }
    }

    private void Init_Cursor() {
        Cursor.visible = false;
        transform_cursor.pivot = Vector2.up;
        if (transform_cursor.GetComponent<Graphic>())
            transform_cursor.GetComponent<Graphic>().raycastTarget = false;
        if (transform_icon.GetComponent<Graphic>())
            transform_icon.GetComponent<Graphic>().raycastTarget = false;
    }



    // Ŀ�� ��ġ ����
    private void Update_MousePosition() {
        Vector2 mousePos = Input.mousePosition;
        transform_cursor.position = mousePos;
    }

    // ����� ������ ��ũ������ ��ǥ�� ����
    private void ItemPosition() {
        if(PlayerItemPickControll.ClosestItem.transform.TryGetComponent(out Collider col)) {
            // �ݶ��̴��� ���� ��� �ش� �ݶ��̴��� �߽��� �������� ��ġ ����
            Vector3 vector3 = col.bounds.center;
            itemCanvasPosition = Camera.main.WorldToScreenPoint(vector3);
        }
        else {
            // ����...�ݶ��̴��� ���� ���� �ֳ�?
            // Convert world position to screen position
            itemCanvasPosition = Camera.main.WorldToScreenPoint(PlayerItemPickControll.ClosestItem.transform.position);
        }        
    }

    // Ŀ���� ������ �־����� ��� ������ defualt�� ����
    private void CursorDistanceCheck(float distance) {
        float _distance = Vector3.Distance(itemCanvasPosition, transform_cursor.position);
        if (_distance >= distance) isChangeSprite = false; // �־����� false
        else isChangeSprite = true;
    }


    // ����� ������ ������ 
    private void IconPosition(float iconDistance) {
        transform_icon.gameObject.SetActive(true);
        spaceText.gameObject.SetActive(true);
        transform_icon.position = new Vector2(itemCanvasPosition.x, itemCanvasPosition.y + iconDistance);

        string itemName = PlayerItemPickControll.ClosestItem.name;
        if (itemName.Contains("(Clone)"))
            itemName = itemName.Replace("(Clone)", "");
        if (itemName.Contains("0")) {
            itemName = itemName.Split('0')[0];
        }

        spaceText.text = itemName;
    }
    // ������ ������ off
    private void IconPositionOff() {
        transform_icon.gameObject.SetActive(false);
        spaceText.gameObject.SetActive(false);
    }


    private void DefaultMouseSprite() {
        transform_cursor.GetComponent<Image>().sprite = defaultM;
    }

    private bool CheckCol() {
        if (PlayerItemPickControll.ClosestItem != null) return true;
        else return false;
    }

    // ���̾ ������ ����
    private void ChangeSprite() {
        switch (PlayerItemPickControll.ClosestItem.layer) {
            case 8: // ������ ���콺 �Ÿ� 70f
                transform_cursor.GetComponent<Image>().sprite = MouseCursorOn[2];
                CursorDistanceCheck(70f);
                iconDistance = 50f;
                break;
            case 9:// ���๰ ����Ÿ� 500f
                transform_cursor.GetComponent<Image>().sprite = MouseCursorOn[6];
                CursorDistanceCheck(500f);
                //TODO: ���๰ �� ������ ��ġ �ȸ���
                IconPositionOff();
                iconDistance = -10f;
                break;
            default:
                isChangeSprite = false;
                break;
        }
    }


}

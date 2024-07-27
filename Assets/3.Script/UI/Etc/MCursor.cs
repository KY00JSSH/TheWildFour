using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MCursor : MonoBehaviour {


    public RectTransform transform_cursor;
    public RectTransform transform_icon;    

    // default mouse sprite
    private Sprite defaultM;
    public Sprite[] MouseCursorOn;

    // 검출대상 위 스프라이트 표시
    public Sprite[] spaceEnter;
    public Text spaceText;

    // 검출된 아이템 캔버스 위치
    private Vector2 itemCanvasPosition;

    // 마우스 스프라이트 변경여부
    private bool isChangeSprite;

    // 아이콘이 표시될 위치 보정값
    private float iconDistance;


    private void Awake() {
        defaultM = transform_cursor.gameObject.GetComponent<Image>().sprite;
        iconDistance = 50f;
    }

    private void Start() {
        Init_Cursor();
    }

    private void Update() {
        Update_MousePosition();
        // 각 Build UI Open 되면 기본값, 스페이스 아이콘 off
        if (WorkShopUI.isWorkshopUIOpen || ShelterUI.isShelterUIOpen) {
            DefaultMouseSprite();
            IconPositionOff();
        }
        else {
            if (CheckCol()) {
                // 거리 계산
                ItemPosition();
                // 레이어별 거리 계산 -> 스프라이트 변경 여부 확인
                ChangeCursorSprite();
                if (isChangeSprite) {
                    ChangeCursorSprite();
                    IconPosition(iconDistance);
                }
                else {
                    // 멀어지면 default로 변경 + 아이콘 없애기
                    DefaultMouseSprite();
                    IconPositionOff();
                }

            }
            else {
                IconPositionOff();
                DefaultMouseSprite();
            }
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



    // 커서 위치 변경
    private void Update_MousePosition() {
        Vector2 mousePos = Input.mousePosition;
        transform_cursor.position = mousePos;
    }

    // 검출된 아이템 스크린상의 좌표로 변경
    private void ItemPosition() {
        if(PlayerItemPickControll.ClosestItem.transform.TryGetComponent(out Collider col)) {
            // 콜라이더가 있을 경우 해당 콜라이더의 중심을 기준으로 위치 변경
            Vector3 vector3 = col.bounds.center;
            itemCanvasPosition = Camera.main.WorldToScreenPoint(vector3);
        }
        else {
            // 몰까...콜라이더가 없을 수도 있나?
            // Convert world position to screen position
            itemCanvasPosition = Camera.main.WorldToScreenPoint(PlayerItemPickControll.ClosestItem.transform.position);
        }        
    }

    // 커서와 아이템 멀어졌을 경우 아이콘 defualt값 변경
    private void CursorDistanceCheck(float distance) {
        float _distance = Vector3.Distance(itemCanvasPosition, transform_cursor.position);
        if (_distance >= distance) isChangeSprite = false; // 멀어지면 false
        else isChangeSprite = true;
    }


    // 검출된 아이템 아이콘 
    private void IconPosition(float iconDistance) {
        transform_icon.gameObject.SetActive(true);
        spaceText.gameObject.SetActive(true);
        transform_icon.position = new Vector2(itemCanvasPosition.x, itemCanvasPosition.y + iconDistance);

        string itemName = PlayerItemPickControll.ClosestItem.name;
        if (itemName.Contains("(Clone)")) itemName = itemName.Replace("(Clone)", "");
        if (itemName.Contains("0")) itemName = itemName.Split('0')[0];
        if (itemName.Contains("Prf")) itemName = itemName.Replace("Prf", "");

        spaceText.text = itemName;
    }
    // 아이템 아이콘 off
    private void IconPositionOff() {
        transform_icon.gameObject.SetActive(false);
        spaceText.gameObject.SetActive(false);
    }


    private void DefaultMouseSprite() {
        transform_cursor.GetComponent<Image>().sprite = defaultM;
    }

    private bool CheckCol() {
        if (PlayerItemPickControll.ClosestItem != null) {
            if (PlayerItemPickControll.ClosestItem.activeSelf) return true;
            else return false;
        }
        else return false;
    }

    //TODO: 10~12번 검출거리 변경해야함
    // 레이어별 아이콘 변경
    private void ChangeCursorSprite() {
        switch (PlayerItemPickControll.ClosestItem.layer) {
            case 8: // 아이템 마우스 거리 70f
                transform_cursor.GetComponent<Image>().sprite = MouseCursorOn[1];
                CursorDistanceCheck(70f);
                iconDistance = 50f;
                break;
            case 9:// 건축물 마우스 거리 500f
                FindBuildType();
                break;
            case 10:// 동물 마우스 거리 500f
                transform_cursor.GetComponent<Image>().sprite = MouseCursorOn[4];
                CursorDistanceCheck(150f);
                iconDistance = 30f;
                break;
            case 11:// 나무 마우스 거리 500f
                transform_cursor.GetComponent<Image>().sprite = MouseCursorOn[5];
                CursorDistanceCheck(150f);
                iconDistance = 0f;
                break;
            case 12:// 돌 마우스 거리 500f
                transform_cursor.GetComponent<Image>().sprite = MouseCursorOn[6];
                CursorDistanceCheck(100f);
                iconDistance = 30f;
                break;
            default:
                isChangeSprite = false;
                break;
        }
    }

    private void FindBuildType() {
        BuildingType buildingType = PlayerItemPickControll.ClosestItem.GetComponentInParent<BuildingInteraction>().Type;
        switch (buildingType) {
            case BuildingType.Campfire:
                transform_cursor.GetComponent<Image>().sprite = MouseCursorOn[3];
                CursorDistanceCheck(150f);
                iconDistance = -10f;
                break;
            case BuildingType.Furnace:
                transform_cursor.GetComponent<Image>().sprite = MouseCursorOn[3];
                CursorDistanceCheck(150f);
                iconDistance = -10f;
                break;
            case BuildingType.Shelter:
                transform_cursor.GetComponent<Image>().sprite = MouseCursorOn[2];
                CursorDistanceCheck(200f);
                iconDistance = -5f;
                break;
            case BuildingType.Workshop:
                transform_cursor.GetComponent<Image>().sprite = MouseCursorOn[2];
                CursorDistanceCheck(200f);
                iconDistance = -5f;
                break;
            case BuildingType.Chest:
                transform_cursor.GetComponent<Image>().sprite = MouseCursorOn[3];
                CursorDistanceCheck(150f);
                iconDistance = -10f;
                break;
            default:
                break;
        }

    }

}

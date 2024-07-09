using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Cursor : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {
    private static Cursor instance;
    public static Sprite CursorSprite;

    public static Cursor Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<Cursor>();
            }
            return instance;
        }
    }
    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
            // return => 하단에 초기화가 있을경우
        }
    }


    private void Update() {
        Vector3 mousePosition = Input.mousePosition;
        //mousePosition.z = 10.0f; 
        transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
        Debug.Log("현재 오브젝트 위치 " + transform.position);
        Debug.Log("현재 마우스 위치 " + mousePosition);
    }

    public static Vector2 DefaultPos;

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData) {
        DefaultPos = this.transform.position;
    }

    void IDragHandler.OnDrag(PointerEventData eventData) {
        Vector2 currentPos = eventData.position;
        this.transform.position = currentPos;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData) {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = DefaultPos;
    }




    public bool IsCusorOnButton(Button button) {
        Vector2 CursorPosition = button.GetComponent<RectTransform>().InverseTransformPoint(Input.mousePosition);
        return button.GetComponent<RectTransform>().rect.Contains(CursorPosition);
    }


    private void OnCollisionEnter(Collision col) {
        Debug.Log("마우스와 콜라이더 충돌" + col);
    }

}

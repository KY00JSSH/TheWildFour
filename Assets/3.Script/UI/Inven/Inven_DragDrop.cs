using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Inven_DragDrop : MonoBehaviour {

    private GraphicRaycaster _gr;
    private PointerEventData _ped;
    private List<RaycastResult> _rrList;

    private Inven_Bottom_Box _beginDragSlot; // 현재 드래그를 시작한 슬롯
    private Transform _beginDragIconTransform; // 해당 슬롯의 아이콘 트랜스폼

    private Vector3 _startDragIcon;// 드래그 시작 시 슬롯의 위치
    private Vector3 _startDragMCursor;// 드래그 시작 시 커서의 위치
    private int _DragSlotIndex;

    private Inven_Bottom_Controll InvenCtrl;

    private void Awake() {
        Debug.Log("Awake 메서드 시작");

        _gr = GetComponent<GraphicRaycaster>();
        if (_gr == null) {
            Debug.LogError("GraphicRaycaster가 없음");
            return;
        }

        _ped = new PointerEventData(EventSystem.current);
        Debug.Log("PointerEventData 생성됨: " + _ped);

        _rrList = new List<RaycastResult>();
        Debug.Log("RaycastResult 리스트 생성됨: " + _rrList);

        InvenCtrl = FindObjectOfType<Inven_Bottom_Controll>();
        if (InvenCtrl == null) {
            Debug.LogError("Inven_Bottom_Controll 인스턴스를 찾을 수 없음");
        }

        Debug.Log("Awake 메서드 끝");
    }


    private void Update() {
        if (_ped == null || _gr == null) {
            // Debug.Log("?????");
            return;
        }

        _ped.position = Input.mousePosition;
        OnPointerDown();
        OnPointerDrag();
        OnPointerUp();
    }

    private T RaycastAndGetFirstComponent<T>() where T : Component {
        _rrList.Clear();
        _gr.Raycast(_ped, _rrList);
        Debug.Log("raycast component 확인 : " + _rrList[0]);
        if (_rrList.Count == 0) return null;
        return _rrList[0].gameObject.GetComponent<T>();
    }

    private void OnPointerDown() {

        if (_ped == null || _gr == null || _rrList == null) {
            Debug.Log("dlrj dkslsrjtrkxdk");
            return;
        }
        // 좌클릭 시작
        if (Input.GetMouseButtonDown(0)) {
            // 아이템을 갖고 있는 슬롯만 해당
            _beginDragSlot = RaycastAndGetFirstComponent<Inven_Bottom_Box>();
            if (_beginDragSlot != null && _beginDragSlot.isItemIn) {
                // 위치 기억, 참조 등록
                _beginDragIconTransform = _beginDragSlot.Inven_Item.transform;
                _startDragIcon = _beginDragIconTransform.position;
                _startDragMCursor = Input.mousePosition;

                // 맨 위에 보이기
                _DragSlotIndex = _beginDragSlot.transform.GetSiblingIndex();
                _beginDragSlot.transform.SetAsLastSibling();

                // 해당 슬롯의 하이라이트 이미지를 아이콘보다 뒤에 위치시키기
                // _beginDragSlot.SetHighlightOnTop(false);
            }


        }
        else {
            _beginDragSlot = null;
        }
    }

    private void OnPointerDrag() {
        if (_beginDragSlot == null) return;

        if (Input.GetMouseButton(0)) {
            // 위치 이동
            _beginDragIconTransform.position =
                _startDragIcon + (Input.mousePosition - _startDragMCursor);
        }
    }
    private void OnPointerUp() {
        if (Input.GetMouseButtonUp(0)) {
            // End Drag
            if (_beginDragSlot != null) {
                // 위치 복원
                _beginDragIconTransform.position = _startDragIcon;

                // UI 순서 복원
                _beginDragSlot.transform.SetSiblingIndex(_DragSlotIndex);

                // 드래그 완료 처리
                // EndDrag();

                // 참조 제거
                _beginDragSlot = null;
                _beginDragIconTransform = null;
            }
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Inven_DragDrop : MonoBehaviour {

    private GraphicRaycaster _gr;
    private PointerEventData _ped;
    private List<RaycastResult> _rrList;

    private Inven_Bottom_Box _beginDragSlot; // ���� �巡�׸� ������ ����
    private Transform _beginDragIconTransform; // �ش� ������ ������ Ʈ������

    private Vector3 _startDragIcon;// �巡�� ���� �� ������ ��ġ
    private Vector3 _startDragMCursor;// �巡�� ���� �� Ŀ���� ��ġ
    private int _DragSlotIndex;

   // private Inven_Bottom_Controll InvenCtrl;

    private void Awake() {
        Debug.Log("Awake �޼��� ����");

        _gr = GetComponent<GraphicRaycaster>();
        if (_gr == null) {
            Debug.LogError("GraphicRaycaster�� ����");
            return;
        }

        _ped = new PointerEventData(EventSystem.current);
        Debug.Log("PointerEventData ������: " + _ped);

        _rrList = new List<RaycastResult>();
        Debug.Log("RaycastResult ����Ʈ ������: " + _rrList);

       // InvenCtrl = FindObjectOfType<Inven_Bottom_Controll>();
        //if (InvenCtrl == null) {
        //    Debug.LogError("Inven_Bottom_Controll �ν��Ͻ��� ã�� �� ����");
        //}

        Debug.Log("Awake �޼��� ��");
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
        Debug.Log("raycast component Ȯ�� : " + _rrList[0]);
        if (_rrList.Count == 0) return null;
        return _rrList[0].gameObject.GetComponent<T>();
    }

    private void OnPointerDown() {

        if (_ped == null || _gr == null || _rrList == null) {
            Debug.Log("dlrj dkslsrjtrkxdk");
            return;
        }
        // ��Ŭ�� ����
        if (Input.GetMouseButtonDown(0)) {
            // �������� ���� �ִ� ���Ը� �ش�
            _beginDragSlot = RaycastAndGetFirstComponent<Inven_Bottom_Box>();
            if (_beginDragSlot != null && _beginDragSlot.isItemIn) {
                // ��ġ ���, ���� ���
                //_beginDragIconTransform = _beginDragSlot.Inven_Item.transform;
                _startDragIcon = _beginDragIconTransform.position;
                _startDragMCursor = Input.mousePosition;

                // �� ���� ���̱�
                _DragSlotIndex = _beginDragSlot.transform.GetSiblingIndex();
                _beginDragSlot.transform.SetAsLastSibling();

                // �ش� ������ ���̶���Ʈ �̹����� �����ܺ��� �ڿ� ��ġ��Ű��
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
            // ��ġ �̵�
            _beginDragIconTransform.position =
                _startDragIcon + (Input.mousePosition - _startDragMCursor);
        }
    }
    private void OnPointerUp() {
        if (Input.GetMouseButtonUp(0)) {
            // End Drag
            if (_beginDragSlot != null) {
                // ��ġ ����
                _beginDragIconTransform.position = _startDragIcon;

                // UI ���� ����
                _beginDragSlot.transform.SetSiblingIndex(_DragSlotIndex);

                // �巡�� �Ϸ� ó��
                // EndDrag();

                // ���� ����
                _beginDragSlot = null;
                _beginDragIconTransform = null;
            }
        }
    }


}

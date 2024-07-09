using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MCursor : MonoBehaviour {
    private static MCursor instance;
    public static Sprite CursorSprite;
    public GameObject player;
    private GameObject findCol;

    public static MCursor Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<MCursor>();
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
            // return => �ϴܿ� �ʱ�ȭ�� �������
        }

        Cursor.visible = false;
        transform_cursor.pivot = Vector2.up;
    }

    public RectTransform transform_cursor;

    //TODO: ���콺 Ŀ�� ������ -> Ŀ�� ��������Ʈ ���� 
    private void Update() {
        UpdateCursor();
        findCol = FindGameObject();
        if (findCol != null) {
            Debug.Log(findCol.name);
        }
    }


    private void UpdateCursor() {
        Vector2 mousePos = Input.mousePosition;
        transform_cursor.position = mousePos;
    }



    // �÷��̾� �ֺ����� col ����
    private RaycastHit hit;
    private GameObject FindGameObject() {
        if (player != null) {
            Vector3 rayOrigin = player.transform.position + Vector3.up;
            Vector3 rayDirection = player.transform.rotation * Vector3.forward;

            if (Physics.Raycast(rayOrigin, rayDirection, out hit, 500f)) {
                if (hit.collider.gameObject != null) return hit.collider.gameObject;
            }
        }
        return null;
    }

    // �� ���� player���� ��ġ Debug�뵵
    private void OnDrawGizmos() {
        if (player != null) {
            Gizmos.color = Color.red;
            Vector3 start = player.transform.position + Vector3.up;
            Vector3 end = start + player.transform.rotation * Vector3.forward * 5.0f; // 5.0f�� �׸� ���� ����

            Gizmos.DrawLine(start, end);
        }
    }

    public bool IsCusorOnButton(Button button) {
        Vector2 CursorPosition = button.GetComponent<RectTransform>().InverseTransformPoint(Input.mousePosition);
        return button.GetComponent<RectTransform>().rect.Contains(CursorPosition);
    }


}

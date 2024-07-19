using System.Collections.Generic;
using UnityEngine;

public class PlayerItemUseControll : MonoBehaviour {
    [SerializeField]
    private float holdTime = 1.5f;
    private float timer = 0f;
    private bool isKeyPressed = false;
    private bool isLong = false;

    private InventoryBox invenBox;
    private InvenDrop invenDrop;
    private InvenController invenCont;

    private int selectBoxKey;

    private void Awake() {
        invenDrop = FindObjectOfType<InvenDrop>();
        invenBox = FindObjectOfType<InventoryBox>();
        invenCont = FindObjectOfType<InvenController>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.F)) {
            //������ ���
            invenCont.useInvenItem(selectBoxKey);
        }

        if (Input.GetKeyDown(KeyCode.T)) {
            isKeyPressed = true;
            isLong = true;
            timer = 0f; // Ÿ�̸� �ʱ�ȭ
        }

        if (isKeyPressed) {
            timer += Time.deltaTime;

            if (timer >= holdTime && isLong) {
                // ��� ������ - ��ü ������ ���
                //�������� �����ϴ��� üũ �� Drop
                invenDrop.DropItemAll(selectBoxKey);
                isKeyPressed = false;
                isLong = false;
                timer = 0f;
            }

            if (Input.GetKeyUp(KeyCode.T)) {
                invenDrop.DropItem(selectBoxKey); // ª�� Ŭ������ ������ �� �� ����߸���
                isKeyPressed = false;
                timer = 0f;
            }
        }
    }

    public void SetSelectedBoxKey(int key) {
        selectBoxKey = key;
    }
}

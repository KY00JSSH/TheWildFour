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
        if (PlayerStatus.isDead) return;

        if (Input.GetKeyDown(KeyCode.F)) {
            //아이템 사용
            invenCont.useInvenItem(selectBoxKey);
        }

        if (Input.GetKeyDown(KeyCode.T)) {
            isKeyPressed = true;
            isLong = true;
            timer = 0f; // 타이머 초기화
        }

        if (isKeyPressed) {
            timer += Time.deltaTime;

            if (timer >= holdTime && isLong) {
                // 길게 누르기 - 전체 아이템 드랍
                //아이템이 존재하는지 체크 후 Drop
                invenDrop.dropItemAll(selectBoxKey);
                isKeyPressed = false;
                isLong = false;
                timer = 0f;
            }

            if (Input.GetKeyUp(KeyCode.T)) {
                invenDrop.dropItem(selectBoxKey); // 짧은 클릭으로 아이템 한 개 떨어뜨리기
                isKeyPressed = false;
                timer = 0f;
            }
        }
    }

    public void SetSelectedBoxKey(int key) {
        selectBoxKey = key;
    }
}

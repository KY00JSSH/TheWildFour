using UnityEngine;

public class PlayerItemUseControll : MonoBehaviour
{
    [SerializeField]
    private float holdTime = 2f;
    private float timer = 0f;
    private bool isHolding = false;

    private InvenController invenController;
    private InventoryBox invenBox;

    int selectBoxKey;

    private void Start() {
        invenController = GetComponent<InvenController>();
        invenBox = GetComponent<InventoryBox>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.F)) {
            //������ ���
        }

        if (Input.GetKey(KeyCode.T)) {
            if (!isHolding) {
                isHolding = true;
                timer = 0f;
            }
            else {
                timer += Time.deltaTime;

                if (timer >= holdTime) {
                    //TODO: ��� ������ ��ü
                    isHolding = false;
                    timer = 0f;
                }
            }
        }
        else {
            isHolding = false;
            timer = 0f;
        }
    }

    public void SetSelectedBoxKey(int key) {
        selectBoxKey = key;
        Debug.Log("Selected Box Key: " + selectBoxKey);
    }

}

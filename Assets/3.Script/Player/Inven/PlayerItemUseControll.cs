using System.Collections.Generic;
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
        invenController = FindObjectOfType<InvenController>();
        invenBox = FindObjectOfType<InventoryBox>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.F)) {
            //æ∆¿Ã≈€ ªÁøÎ
        }

        if (Input.GetKey(KeyCode.T)) {
            if (!isHolding) {
                isHolding = true;
                timer = 0f;
            }
            else {
                timer += Time.deltaTime;

                if (timer >= holdTime) {
                    //æ∆¿Ã≈€ «—π∂≈÷¿Ã ∂≥±∏±‚
                    List<Item> inven = invenController.Inventory;
                    Vector3 itemDropPosition = new Vector3(gameObject.transform.position.x-0.1f, gameObject.transform.position.y+0.5f, gameObject.transform.position.z - 0.1f);
                    Item itemComponent = inven[selectBoxKey];
                    Instantiate(itemComponent.itemData.DropItemPrefab, itemDropPosition, Quaternion.identity);
                    invenController.removeItem(selectBoxKey);
                    invenController.invenFullFlagReset();
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
    }


}

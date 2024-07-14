using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemControll : MonoBehaviour {

    [SerializeField] private float checkRadius = 5.0f;
    private InvenController invenController;

    private void Start() {
        invenController = FindObjectOfType<InvenController>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            CheckForItems();
        }
    }

    private void CheckForItems() {
        int layerMask = 1 << 6;
        Collider[] cols = Physics.OverlapSphere(gameObject.transform.position, checkRadius, layerMask);
        float closestDistance = Mathf.Infinity;
        GameObject closestItem = null;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 mousePosition = Vector3.zero;

        if (Physics.Raycast(ray, out hit)) {
            mousePosition = hit.point;
        }

        foreach (Collider hitCol in cols) {
            float distanceToMouse = Vector3.Distance(hitCol.transform.position, mousePosition);
            if (distanceToMouse < closestDistance) {
                closestDistance = distanceToMouse;
                closestItem = hitCol.gameObject;
            }
        }

        if (closestItem != null) {
            ShowTooltip(closestItem);
            PickupItem(closestItem);
        }
    }

    private void ShowTooltip(GameObject item) {
        Debug.Log("Tooltip 보여줌");
    }

    private void PickupItem(GameObject item) {
        if (item != null) {
            Item itemComponent = item.GetComponent<Item>();
            if (itemComponent == null) {
                Debug.LogWarning("아이템에 Item 컴포넌트가 없습니다.");
                return;
            }
            else {
                if (itemComponent.itemData != null) {
                   // Debug.Log("Item Name: " + itemComponent.itemData.ItemName);
                }
                else {
                    Debug.LogError("itemComponent.itemData가 null입니다.");
                    return;
                }
            }
            if (invenController == null) {
                Debug.LogError("invenController가 초기화되지 않았습니다.");
                return;
            }
            if (!invenController.IsInvenFull) {
                invenController.itemObejct = item;
                invenController.ItemAdd();
                Destroy(item);
            }
        }
        else {
            Debug.LogWarning("PickupItem이 null 아이템으로 호출되었습니다.");
        }
    }
}

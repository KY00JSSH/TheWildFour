using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemPickControll : MonoBehaviour {

    [SerializeField] private float checkRadius = 2.5f;
    private InvenController invenController;
    [SerializeField] private GameObject player;

    private GameObject closestItem;
    public static GameObject ClosestItem { get { return GameObject.FindObjectOfType<PlayerItemPickControll>().closestItem; } }

    private GameObject mouseHoverItem;

    private GameObject previousItem = null;

    private void Start() {
        invenController = FindObjectOfType<InvenController>();
    }

    private void Update() {
        if (PlayerStatus.isDead) return;

        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) {
            CheckForItems();
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            pickupItem(closestItem);
        }

        if (Input.GetMouseButtonDown(0) && mouseHoverItem == closestItem) {
            pickupItem(closestItem);
        }
    }

    private void CheckForItems() {
        int layerMask = (1 << 8) + (1 << 9) + (1 << 10) + (1 << 11) + (1 << 12);
        Collider[] cols = Physics.OverlapSphere(player.transform.position, checkRadius, layerMask);

        float closestDistance = Mathf.Infinity;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 mousePosition = Vector3.zero;

        if (Physics.Raycast(ray, out hit)) {
            mousePosition = hit.point;
            mouseHoverItem = hit.collider.gameObject;
        }

        foreach (Collider hitCol in cols) {
            float distanceToMouse = Vector3.Distance(hitCol.transform.position, mousePosition);
            if (distanceToMouse < closestDistance) {
                closestDistance = distanceToMouse;
                closestItem = hitCol.gameObject;
            }
        }

        if (closestItem != null) {
            if (previousItem != closestItem) {
                ShowTooltip(closestItem);
                if (previousItem != null) {
                    if (previousItem.GetComponent<ItemSelectControll>() != null) {
                        previousItem.GetComponent<ItemSelectControll>().outSelect();
                    }
                }

                if (closestItem.GetComponent<ItemSelectControll>() != null) {
                    closestItem.GetComponent<ItemSelectControll>().selectItem();
                }
                previousItem = closestItem;
            }
        }
        else if (previousItem != null) {
            // 선택된 아이템이 없을 때 이전 아이템의 outSelect 호출
            if (previousItem.GetComponent<ItemSelectControll>() != null) {
                previousItem.GetComponent<ItemSelectControll>().outSelect();
            }
            previousItem = null;
        }
    }

    //sphere 확인용 gizmo
    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(player.transform.position, checkRadius);
    }

    //tooltip 보여주는 시점
    private void ShowTooltip(GameObject item) {
        // Debug.Log("Tooltip 보여줌");
    }

    //아이템 줍기
    private void pickupItem(GameObject item) {
        if (item != null && item.layer == 8) {
            invenController.itemObject = item;
            if (invenController.canItemAdd()) {
                invenController.ItemAdd();
                item.SetActive(false);
                //Destroy(item);
            }
        }
        else {
            //Debug.LogWarning("null");
        }
    }
}

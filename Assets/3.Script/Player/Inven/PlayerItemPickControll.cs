using UnityEngine;

public class PlayerItemPickControll : MonoBehaviour {

    [SerializeField] private float checkRadius = 2.5f;
    private InvenController invenController;
    private GameObject player;

    private GameObject closestItem;
    public static GameObject ClosestItem { get { return GameObject.FindObjectOfType<PlayerItemPickControll>()?.closestItem; } }

    private GameObject mouseHoverItem;

    private GameObject previousItem = null;

    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
    }
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
    //private void OnDrawGizmos() {
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(player.transform.position, checkRadius);
    //}

    //아이템 줍기
    private void pickupItem(GameObject item) {
        if (item != null && item.layer == 8) {
            invenController.itemObject = item;
            //겹쳐서 넣을 수 있는지 확인
            if(item.GetComponent<CountableItem>() != null) {
                int checkNum = invenController.canAddThisBox(item.GetComponent<Item>().Key);
                if(checkNum != 99) {
                        //겹쳐서 넣을수 있으면 집은 필드 아이템은 destroy
                        invenController.ItemAdd();
                        Destroy(item);
                }
                else {
                    if (invenController.canItemAdd()) {
                        //겹쳐서 넣을수 없으면 집은 필드의 아이템은 active-false
                        invenController.ItemAdd();
                        item.SetActive(false);
                    }
                }
            }
            else {
                if (invenController.canItemAdd()) {
                    //겹쳐서 넣을수 없으면 집은 필드의 아이템은 active-false
                    invenController.ItemAdd();
                    item.SetActive(false);
                }
            }
        }
        else {
            //Debug.LogWarning("null");
        }
    }
}

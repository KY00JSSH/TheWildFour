using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenUIController : MonoBehaviour {
    private InvenController invenController;

    private List<GameObject> InvenTotal = new List<GameObject>();

    private int currInvenCount = 0;         //���� �κ��丮 Ȱ��ȭ ����
    public int CurrInvenCount { get { return currInvenCount; } }
    private int invenMaxcount = 15;         //�κ��丮 Ȱ��ȭ �ִ� ����    

    public GameObject InvenBoxPrefab;       //�κ��丮 UI BOX Prefab

    private void Awake() {
        currInvenCount = 8;
        invenController = GetComponent<InvenController>();
        invenController.InvenChanged += UpdateUI;
        InvenTotal = InitInven();
        for (int i = 0; i < currInvenCount; i++) {
            InvenTotal[i].SetActive(true);
        }
    }

    private void OnDestroy() {
        invenController.InvenChanged -= UpdateUI;
    }

    private void UpdateUI(List<Item> inventory) {
        for (int i = 0; i < inventory.Count; i++) {
            if (inventory[i] is CountableItem ci) {
                Debug.Log($"{i} : {inventory[i].itemData.ItemName} , {ci.CurrStackCount}");
            }
            else {
                Debug.Log($"{i} : {inventory[i].itemData.ItemName} - ITEM");
            }
        }

        for (int i = 0; i < currInvenCount; i++) {
            InventoryBox box = InvenTotal[i].GetComponent<InventoryBox>();
            if (box != null) {
                if (i < inventory.Count) {
                    Item newItem = inventory[i];
                    if (box.CurrentItem != newItem) {
                        box.UpdateBox(inventory[i]);
                    }
                    else {
                        if (box.CurrentItem is CountableItem ci) {
                            if (ci.CurrStackCount != 0) {
                                box.UpdateBox(newItem);
                            }
                        }
                    }
                }
                else {
                    if (box.CurrentItem != null) {
                        box.UpdateBox(null);
                    }
                }
            }
        }
    }

    //�κ��丮 �ʱ�ȭ - �ִ� Ȱ��ȭ ������ŭ �̸� ���� �� ���� Ȱ��ȭ ���� ������ active
    private List<GameObject> InitInven() {
        List<GameObject> InvenBox = new List<GameObject>();
        for (int i = 0; i < invenMaxcount; i++) {
            Vector3 invenPosition = new Vector3(transform.position.x + i * 75, transform.position.y, InvenBoxPrefab.transform.position.z);
            GameObject invenBoxPrefabs = Instantiate(InvenBoxPrefab, invenPosition, Quaternion.identity);
            invenBoxPrefabs.transform.SetParent(transform);
            invenBoxPrefabs.name = InvenBoxPrefab.name;
            invenBoxPrefabs.SetActive(false);
            InventoryBox invenBox = invenBoxPrefabs.GetComponent<InventoryBox>();
            if (invenBox != null) {
                invenBox.setKey(i); // key ����
            }
            InvenBox.Add(invenBoxPrefabs);
        }
        return InvenBox;
    }

    //inven upgrade�� �κ��丮 �߰� Ȱ��ȭ - upgrade �ϴ� ���� �Ź� ȣ�� �ʿ�
    public void InvenCountUpgrade() {
        if (currInvenCount <= invenMaxcount) {
            currInvenCount++;
            invenController.invenFullReset();
            InvenTotal[currInvenCount - 1].SetActive(true);

            InventoryBox box = InvenTotal[currInvenCount - 1].GetComponent<InventoryBox>();
            if (box != null) {
                box.UpdateBox(null);
            }
        }
        else {
            Debug.Log("Inventory is already at maximum capacity.");
            return;
        }
    }
}

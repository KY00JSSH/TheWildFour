using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenUIController : MonoBehaviour {
    private InvenController invenController;

    private List<GameObject> InvenTotal = new List<GameObject>();

    private int currInvenCount = 0;         //현재 인벤토리 활성화 개수
    public int CurrInvenCount { get { return currInvenCount; } }
    private int invenMaxcount = 15;         //인벤토리 활성화 최대 개수    

    public GameObject InvenBoxPrefab;       //인벤토리 UI BOX Prefab

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
            InventoryBox box = InvenTotal[i].GetComponent<InventoryBox>();
            box.UpdateBox(inventory[i]);
        }
    }

    //인벤토리 초기화 - 최대 활성화 개수만큼 미리 생성 후 현재 활성화 가능 개수만 active
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
                invenBox.setKey(i); // key 설정
            }
            InvenBox.Add(invenBoxPrefabs);
        }
        return InvenBox;
    }

    //inven upgrade시 인벤토리 추가 활성화 - upgrade 하는 순간 매번 호출 필요
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
            return;
        }
    }
}

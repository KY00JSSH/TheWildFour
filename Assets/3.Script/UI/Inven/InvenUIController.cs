using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenUIController : MonoBehaviour {
    private InvenController invenController;    //인벤토리 로직 컨트롤러

    private List<GameObject> invenTotalList = new List<GameObject>();   //전체 인벤박스 리스트
    public List<GameObject> InvenTotalList { get { return invenTotalList; } }

    public GameObject InvenBoxPrefab;       //인벤토리 UI BOX Prefab

    private int currInvenCount = 0;         //현재 인벤토리 활성화 개수
    public int CurrInvenCount { get { return currInvenCount; } }
    private int invenMaxcount = 15;         //인벤토리 활성화 최대 개수    

    private List<Vector2[]> boxPositionsList = new List<Vector2[]>();
    public List<Vector2[]> BoxPositions { get { return boxPositionsList; } }

    private void Awake() {
        //기본 인벤 초기화
        invenController = GetComponent<InvenController>();
        InitInven();
        //실제 인벤 데이터가 변할때마다 UI 업데이트 되도록 옵저버 패턴으로 구독 설정
        invenController.InvenChanged += UpdateUI;
    }

    private void OnDestroy() {
        invenController.InvenChanged -= UpdateUI;
    }

    //인벤토리의 데이터가 변할때마다 UI 업데이트
    private void UpdateUI(List<Item> inventory) {
        for (int i = 0; i < inventory.Count; i++) {
            InventoryBox box = invenTotalList[i].GetComponent<InventoryBox>();
            box.UpdateBox(inventory[i]);
        }
    }

    //인벤토리 초기화 - 최대 활성화 개수만큼 미리 생성 후 현재 활성화 가능 개수만 active
    private void InitInven() {
        currInvenCount = 8; //기본 활성화 개수 설정
        List<GameObject> invenBoxList = new List<GameObject>();
        for (int i = 0; i < invenMaxcount; i++) {
            Vector3 invenPosition = new Vector3(transform.position.x + i * 75, transform.position.y, InvenBoxPrefab.transform.position.z);
            GameObject invenBoxPrefabs = Instantiate(InvenBoxPrefab, invenPosition, Quaternion.identity);
            invenBoxPrefabs.transform.SetParent(transform);
            invenBoxPrefabs.name = InvenBoxPrefab.name;
            invenBoxPrefabs.SetActive(false);
            InventoryBox invenBoxPrf = invenBoxPrefabs.GetComponent<InventoryBox>();
            if (invenBoxList != null) {
                invenBoxPrf.setKey(i); // key 설정
            }
            invenBoxList.Add(invenBoxPrefabs);
            //positon 왼쪽위, 오른쪽위, 오른쪽아래, 왼쪽아래 순으로 설정
            boxPositionsList.Add(new Vector2[]{
                new Vector2(invenBoxList[i].transform.position.x - 35, invenBoxList[i].transform.position.y + 35),
                new Vector2(invenBoxList[i].transform.position.x + 35, invenBoxList[i].transform.position.y + 35),
                new Vector2(invenBoxList[i].transform.position.x + 35, invenBoxList[i].transform.position.y - 35),
                new Vector2(invenBoxList[i].transform.position.x - 35, invenBoxList[i].transform.position.y - 35)
                });
        }
        invenTotalList = invenBoxList;
        for (int i = 0; i < currInvenCount; i++) {
            invenTotalList[i].SetActive(true);
        }
    }

    //inven upgrade시 인벤토리 추가 활성화 - upgrade 하는 순간 매번 호출 필요
    public void InvenCountUpgrade() {
        if (currInvenCount <= invenMaxcount) {
            currInvenCount++;
            invenController.invenFullReset();
            invenTotalList[currInvenCount - 1].SetActive(true);

            InventoryBox box = invenTotalList[currInvenCount - 1].GetComponent<InventoryBox>();
            if (box != null) {
                box.UpdateBox(null);
            }
        }
        else {
            return;
        }
    }
}

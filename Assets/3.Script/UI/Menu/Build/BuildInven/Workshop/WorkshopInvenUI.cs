using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkshopInvenUI : MonoBehaviour
{
    private List<GameObject> invenTotalList = new List<GameObject>();   //전체 인벤박스 리스트
    public List<GameObject> InvenTotalList { get { return invenTotalList; } }

    private int currInvenCount = 0;         //현재 인벤토리 활성화 개수
    public int CurrInvenCount { get { return currInvenCount; } }
    private int invenMaxcount = 8;         //인벤토리 활성화 최대 개수    

    public GameObject InvenBoxPrefab;       //BOX Prefab

    private WorkshopInvenControll workshopInven;    //인벤토리 로직 컨트롤러
    private void Awake() {
        initInven();
        workshopInven = GetComponent<WorkshopInvenControll>();
        workshopInven.InvenChanged += UpdateUI;
    }

    private void OnDestroy() {
        workshopInven.InvenChanged -= UpdateUI;
    }

    private void UpdateUI(List<GameObject> inventory) {
        for (int i = 0; i < inventory.Count; i++) {
            WorkshopInvenBox box = invenTotalList[i].GetComponent<WorkshopInvenBox>();
            box.UpdateBox(inventory[i]);
        }
    }

    private void initInven() {
        currInvenCount = 4;
        List<GameObject> invenBoxList = new List<GameObject>();
        for (int i = 0; i < invenMaxcount; i++) {
            Vector3 position;
            if (i % 2 == 0) {
                position = new Vector3((transform.position.x - 30f) + (i * 40.0f), (transform.position.y - 70f), 0f);
            }
            else {
                int index = i / 2;
                position = new Vector3((transform.position.x - 30f) + (index * 80.0f), (transform.position.y - 70f) - 80f, 0f);
            }
            GameObject invenBoxPrefabs = Instantiate(InvenBoxPrefab, position, Quaternion.identity);
            invenBoxPrefabs.transform.SetParent(transform);
            invenBoxPrefabs.name = $"inven{i}";
            invenBoxPrefabs.SetActive(false);
            WorkshopInvenBox workshopInvenBox = invenBoxPrefabs.GetComponent<WorkshopInvenBox>();
            if (invenBoxList != null) {
                workshopInvenBox.setKey(i); // key 설정
            }
            invenBoxList.Add(invenBoxPrefabs);
        }
        invenTotalList = invenBoxList;
        for (int i = 0; i < currInvenCount; i++) {
            invenTotalList[i].SetActive(true);
        }
    }
}

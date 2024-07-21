using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenUIController : MonoBehaviour {
    private InvenController invenController;    //�κ��丮 ���� ��Ʈ�ѷ�

    private List<GameObject> invenTotalList = new List<GameObject>();   //��ü �κ��ڽ� ����Ʈ
    public List<GameObject> InvenTotalList { get { return invenTotalList; } }

    public GameObject InvenBoxPrefab;       //�κ��丮 UI BOX Prefab

    private int currInvenCount = 0;         //���� �κ��丮 Ȱ��ȭ ����
    public int CurrInvenCount { get { return currInvenCount; } }
    private int invenMaxcount = 15;         //�κ��丮 Ȱ��ȭ �ִ� ����    

    private void Awake() {
        //�⺻ �κ� �ʱ�ȭ
        invenController = GetComponent<InvenController>();
        initInven();
        //���� �κ� �����Ͱ� ���Ҷ����� UI ������Ʈ �ǵ��� ������ �������� ���� ����
        invenController.InvenChanged += UpdateUI;
    }

    private void OnDestroy() {
        invenController.InvenChanged -= UpdateUI;
    }

    //�κ��丮�� �����Ͱ� ���Ҷ����� UI ������Ʈ
    private void UpdateUI(List<Item> inventory) {
        for (int i = 0; i < inventory.Count; i++) {
            InventoryBox box = invenTotalList[i].GetComponent<InventoryBox>();
            box.UpdateBox(inventory[i]);
        }
    }

    //�κ��丮 �ʱ�ȭ - �ִ� Ȱ��ȭ ������ŭ �̸� ���� �� ���� Ȱ��ȭ ���� ������ active
    private void initInven() {
        currInvenCount = 8; //�⺻ Ȱ��ȭ ���� ����
        List<GameObject> invenBoxList = new List<GameObject>();
        for (int i = 0; i < invenMaxcount; i++) {
            Vector3 invenPosition = new Vector3(transform.position.x + i * 75, transform.position.y, InvenBoxPrefab.transform.position.z);
            GameObject invenBoxPrefabs = Instantiate(InvenBoxPrefab, invenPosition, Quaternion.identity);
            invenBoxPrefabs.transform.SetParent(transform);
            invenBoxPrefabs.name = InvenBoxPrefab.name;
            invenBoxPrefabs.SetActive(false);
            InventoryBox invenBoxPrf = invenBoxPrefabs.GetComponent<InventoryBox>();
            if (invenBoxList != null) {
                invenBoxPrf.setKey(i); // key ����
            }
            invenBoxList.Add(invenBoxPrefabs);
            //positon ������, ��������, �����ʾƷ�, ���ʾƷ� ������ ����
        }
        invenTotalList = invenBoxList;
        for (int i = 0; i < currInvenCount; i++) {
            invenTotalList[i].SetActive(true);
        }
    }

    //inven upgrade�� �κ��丮 �߰� Ȱ��ȭ - upgrade �ϴ� ���� �Ź� ȣ�� �ʿ�
    public void InvenCountUpgrade() {
        if (currInvenCount <= invenMaxcount) {
            currInvenCount++;
            invenController.addInvenBox();          //Ȱ��ȭ 1�� �ɶ����� ���� ������ inventory.add(null)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenUIController : MonoBehaviour {

    private List<GameObject> InvenTotal = new List<GameObject>();

    private int currInvenCount = 0;         //���� �κ��丮 Ȱ��ȭ ����
    public int CurrInvenCount { get { return currInvenCount; } }
    private int invenMaxcount = 15;         //�κ��丮 Ȱ��ȭ �ִ� ����    

    public GameObject InvenBoxPrefab;       //�κ��丮 UI BOX Prefab
    private InvenController invenController;

    private void Awake() {
        currInvenCount = 8;
        invenController = GetComponent<InvenController>();
        InvenTotal = InitInven();
        for (int i = 0; i < currInvenCount; i++) {
            InvenTotal[i].SetActive(true);
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
            InvenBox.Add(invenBoxPrefabs);
        }
        return InvenBox;
    }

    //inven upgrade�� �κ��丮 �߰� Ȱ��ȭ - upgrade �ϴ� ���� �Ź� ȣ�� �ʿ�
    public void InvenCountUpgrade() {
        if (currInvenCount <= invenMaxcount) {
            currInvenCount++;
            invenController.invenFullReset();
            InvenTotal[currInvenCount].SetActive(true);
        }
        else {
            return;
        }
    }
}

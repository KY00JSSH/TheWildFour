using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inven_Bottom_Controll : MonoBehaviour {

    /*
     1. inven_count��ŭ �κ� ������ ����
     2. �÷��̾ space Ű�� ������ Ȱ��ȭ �Ǿ��ִ� ������ŭ ���鼭 ���� �̸��� �ִ��� Ȯ�� �ϱ� 
        2-1. ���� �������̸� �ֱ�
        2-2. �ٸ� �������̸� ó������ �ֱ�

     */
    private int inven_count = 0;
    private int inven_Maxcount = 15;
    public GameObject InvenBoxPrefab;
    private List<GameObject> InvenTotal = new List<GameObject>();

    private GameObject Item;
    public GameObject Player;
    private bool isInvenFull;

    private void Awake() {
        inven_count = 8;
        InvenTotal = InitInven();
        Debug.Log("��ü �κ�" + InvenTotal.Count);
        for (int i = 0; i < inven_count; i++) {
            InvenTotal[i].SetActive(true);
        }
    }

    //private void Update() {
    //    if (ItemGetCheck()) {
    //        ItemAdd();
    //        return;
    //    }
    //}

    // �̸� inven_Maxcount��ŭ pooling �س���
    private List<GameObject> InitInven() {
        List<GameObject> InvenBox = new List<GameObject>();
        for (int i = 0; i < inven_Maxcount; i++) {
            Vector3 invenPosition = new Vector3(transform.position.x + i * 75, transform.position.y, InvenBoxPrefab.transform.position.z);
            GameObject invenBoxPrefabs = Instantiate(InvenBoxPrefab, invenPosition, Quaternion.identity);
            invenBoxPrefabs.transform.SetParent(transform);
            invenBoxPrefabs.name = InvenBoxPrefab.name;
            invenBoxPrefabs.SetActive(false);

            InvenBox.Add(invenBoxPrefabs);
        }
        return InvenBox;
    }

    // inven upgrade �� �Լ� ȣ���ϸ� �κ��丮 �߰� Ȱ��ȭ
    public void InvenCountUpgrade() {
        inven_count++;
        isInvenFull = false;
        if (inven_count <= inven_Maxcount) {
            InvenTotal[inven_count].SetActive(true);
        }
        else {
            return;
        }
    }

    // ����ڰ� space�� ������ ���
   

    //TODO: ���콺�� Ŭ���� ������
    public void ItemAdd() {
        int invenindex = ItemBoxAvailableCheck();
        // invencount ��ŭ �κ��� ���鼭 ã�� �������� �ִ� �� Ȯ��
        if (invenindex != 99) {
            // ���� �κ��� �ִ� �������� ��� ������ count ����
            ItemAddInven(invenindex);
        }
        else {
            // ���ٸ� �ű� �߰�
            NewItemAddInven();
        }

    }

    // �κ��丮�� �����ִ� ���� ��ŭ ������ ���� ������ �˻�
    private int ItemBoxAvailableCheck() {
        int itemBoxNum;
        int invenUseCount = 0;

        for (int i = 0; i < inven_count; i++) {
            if (InvenTotal[i].TryGetComponent(out Inven_Bottom_Box box)) {

                if (box.isItemIn) {
                    if (box.isInvenBoxAvailable) {
                        // �κ��� 80���� ������ �ʾҴٸ� ������ �̸� �˻�
                        if (Item.name == box.Inven_Item.name) {
                            isInvenFull = false;
                            itemBoxNum = i;
                            return itemBoxNum;
                        }
                    }
                    else if ((box.isInvenBoxAvailable && Item.name != box.Inven_Item.name)
                        || (Item.name == box.Inven_Item.name && !box.isInvenBoxAvailable)) {
                        invenUseCount++;
                    }
                }

            }
        }

        if (invenUseCount >= inven_count) {
            Debug.Log("�κ��丮 ��� �Ұ�");
            isInvenFull = true;
        }
        else {
            isInvenFull = false;
        }
        return 99;
    }

    private void NewItemAddInven() {
        if (Item != null) {
            for (int i = 0; i < inven_count; i++) {
                if (InvenTotal[i].TryGetComponent(out Inven_Bottom_Box box)) {
                    if (!box.isItemIn) {
                        box.ItemIn(Item);
                        Debug.Log("������ Ȯ�� " + Item.name);
                        break;
                    }
                }
            }
        }

    }

    private void ItemAddInven(int index) {
        if (Item != null) {
            if (InvenTotal[index].TryGetComponent(out Inven_Bottom_Box box)) {
                box.ItemIn(Item);
            }
        }
    }



}
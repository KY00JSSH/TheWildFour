using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemControll : MonoBehaviour {

    private void Update() {
        if (ItemGetCheck()) {
            //TODO: item ���� �� item inventory �߰� �������� ���� üũ
            //inventory ��ü �������� ������������ alert

        }
    }

    private bool ItemGetCheck() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            // �Ÿ� 5������ �ݶ��̴� ��ü ����
            //TODO: �÷��̾� �������θ� �ݶ��̴� �����ϰ� �ؾ���
            Collider[] cols = Physics.OverlapSphere(gameObject.transform.position, 5.0f);
            foreach (Collider col in cols) {
                if (col.gameObject.layer == 6) {

                    Debug.Log("length" + cols.Length);
                    Debug.Log("length" + col.name);

                }
            }
            //Item itemComponent = cols[i].GetComponent<Item>();

            //if (itemComponent.itemData is FoodItemData foodItemData) {
            //    Debug.Log($"{foodItemData.ItemName} + {foodItemData.FullPoint}");
            //}
            //if (cols[i].tag == "Item_Weapon" || cols[i].tag == "Item_Food" || cols[i].tag == "Item_Ingre" || cols[i].tag == "Item_Etc") {
            //Debug.Log("������ ã��" + cols[i].name);
            //   Debug.Log("������ ã��" + cols[i].tag);
            //   Item = cols[i].gameObject;
            //    return true;
            //}
            //else {
            //    Debug.Log("�÷��̾� �ֺ� ������ ����");
            //    //Item = null;
            //}
        }
        return false;
    }

}

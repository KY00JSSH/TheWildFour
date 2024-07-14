using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemControll : MonoBehaviour {

    private void Update() {
        if (ItemGetCheck()) {
            //TODO: item 검출 후 item inventory 추가 가능한지 여부 체크
            //inventory 전체 차있으면 가져갈수없음 alert

        }
    }

    private bool ItemGetCheck() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            // 거리 5안쪽의 콜라이더 전체 검출
            //TODO: 플레이어 앞쪽으로만 콜라이더 검출하게 해야함
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
            //Debug.Log("아이템 찾음" + cols[i].name);
            //   Debug.Log("아이템 찾음" + cols[i].tag);
            //   Item = cols[i].gameObject;
            //    return true;
            //}
            //else {
            //    Debug.Log("플레이어 주변 아이템 없음");
            //    //Item = null;
            //}
        }
        return false;
    }

}

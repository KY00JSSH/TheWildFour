using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inven_Bottom_Controll : MonoBehaviour {

    /*
     1. inven_count만큼 인벤 프리펩 생성
     2. 플레이어가 space 키를 누르면 활성화 되어있는 갯수만큼 돌면서 같은 이름이 있는지 확인 하기 
        2-1. 같은 아이템이면 넣기
        2-2. 다른 아이템이면 처음부터 넣기

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
        Debug.Log("전체 인벤" + InvenTotal.Count);
        for (int i = 0; i < inven_count; i++) {
            InvenTotal[i].SetActive(true);
        }
    }

    private void Update() {
        if (ItemGetCheck()) {
            ItemAdd();
            return;
        }
    }

    // 미리 inven_Maxcount만큼 pooling 해놓음
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

    // inven upgrade 시 함수 호출하면 인벤토리 추가 활성화
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



    // 사용자가 space를 눌렀을 경우
    private bool ItemGetCheck() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            // 거리 5안쪽의 콜라이더 전체 검출
            //TODO: 플레이어 앞쪽으로만 콜라이더 검출하게 해야함
            Collider[] cols = Physics.OverlapSphere(Player.transform.position, 5.0f);
            if (cols.Length > 0) {

                for (int i = 0; i < cols.Length; i++) {
                    if (cols[i].tag == "Item_Weapon" || cols[i].tag == "Item_Food" || cols[i].tag == "Item_Ingre" || cols[i].tag == "Item_Etc") {
                        Debug.Log("아이템 찾음" + cols[i].name);
                        Debug.Log("아이템 찾음" + cols[i].tag);
                        Item = cols[i].gameObject;
                        return true;
                    }
                }
            }
            else {
                Debug.Log("플레이어 주변 아이템 없음");
                Item = null;
            }
        }
        return false;
    }

    //TODO: 마우스로 클릭시 들어가야함
    public void ItemAdd() {
        int invenindex = ItemBoxAvailableCheck();
        // invencount 만큼 인벤을 돌면서 찾은 아이템이 있는 지 확인
        if (invenindex != 99) {
            // 기존 인벤에 있던 아이템일 경우 아이템 count 증가
            ItemAddInven(invenindex);
        }
        else {
            // 없다면 신규 추가
            NewItemAddInven();
        }

    }

    // 인벤토리가 열려있는 개수 만큼 돌려서 같은 아이템 검사
    private int ItemBoxAvailableCheck() {
        int itemBoxNum;
        int invenUseCount = 0;

        for (int i = 0; i < inven_count; i++) {
            if (InvenTotal[i].TryGetComponent(out Inven_Bottom_Box box)) {

                if (box.isItemIn) {
                    if (box.isInvenBoxAvailable) {
                        // 인벤에 80개로 꽉차지 않았다면 아이템 이름 검사
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
            Debug.Log("인벤토리 사용 불가");
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
                        Debug.Log("아이템 확인 " + Item.name);
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Location {
    Normal, Inventory
}

// 1. 아이템을 생성할 때마다 Register() 메서드를 통해 ItemManager의 자식 객체로 두고,
// 2. 인벤토리 아이템의 경우 Position을 특정 값으로 줘서 관리
// 3. 저장할 때는 하위 자식 객체를 순회해서 key와 position 값을 주고
// 4. 불러올 때는 key와 position 값을 가져온 뒤 key 값에 해당하는 아이템을 Instantiate
// 5. 불러온 position 값이 인벤토리 특정 값일 경우 List에 Instantiate 한 객체를 저장하고
// 6. 불러오기가 끝난 뒤에 저장된 Inventory 리스트를 CommonInven의 inventory에 대입.
// 7. InventoryUI 는 Awake()나 Start() 에서 CommonInven을 조사해서 대응하는 이미지를 띄울 것
// 8. 중첩되는 아이템 개수는 추가 구현해야함

public class ItemManager : MonoBehaviour {
    [SerializeField] private GameObject[] itemPrefabs;

    public List<int> ItemKey = new List<int>();
    public List<Vector3> ItemPosition = new List<Vector3>();
    public List<GameObject> InventoryItem = new List<GameObject>();

    private void Awake() {
        while (InventoryItem.Count < Save.Instance.saveData.playerInvenCount)
            InventoryItem.Add(null);

        if (Save.Instance.saveData.ItemKey != null) {
            ItemKey = Save.Instance.saveData.ItemKey;
            ItemPosition = Save.Instance.saveData.ItemPosition;

            for (int i = 0; i < ItemKey.Count; i++)
                foreach (var eachItem in itemPrefabs)
                    if (eachItem.GetComponent<Item>()?.Key == ItemKey[i]) {
                        GameObject item = Instantiate(eachItem, ItemPosition[i], Quaternion.identity, transform);
                        if (ItemPosition[i].x == 999 && ItemPosition[i].y == 999) {
                            int count = (int)ItemPosition[i].z;
                            while (InventoryItem.Count <= count)
                                InventoryItem.Add(null);

                            InventoryItem[i] = item;
                        }
                        break;
                    }
        }
        FindObjectOfType<InvenController>().InitInven(InventoryItem);
    }

    public static void Register(GameObject item, Location location = Location.Normal, int count = 0) {
        if (item.layer != LayerMask.NameToLayer("Item")) return;

        item.transform.parent = FindObjectOfType<ItemManager>().transform;
        if(location == Location.Inventory) {
            foreach (var eachRigid in item.GetComponentsInChildren<Rigidbody>())
                eachRigid.isKinematic = true;
            foreach (var eachCollide in item.GetComponentsInChildren<Collider>())
                eachCollide.enabled = false;
            item.transform.position = new Vector3(999, 999, count);
        }
        else {
            foreach (var eachRigid in item.GetComponentsInChildren<Rigidbody>())
                eachRigid.isKinematic = false;
            foreach (var eachCollide in item.GetComponentsInChildren<Collider>())
                eachCollide.enabled = true;
        }
    }

    public List<Vector3> GetItemPosition() {
        ItemPosition.Clear();
        foreach(Transform eachChild in transform) 
            ItemPosition.Add(eachChild.position);
        
        return ItemPosition;
    }
    public List<int> GetItemKey() {
        ItemKey.Clear();
        foreach (Transform eachChild in transform) 
            ItemKey.Add(eachChild.GetComponent<Item>().Key);
        
        return ItemKey;
    }

}
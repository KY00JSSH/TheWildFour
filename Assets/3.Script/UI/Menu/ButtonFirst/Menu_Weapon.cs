using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Weapon : MonoBehaviour, IMenuButton {
    /* 
     * 아이템  
            메소드 이름을 넘겨받으려면 따로 이벤트 다시 건드려야함 => 포기
     1. InvenController useItem(int index) 여기에서 아이템을 사용할 경우 weapon에서 아이템 저장하는 메소드
            1. 담을 list 생성
            2. list count가 2개이상이면 return
            3. 비워있는 곳에 넣을 수 있을까?
     2. 무기라면 아이템 받아와서 표시 및 저장 => 상태창에 사용
     //TODO: 내구도 슬라이드
     3. weapon UI -> 현재 내구도 슬라이드 표시
     4. 아이템의 
     

    ====== 버튼이 눌릴 경우
    1. 무기가 있는지 확인해야함 
    2. 커서는 자동을 왔다갔다...
    3. 

     */
    private Menu_Controll menuControll;
    private InvenController invenController;
    [SerializeField] private GameObject[] WeaponBoxs;
    private InventoryBox inventoryBox;

    // 아이템 담을 공간
    public List<Item> WeaponItem;

    private int CursorCount = 0;
    private void Awake() {
        menuControll = FindObjectOfType<Menu_Controll>();
        invenController = FindObjectOfType<InvenController>();

        WeaponItem = new List<Item>(2);

        inventoryBox = GetComponentInChildren<InventoryBox>();
        invenController.InvenChanged += UpdateWeaponInven;
    }

    private void OnDestroy() {
        invenController.InvenChanged -= UpdateWeaponInven;
    }

    public void I_ButtonOffClick() {
    }

    public void I_ButtonOnClick() {
        WeaponCursorSetting();
    }

    public void ButtonOffClick() {
    }

    public void ButtonOnClick() {
        WeaponCursorSetting();
    }

    private void WeaponCursorSetting() {
        CursorCount++;
        if (CursorCount == 2) {
            CursorCount = 0;
            transform.Find("Weapon_1").GetChild(1).gameObject.SetActive(false);
            transform.Find("Weapon_2").GetChild(1).gameObject.SetActive(true);
        }
        else {
            transform.Find("Weapon_2").GetChild(1).gameObject.SetActive(false);
            transform.Find("Weapon_1").GetChild(1).gameObject.SetActive(true);
        }
    }

    // UI 하단 인벤토리에서 무기 아이템을 사용할 경우 아이템을 들고와야함
    private void UpdateWeaponInven(List<Item> inventory) {
        foreach(Item each in WeaponItem) {
            inventoryBox.UpdateBox(each);
        }
    }

    //TODO: InvenController -> useItem 안에 넣어야함
    public void WeaponItemIn(Item item) {
        // 현재 2개 다 차있으면 리턴
        if (WeaponItem.Count >= 2) return;

        // 없으면 item이 무기인지 확인 후 넣음
        if (item is WeaponItem additem) WeaponItem.Add(additem);
        else return;
    }

    // UI를 어떻게 해야하지 

}

using UnityEngine;

public class PlayerWeaponEquip : MonoBehaviour {
    [SerializeField] private Transform WeaponPoint;
    [SerializeField] private Collider[] fistCollider;

    [SerializeField] private WeaponItemData DEBUG_dummyWeapon;

    private GameObject CurrentEquipWeapon;
    private PlayerAttack playerAttack;
    private MenuWeapon weaponSlot;
    private Vector3 positionOffset;
    private Quaternion rotationOffset;

    public bool isEquip { get { return CurrentEquipWeapon ? true : false; } }

    private void Awake() {
        playerAttack = FindObjectOfType<PlayerAttack>();
        weaponSlot = FindObjectOfType<MenuWeapon>();

        GameObject[] fistObject = GameObject.FindGameObjectsWithTag("PlayerFist");
        fistCollider = new Collider[fistObject.Length];
        for (int i = 0; i < fistObject.Length; i++) 
            fistCollider[i] = fistObject[i].GetComponent<Collider>();
    }

    private WeaponItemData GetCurrentSlotWeapon() {
        // return weaponSlot.getcurrentItem(weaponSlot.CurrentSelectSlot);
        if (isEquip) return null;
            return DEBUG_dummyWeapon;
    }

    private void ChangeEquipWeapon() {
        foreach(Transform weapon in transform) 
            weapon.gameObject.SetActive(false);
        

        if (GetCurrentSlotWeapon()) {   // 현재 무기 슬롯에 무기가 있을 경우
            for (int i = 0; i < transform.childCount; i++) {
                GameObject eachWeaponObject = transform.GetChild(i).gameObject;
                eachWeaponObject.SetActive(false);

                if (eachWeaponObject.TryGetComponent(out WeaponItem eachWeapon)) {
                    
                    // Equipment로 등록된 child Object에서 WeaponItemData를 가져와
                    // 현재 선택된 무기슬롯의 WeaponItemData와 같으면 해당 무기를 출력합니다.
                    if (eachWeapon.weaponItemData == GetCurrentSlotWeapon()) {
                        SetPositionOffset(eachWeapon);  SetRotationOffset(eachWeapon);
                        eachWeaponObject.SetActive(true);

                        CurrentEquipWeapon = eachWeaponObject;  // 현재 장착 무기 설정
                    }
                }
            }
        }
        else CurrentEquipWeapon = null;         // 슬롯이 비었으면 현재 장착 무기 없음으로 설정

        foreach (var fist in fistCollider)      // 무기 장착시 주먹 비활성화
            fist.enabled = !isEquip;
        playerAttack.SetEquip(isEquip);
    }

    private void SetWeaponPosition() {
        if (CurrentEquipWeapon) {
            CurrentEquipWeapon.transform.rotation = Quaternion.Euler(180, 180, 180) * WeaponPoint.rotation * rotationOffset;
            WeaponPoint.localPosition = positionOffset;
            CurrentEquipWeapon.transform.position = WeaponPoint.position;
        }
    }

    private void SetPositionOffset(WeaponItem weapon) {
        switch(weapon.Key) {
            case 13: case 14:
                positionOffset = new Vector3(-0.12f, -0.11f, 0); break;
            case 22: positionOffset = new Vector3(-0.47f, 0, 0); break;     // STICK
            case 24: positionOffset = new Vector3(-0.1f, -0.05f, 0); break; // BOW
            case 33: positionOffset = new Vector3(-0.2f, -0.08f, -0.15f); break; // UP AXE
            case 34: positionOffset = new Vector3(-0.15f, -0.05f, -0.2f); break; // UP PICK
        }
    }
    private void SetRotationOffset(WeaponItem weapon) {
        switch (weapon.Key) {
            case 22: rotationOffset = Quaternion.Euler(0, 0, 100); break; // STICK
            case 24: rotationOffset = Quaternion.Euler(90, 90, 180); break; // BOW
            case 34: rotationOffset = Quaternion.Euler(0, 0, 180); break; // BOW
            default: rotationOffset = Quaternion.identity; break;
        }
    }
    
    private void Update() {
        SetWeaponPosition();

        if (Input.GetKeyDown(KeyCode.X)) {
            ChangeEquipWeapon();
        }
    }

}
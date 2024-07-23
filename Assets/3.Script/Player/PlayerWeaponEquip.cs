using UnityEngine;

public class PlayerWeaponEquip : MonoBehaviour {
    [SerializeField] private Transform WeaponPoint;

    [SerializeField] private WeaponItemData DEBUG_dummyWeapon;

    private GameObject CurrentEquipWeapon;
    private PlayerAttack playerAttack;
    private MenuWeapon weaponSlot;
    
    public bool isEquip { get { return CurrentEquipWeapon ? true : false; } }

    private void Awake() {
        playerAttack = FindObjectOfType<PlayerAttack>();
        weaponSlot = FindObjectOfType<MenuWeapon>();
    }

    private WeaponItemData GetCurrentSlotWeapon() {
        // return weaponSlot.getcurrentItem(weaponSlot.CurrentSelectSlot);
        return DEBUG_dummyWeapon;
    }

    private void ChangeEquipWeapon() {
        CurrentEquipWeapon = null;      // 현재 장착 무기 없음으로 설정

        if (GetCurrentSlotWeapon()) {   // 현재 무기 슬롯에 무기가 있을 경우
            for (int i = 0; i < transform.childCount; i++) {
                GameObject eachWeaponObject = transform.GetChild(i).gameObject;
                eachWeaponObject.SetActive(false);

                if (eachWeaponObject.TryGetComponent(out WeaponItem eachWeapon)) {
                    // Equipment로 등록된 child Object에서 WeaponItemData를 가져와
                    // 현재 선택된 무기슬롯의 WeaponItemData와 같으면 해당 무기를 출력합니다.
                    if (eachWeapon.WeaponItemData == GetCurrentSlotWeapon()) {
                        eachWeaponObject.SetActive(true);
                        CurrentEquipWeapon = eachWeaponObject;
                    }

                }
            }
        }
        playerAttack.SetEquip(isEquip);
    }

    private void SetWeaponPosition() {
        if (CurrentEquipWeapon) {
            Transform playerHand = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().
                GetBoneTransform(HumanBodyBones.RightHand);
            WeaponPoint = playerHand;
            CurrentEquipWeapon.transform.position = WeaponPoint.position;
            CurrentEquipWeapon.transform.rotation = Quaternion.Euler(0, 180, 0) * WeaponPoint.rotation;

        }
    }

    private void Update() {
        SetWeaponPosition();

        if (Input.GetKeyDown(KeyCode.X)) {
            ChangeEquipWeapon();
        }
    }
}
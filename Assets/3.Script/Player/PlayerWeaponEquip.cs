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
        CurrentEquipWeapon = null;      // ���� ���� ���� �������� ����

        if (GetCurrentSlotWeapon()) {   // ���� ���� ���Կ� ���Ⱑ ���� ���
            for (int i = 0; i < transform.childCount; i++) {
                GameObject eachWeaponObject = transform.GetChild(i).gameObject;
                eachWeaponObject.SetActive(false);

                if (eachWeaponObject.TryGetComponent(out WeaponItem eachWeapon)) {
                    // Equipment�� ��ϵ� child Object���� WeaponItemData�� ������
                    // ���� ���õ� ���⽽���� WeaponItemData�� ������ �ش� ���⸦ ����մϴ�.
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
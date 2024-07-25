using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkshopBtn : MonoBehaviour {

    public ItemData itemData;

    public bool isWeap = false;
    public bool isMedic = false;

    public WeaponItemData weaponItem;
    public MedicItemData medicItem;

    private WorkshopManager workshopManager;

    // 버튼 눌리면 아이템 제작 메소드 사용
    private ButtonCoolTimeUI buttonCoolTimeUI;
    private InvenController invenController;

    private void Awake() {
        workshopManager = FindObjectOfType<WorkshopManager>();
        buttonCoolTimeUI = GetComponent<ButtonCoolTimeUI>();
        invenController = FindObjectOfType<InvenController>();
        CheckItemDataType();
    }

    private void OnEnable() {
        CheckLevel();
    }
    private void Update() {
        // 버튼 눌리면 쿨타임 확인해서 아이템 제작 메소드 사용
        if (buttonCoolTimeUI.isBuildComplete) {
            buttonCoolTimeUI.isBuildComplete = false;
            useCraftItemUseMain();
        }

    }

    // InvenController 아이템 제작 메소드 사용
    private void useCraftItemUseMain() {
        Debug.Log(itemData.name);
        invenController.craftItemUseMain(itemData);
    }



    private void CheckItemDataType() {
       if(itemData is WeaponItemData weaponItemData) {
            isWeap = true; isMedic = false;
            weaponItem = weaponItemData;
            medicItem = null;

        }
       else if(itemData is MedicItemData medicItemData) {
            isWeap = false; isMedic = true;
            weaponItem = null;
            medicItem = medicItemData;
        }

    }

    private void CheckLevel() {
        if (itemData is WeaponItemData weapon) {
            if (weapon.Level <= workshopManager.WorkshopLevel) {
                transform.GetChild(2).gameObject.SetActive(false);
                transform.GetComponent<Button>().enabled = true;
            }
            else {
                transform.GetChild(2).gameObject.SetActive(true);
                transform.GetComponent<Button>().enabled = false;

            }
        }
        else if (itemData is MedicItemData medic) {
            if (medic.Level <= workshopManager.WorkshopLevel) {
                transform.GetChild(2).gameObject.SetActive(false);
                transform.GetComponent<Button>().enabled = true;
            }
            else {
                transform.GetChild(2).gameObject.SetActive(true);
                transform.GetComponent<Button>().enabled = false;

            }
        }
    }

}

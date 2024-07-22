using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkshopBtn : MonoBehaviour {

    public ItemData iTemData;

    public bool isWeap = false;
    public bool isMedic = false;

    public WeaponItemData weaponItem;
    public MedicItemData medicItem;

    private WorkshopManager workshopManager;

    private void Awake() {
        workshopManager = FindObjectOfType<WorkshopManager>();
        CheckItemDataType();
        Debug.Log("weaponitemdata" + weaponItem);
        Debug.Log("medicItemdata" + medicItem);
    }

    private void OnEnable() {
        CheckLevel();
    }


    private void CheckItemDataType() {
       if( iTemData is WeaponItemData weaponItemData) {
            isWeap = true; isMedic = false;
            weaponItem = weaponItemData;
            medicItem = null;

        }
       else if(iTemData is MedicItemData medicItemData) {
            isWeap = false; isMedic = true;
            weaponItem = null;
            medicItem = medicItemData;
        }

    }

    private void CheckLevel() {
        if (iTemData is WeaponItemData weapon) {
            if (weapon.Level <= workshopManager.WorkshopLevel) {
                transform.GetChild(2).gameObject.SetActive(false);
                transform.GetComponent<Button>().enabled = true;
            }
            else {
                transform.GetChild(2).gameObject.SetActive(true);
                transform.GetComponent<Button>().enabled = false;

            }
        }
        else if (iTemData is MedicItemData medic) {
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

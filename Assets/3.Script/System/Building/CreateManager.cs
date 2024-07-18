using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateManager : MonoBehaviour {
    private BuildingCreate[] buildingCreates;
    public GameObject BuildImg;

    private void Awake() {
        buildingCreates = GetComponentsInChildren<BuildingCreate>();
    }

    public void CancelAllBuildings() {
        BuildImg.SetActive(false);
        foreach (BuildingCreate each in buildingCreates) {
            each.isBuild = false;
            each.CancelBuilding();
        }
        
    }

}

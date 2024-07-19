using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateManager : MonoBehaviour {
    private BuildingCreate[] buildingCreates;

    private void Awake() {
        buildingCreates = GetComponentsInChildren<BuildingCreate>();
    }

    public void CancelAllBuildings() {
        foreach (BuildingCreate each in buildingCreates) {
            if (!each.isExist)
            each.CancelBuilding();
        }
    }
}

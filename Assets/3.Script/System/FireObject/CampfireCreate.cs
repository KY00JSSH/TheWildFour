using UnityEngine;
using System.Collections.Generic;

public class CampfireCreate : BuildingCreate {
    private CampfireManager campfireManager;
    private Animator campfireAnimator;

    private GameObject newCampfire;

    protected override void Awake() {
        base.Awake();
        campfireManager = GetComponent<CampfireManager>();
        
    }

    public override GameObject Building {
        get { return newCampfire; }
    }

    public override void BuildMode() {
        if (newCampfire == null)
            newCampfire = Instantiate(buildingPrefabs[0]);

        buildingColliders = Building.GetComponentsInChildren<Collider>();
        base.BuildMode();
    }

    public override void CreateBuilding() {
        buildingColliders = Building.GetComponentsInChildren<Collider>();
        base.CreateBuilding();
        newCampfire.GetComponent<Animator>().SetTrigger("Create");
    }

    public override void CancelBuilding() {
        base.CancelBuilding();
    }

}

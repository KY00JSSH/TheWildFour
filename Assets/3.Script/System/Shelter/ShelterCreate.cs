using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelterCreate : BuildingCreate {
    private ShelterManager shelterManager;
    private Animator shelterAnimator;

    //TODO: UI > 거처 메뉴 버튼 OnClicked => BuildShelter();
    //TODO: UI > 거처 내부 버튼 '짐싸기' OnClicked => DestroyShelter();

    protected override void Awake() {
        base.Awake();
        shelterAnimator = GetComponent<Animator>();
        shelterManager = GetComponent<ShelterManager>();
    }

    protected override GameObject Building {
        get { return buildingPrefabs[shelterManager.ShelterLevel]; }
    }

    public override void CreateBuilding() {
        base.CreateBuilding();
        shelterAnimator.SetTrigger("Create");
    }

}

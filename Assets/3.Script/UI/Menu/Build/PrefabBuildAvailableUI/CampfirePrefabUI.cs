using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfirePrefabUI : BuildPrefabUI {
    [SerializeField]private CampfireChestCreate campfireCreate;
    protected override void Awake() {
        base.Awake();
    }

    protected override void Update() {
        if (buildingObj != null) {
            isValid = campfireCreate.isValidBuild;
            base.Update();
        }
    }

    public void BuildAvaildableMode() {
        if (!tooltip_Build.isBuildAvailable) return;

        if (campfireCreate.isExist) return;

        buildingObj = campfireCreate.Building;
        StartCoroutine(FindObject());
    }
    protected IEnumerator FindObject()
    {
        while (buildingObj == null)
        {
            buildingObj = campfireCreate.Building;
            yield return null;
        }

        Debug.Log("==========================" + buildingObj.name);
        isBuiltStart = true;
        BuildImg.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponItemData", menuName = "ScriptableObjects/WeaponItemData", order = 6)]
public class WeaponItemData : EquipItemData {
    [SerializeField] private float maxPowerPoint;        //최대 공격력
    [SerializeField] private float minPowerPoint;        //최소 공격력

    public float getPowerPoint() {
        return Random.Range(minPowerPoint, maxPowerPoint);
    }
}

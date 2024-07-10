using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponItemData", menuName = "ScriptableObjects/WeaponItemData", order = 6)]
public class WeaponItemData : EquipItemData {
    [SerializeField] private float maxPowerPoint;        //�ִ� ���ݷ�
    [SerializeField] private float minPowerPoint;        //�ּ� ���ݷ�

    public float getPowerPoint() {
        return Random.Range(minPowerPoint, maxPowerPoint);
    }
}

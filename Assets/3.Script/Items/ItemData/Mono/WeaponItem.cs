using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : EquipItem
{
    public WeaponItemData WeaponItemData;

    public float getPowerPoint() {
        return Random.Range(WeaponItemData.MinPowerPoint, WeaponItemData.MaxPowerPoint);
    }
}

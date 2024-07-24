using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : EquipItem
{
    public WeaponItemData weaponItemData;

    public float getPowerPoint() {
        return Random.Range(weaponItemData.MinPowerPoint, weaponItemData.MaxPowerPoint);
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "WeaponItemData", menuName = "Items/WeaponItemData", order = 6)]
public class WeaponItemData : EquipItemData {
    [SerializeField] private float minPowerPoint;        //최소 공격력
    [SerializeField] private float maxPowerPoint;        //최대 공격력

    public float MinPowerPoint { get { return minPowerPoint; } }
    public float MaxPowerPoint { get { return maxPowerPoint; } }
}

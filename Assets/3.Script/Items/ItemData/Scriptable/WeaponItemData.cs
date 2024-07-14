using UnityEngine;

[CreateAssetMenu(fileName = "WeaponItemData", menuName = "Items/WeaponItemData", order = 6)]
public class WeaponItemData : EquipItemData {
    [SerializeField] private float minPowerPoint;        //�ּ� ���ݷ�
    [SerializeField] private float maxPowerPoint;        //�ִ� ���ݷ�

    public float MinPowerPoint { get { return minPowerPoint; } }
    public float MaxPowerPoint { get { return maxPowerPoint; } }
}

using UnityEngine;

[CreateAssetMenu(fileName = "WeaponItemData", menuName = "Items/WeaponItemData", order = 6)]
public class WeaponItemData : EquipItemData {
    [SerializeField] private float maxPowerPoint;        //�ִ� ���ݷ�
    [SerializeField] private float minPowerPoint;        //�ּ� ���ݷ�

    public float getPowerPoint() {
        return Random.Range(minPowerPoint, maxPowerPoint);
    }
}

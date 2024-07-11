using UnityEngine;

[CreateAssetMenu(fileName = "ArmorItemData", menuName = "Items/ArmorItemData", order = 7)]
public class ArmorItemData : EquipItemData {
    [SerializeField] private float shieldPoint;      //����

    public float ShieldPoint { get { return shieldPoint; } }
}

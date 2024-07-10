using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArmorItemData", menuName = "ScriptableObjects/ArmorItemData", order = 7)]
public class ArmorItemData : EquipItemData {
    [SerializeField] private float shieldPoint;      //����

    public float ShieldPoint { get { return shieldPoint; } }
}

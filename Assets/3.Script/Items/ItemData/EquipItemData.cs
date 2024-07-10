using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipItemData", menuName = "ScriptableObjects/EquipItemData", order = 4)]
public class EquipItemData : ItemData {
    [SerializeField] private float durability;      //������
    [SerializeField] private EquipType eqType;      //���� Ÿ��
    [SerializeField] private int level;             //���� ����

    public float Durability { get { return durability; } }
    public EquipType EqType {  get { return eqType;  } }
    public int Level { get { return level; } }
}

public enum EquipType {
    PICK,
    AXE,
    SHIRT,
    SHOO,
    WEAP
}
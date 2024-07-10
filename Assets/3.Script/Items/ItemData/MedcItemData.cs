using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MedcItemData", menuName = "ScriptableObjects/MedcItemData", order = 5)]
public class MedcItemData : ItemData {
    [SerializeField] private float healPoint;        //ȸ����

    public float HealPoint { get { return healPoint; } }
}

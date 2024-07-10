using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FoodItemData", menuName = "ScriptableObjects/FoodItemData", order = 3)]
public class FoodItemData : CountableItemData {
    [SerializeField] private float fullPoint;   //������ ����Ʈ
    [SerializeField] private float healPoint;   //ȸ�� ����Ʈ
    [SerializeField] private float decayTime;   //���� �ð�

    public float FullPoint { get { return fullPoint; } }
    public float HealPoint { get { return healPoint; } }
    public float DecayTime { get { return decayTime; } }
}
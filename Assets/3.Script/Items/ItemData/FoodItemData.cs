using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FoodItemData", menuName = "ScriptableObjects/FoodItemData", order = 3)]
public class FoodItemData : CountableItemData {
    [SerializeField] private float fullPoint;   //포만감 포인트
    [SerializeField] private float healPoint;   //회복 포인트
    [SerializeField] private float decayTime;   //부패 시간

    public float FullPoint { get { return fullPoint; } }
    public float HealPoint { get { return healPoint; } }
    public float DecayTime { get { return decayTime; } }
}

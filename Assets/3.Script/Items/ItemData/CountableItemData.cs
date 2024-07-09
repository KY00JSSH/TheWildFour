using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CountableItemData", menuName = "ScriptableObjects/CountableItemData", order = 2)]
public class CountableItemData : ItemData
{
    [SerializeField] private int maxCount;  //최대 겹치는 개수

    public int MaxCount { get { return maxCount; } }
}

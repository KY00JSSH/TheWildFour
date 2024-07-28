using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckSelectedLoad : MonoBehaviour {
    public bool isSelected;
    public int count;

    public void Select() { 
        isSelected = true;
        foreach (var each in GetComponentsInChildren<Text>()) {
            each.color = Color.yellow;
        }
    }
}
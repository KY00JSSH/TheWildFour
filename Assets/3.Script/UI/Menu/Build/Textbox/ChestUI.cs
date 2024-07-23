using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestUI : UIInfo {

    static public bool isChestUIOpen { get { return _isChestUIOpen; } }
    static private bool _isChestUIOpen = false;
    protected override void OnEnable() {
        base.OnEnable();
        _isChestUIOpen = true;
    }
    protected override void OnDisable() {
        _isChestUIOpen = false;
        base.OnDisable();
    }
}

/*
 1. level up 상자 이미지 풀려야함
    - level up 1번만 있음 
 */
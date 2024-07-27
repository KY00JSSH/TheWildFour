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
 1. level up ���� �̹��� Ǯ������
    - level up 1���� ���� 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlFixer : MonoBehaviour {
    private void Awake() {
        gameObject.AddComponent<CameraControl>();
    }
}
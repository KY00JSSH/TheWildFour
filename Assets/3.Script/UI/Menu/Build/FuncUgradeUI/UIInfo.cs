using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInfo : MonoBehaviour {
    [SerializeField] protected GameObject menuButton;

    protected virtual void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) Escape();
    }

    protected virtual void OnEnable() {
        menuButton.SetActive(false);
    }
    protected virtual void OnDisable() {
        menuButton.SetActive(true);
    }

    public virtual void Escape() {
        menuButton.SetActive(true);
        transform.gameObject.SetActive(false);
    }
}

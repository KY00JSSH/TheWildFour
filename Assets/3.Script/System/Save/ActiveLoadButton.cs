using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveLoadButton : MonoBehaviour {

    private void OnEnable() {
        GetComponent<Button>().interactable = false;
        GetComponentInChildren<Text>().color = Color.gray;
    }
    public void Activate() {
        GetComponent<Button>().interactable = true;
        GetComponentInChildren<Text>().color= Color.white;
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    private PlayerType playerType;
    private void Awake() {
        playerType = Save.Instance.saveData.playerType;
        foreach (Transform child in transform) {
            if (child.name == playerType.ToString())
                child.gameObject.SetActive(true);
            else
                child.gameObject.SetActive(false);
        }
    }

}
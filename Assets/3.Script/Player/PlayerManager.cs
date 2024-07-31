using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    public PlayerType playerType;
    private void Awake() {
        ActivatePlayer();
    }


    public GameObject ActivatePlayer() {
        GameObject player = null;
        playerType = Save.Instance.saveData.playerType;
        foreach (Transform child in transform) {
            if (child.name == playerType.ToString()) {
                player = child.gameObject;
                child.gameObject.SetActive(true);
            }
            else
                child.gameObject.SetActive(false);
        }
        return player;
    }
}
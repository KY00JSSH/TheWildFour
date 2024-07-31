using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelect : MonoBehaviour {
    private bool[] playerSelect = new bool[4];
    private PlayerSelectInfo[] players;
    [SerializeField] Animator[] playersAnimator;
    private float[] playerTargetSpeed = new float[4];
    
    private void Awake() {
        players = GetComponentsInChildren<PlayerSelectInfo>();
    }

    private void Update() {
        for(int i = 0; i < playerTargetSpeed.Length; i++) {
            playersAnimator[i].SetFloat("MoveSpeed", 
                Mathf.Lerp(playersAnimator[i].GetFloat("MoveSpeed"), playerTargetSpeed[i], Time.deltaTime * 1.5f));
        }
    }

    public void Select(int player) {
        for (int i = 0; i < playerSelect.Length; i++) {
            if (i == player) {
                playerSelect[player] = true;
                players[player].TooltipShow();
                playerTargetSpeed[player] = 1;
            }
            else {
                playerSelect[i] = false;
                players[i].TooltipHide();
                playerTargetSpeed[i] = 0;
            }
        }
        
        Save.Instance.saveData.playerType = (PlayerType)player;
        if ((PlayerType)player == PlayerType.Ju)
            Save.Instance.saveData.playerInvenCount = 10;
        else
            Save.Instance.saveData.playerInvenCount = 8;
    }

}

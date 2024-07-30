using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelect : MonoBehaviour {
    private bool[] playerSelect = new bool[4];
    private PlayerSelectInfo[] players;

    private void Awake() {
        players = GetComponentsInChildren<PlayerSelectInfo>();
    }

    public void Select(int player) {
        for (int i = 0; i < playerSelect.Length; i++) {
            playerSelect[i] = false;
            players[i].TooltipHide();
        }
        playerSelect[player] = true;
        players[player].TooltipShow();
        
        Save.Instance.saveData.playerType = (PlayerType)player;
        if ((PlayerType)player == PlayerType.Ju)
            Save.Instance.saveData.playerInvenCount = 10;
        //TODO: 강화배낭 수치 변경으로 해야함
        else
            Save.Instance.saveData.playerInvenCount = 8;
    }

}
using UnityEngine;

public class PlayerAbility : MonoBehaviour {
     public float playerAttack { get; private set; }
    public float playerDefense { get; private set; }
    public float playerGather { get; private set; }
    public float playerSpeed { get; private set; }
    public float playerInvenCount { get; private set; }
    public float playerAddInvenCount { get; private set; }

    public void SetPlayerAttack(float attack) {
        playerAttack += attack;
    }
    public void SetPlayerDefense(float defense) {
        playerDefense += defense;
    }
    public void GetPlayerGather(float gather) {
        playerGather += gather;
    }
    public void GetPlayerSpeed(float speed) { 
        playerSpeed += speed;
    }
    public void AddPlayerInven() {
        playerInvenCount++;
    }

}

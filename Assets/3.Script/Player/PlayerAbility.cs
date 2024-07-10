using UnityEngine;

public class PlayerAbility : MonoBehavior {
    public static PlayerAbility Instance = null;

    private void Awake() {
        if(Instance == null) { 
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    //TODO: 굳이 싱글톤을 써야할 지 고민해보기
    
    private float playerAttack { get; private set; }
    private float playerDefense { get; private set; }
    private float playerGather { get; private set; }
    private float playerSpeed { get; private set; }
    private float playerInvenCount { get; private set; }
    private float playerAddInvenCount { get; private set; }

    public float SetPlayerAttack(float attack) {
        playerAttack += attack;
    }
    public float SetPlayerDefense(float defense) {
        playerDefense += defense;
    }
    public float GetPlayerGather(float gather) {
        playerGather += gather;
    }
    public float GetPlayerSpeed(float speed) { 
        playerSpeed += speed;
    }
    public float AddPlayerInven() {
        playerInvenCount++;
    }

    


}

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
    
    private float playerAttack;
    private float playerDefense;
    private float playerGather;
    private float playerSpeed;
    private float playerInvenCount;
    private float playerAddInvenCount;

    public float GetPlayerAttack() {
        return playerAttack;
    }
    public float GetPlayerDefense() {
        return playerDefense;
    }
    public float GetPlayerGather() {
        return playerGather;
    }
    public float GetPlayerSpeed() { 
        return playerSpeed;
    }
    public float GetPlayerInvenCount() {
        return playerInvenCount;
    }

    


}

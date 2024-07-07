using UnityEngine;

public class PlayerStatus : MonoBehaviour {
    private float defaultHp = 100, defaultHunger = 100, defaultWarm = 100;
    private float PlayerHp, PlayerHunger, PlayerWarm;

    private void Start() {
        PlayerHp = defaultHp;
        PlayerHunger = defaultHunger;
        PlayerWarm = defaultWarm;
    }

    private void Update() {
        
    }

}
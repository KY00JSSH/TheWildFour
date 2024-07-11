using UnityEngine;

public class PlayerAbility : MonoBehaviour {
    private ShelterManager shelterManager;
    private PlayerStatus playerStatus;

    // 플레이어 기본 능력치
    // 플레이어 선택 시에만 변경됨
    private float playerAttack;
    private float playerDefense;
    private float playerGather;
    private float playerSpeed;
    private float playerDashSpeed;
    private float playerInvenCount;

    // 플레이어 추가 능력치
    // 플레이어 스킬 또는 장비에 따라 변경됨
    private float playerAddAttack;
    private float playerAddDefense;
    private float playerAddGather;
    private float playerAddSpeed;
    private float playerAddDashSpeed;
    private float playerAddInvenCount;

    public void SetPlayerAttack(float attack) { playerAttack += attack; }
    public void SetPlayerDefense(float defense) { playerDefense += defense; }
    public void SetPlayerGather(float gather) { playerGather += gather; }
    public void SetPlayerSpeed(float speed) { playerSpeed += speed; }
    public void AddPlayerInven() { playerInvenCount++; }

    private void Awake() {
        shelterManager = FindObjectOfType<ShelterManager>();
        playerStatus = FindObjectOfType<PlayerStatus>();
    }

    private void Start() {
        //TODO: SAVE 구현 시 JSON에서 받아오기
        playerAttack = 2f;
        playerDefense = 2f;
        playerGather = 2f;
        playerSpeed = 1f;
        playerDashSpeed = 2.5f;
        playerInvenCount = 8;

        playerAddAttack = 0;
        playerAddDefense = 0;
        playerAddGather = 0;
        playerAddSpeed = 0;
        playerAddDashSpeed = 0;
        playerAddInvenCount = 0;
    }

    public void UpdateAblity() {
        playerAddAttack = 
            shelterManager.skillAttack[0].GetValue();

        playerStatus.PlayerMaxHp =
            playerStatus.PlayerMaxHp + shelterManager.skillMove[6].GetValue();



    }

}

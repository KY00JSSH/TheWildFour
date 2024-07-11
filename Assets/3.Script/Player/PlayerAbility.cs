using UnityEngine;

public class PlayerAbility : MonoBehaviour {
    private ShelterManager shelterManager;
    private CameraControl2 cameraControl;
    private PlayerStatus playerStatus;
    private PlayerMove playerMove;

    // 플레이어 기본 능력치
    // 플레이어 선택 시에만 변경됨
    private float playerAttack;
    private float playerCriticalAttack;
    private float playerDefense;
    private float playerGather;
    private float playerSpeed;
    private float playerDashSpeed;
    private float playerDecDashGage;
    private float playerInvenCount;

    // 플레이어 추가 능력치
    // 플레이어 스킬 또는 장비에 따라 변경됨
    private float playerAddAttack;
    private float playerAddDefense;
    private float playerAddGather;
    private float playerAddSpeed;
    private float playerAddDashSpeed;
    private float playerAddDecDashGage;
    private float playerAddInvenCount;

    /*
    public void SetPlayerAttack(float attack) { playerAttack += attack; }
    public void SetPlayerDefense(float defense) { playerDefense += defense; }
    public void SetPlayerGather(float gather) { playerGather += gather; }
    public void SetPlayerSpeed(float speed) { playerSpeed += speed; }
    public void AddPlayerInven() { playerInvenCount++; }
    */

    private void Awake() {
        shelterManager = FindObjectOfType<ShelterManager>();
        cameraControl = FindObjectOfType<CameraControl2>();
        playerStatus = FindObjectOfType<PlayerStatus>();
        playerMove = FindObjectOfType<PlayerMove>();
    }

    private void Start() {
        //TODO: SAVE 구현 시 JSON에서 받아오기
        playerAttack = 2f;
        playerCriticalAttack = 5f;

        playerDefense = 2f;
        playerGather = 2f;
        playerSpeed = 1f;
        playerDashSpeed = 2.5f;
        playerDecDashGage = 8f;
        playerInvenCount = 8;

        playerAddAttack = 0;
        playerAddDefense = 0;
        playerAddGather = 0;
        playerAddSpeed = 0;
        playerAddDashSpeed = 0;
        playerAddDecDashGage = 0;
        playerAddInvenCount = 0;
    }

    public void UpdateAblity() {
        playerMove.isSkilled = 
            shelterManager.GetSkill("전력 질주").nowSkillLevel == 1 ? true : false;
        playerAddSpeed = shelterManager.GetSkill("속도").GetValue();
        playerMove.SetPlayerMoveSpeed(playerSpeed + playerAddSpeed);
        cameraControl.maxFOV =
            shelterManager.GetSkill("시야 반경").nowSkillLevel == 1 ? 110f : 100f;
        playerAddDecDashGage =
            shelterManager.GetSkill("전력 질주 시간").GetValue();
        playerMove.DecDashGage = playerDecDashGage - playerAddDecDashGage;
        playerStatus.PlayerMaxHp =
            playerStatus.PlayerMaxHp + shelterManager.GetSkill("운동").GetValue();

        playerAddAttack =       //TODO: 장착 장비의 공격력도 가져오기
            shelterManager.GetSkill("근접 공격력").GetValue() +
            shelterManager.GetSkill("원거리 공격력").GetValue();
       
        //playerAttackSpeed =
        //    playerAttackSpeed + shelterManager.GetSkill("공격 속도").GetValue();
        
    }

    public float GetTotalPlayerAttack() { return playerAttack + playerAddAttack; }

}

using UnityEngine;

public class PlayerAbility : MonoBehaviour {
    private ShelterManager shelterManager;
    private CameraControl cameraControl;
    private PlayerStatus playerStatus;
    private PlayerMove playerMove;

    // 플레이어 기본 능력치
    // 플레이어 선택 시에만 변경됨
    private float playerAttack;
    private float playerAttackSpeed;
    private float playerCriticalAttack;
    private float playerCriticalChance;
    private float playerColdResistance;
    private float playerDefense;
    private float playerGather;
    private float playerSpeed;
    private float playerDashSpeed;
    private float playerDecDashGage;
    private float playerInvenCount;

    // 플레이어 추가 능력치
    // 플레이어 스킬 또는 장비에 따라 변경됨
    private float playerAddAttack;
    private float playerAddAttackSpeed;
    private float playerAddCriticalAttack;
    private float playerAddCriticalChance;
    private float playerAddColdResistance;
    private float playerAddDefense;
    private float playerAddGather;
    private float playerAddSpeed;
    private float playerAddDashSpeed;
    private float playerAddDecDashGage;
    private float playerAddInvenCount;

    private void Awake() {
        shelterManager = FindObjectOfType<ShelterManager>();
        playerStatus = FindObjectOfType<PlayerStatus>();
        playerMove = FindObjectOfType<PlayerMove>();

        //TODO: SAVE 구현 시 JSON에서 받아오기
        playerAttack = 2f;
        playerAttackSpeed = 1f;
        playerCriticalAttack = 5f;
        playerCriticalChance = 0.1f;
        playerColdResistance = 0f;
        playerDefense = 2f;
        playerGather = 2f;
        playerSpeed = 1f;
        playerDashSpeed = 2.5f;
        playerDecDashGage = 8f;
        playerInvenCount = 8;

        playerAddAttack = 0;
        playerAddAttackSpeed = 0;
        playerAddCriticalAttack = 0;
        playerAddCriticalChance = 0;
        playerAddColdResistance = 0;
        playerAddDefense = 0;
        playerAddGather = 0;
        playerAddSpeed = 0;
        playerAddDashSpeed = 0;
        playerAddDecDashGage = 0;
        playerAddInvenCount = 0;
    }

    private void Start() {
        cameraControl = FindObjectOfType<CameraControl>();
    }

    private void Update() {
        //if(ShelterManager.)
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

        playerAddAttack =
            shelterManager.GetSkill("근접 공격력").GetValue() +
            shelterManager.GetSkill("원거리 공격력").GetValue() +
            PlayerWeaponEquip.CurrentEquipWeaponAttackPoint;
        //TODO: 플레이어 공격 구현 후 공격속도 적용
        playerAddAttackSpeed = shelterManager.GetSkill("공격 속도").GetValue();
        FindObjectOfType<PlayerAttack>().SetAttackSpeed(GetTotalPlayerAttackSpeed());

        playerAddCriticalAttack = shelterManager.GetSkill("치명타 공격력").GetValue();
        playerAddCriticalChance = shelterManager.GetSkill("치명타 공격 확률").GetValue();

        playerAddInvenCount = shelterManager.GetSkill("강화 배낭").GetValue();
        if(playerInvenCount != playerAddInvenCount) {
            InvenUIController invenUIController = FindObjectOfType<InvenUIController>();
            for (int i = 0; i < playerInvenCount - playerAddInvenCount; i++)
                invenUIController.InvenCountUpgrade();
            playerInvenCount = playerAddInvenCount;
        }
        playerAddGather = 
            shelterManager.GetSkill("도끼의 달인").GetValue() +
            shelterManager.GetSkill("곡괭이의 달인").GetValue();
    }

    public float GetTotalPlayerAttack() { return playerAttack + playerAddAttack; }
    public float GetTotalPlayerCriticalAttack() { return playerCriticalAttack + playerAddCriticalAttack; }
    public float GetTotalPlayerCriticalChance() { return playerCriticalChance + playerAddCriticalChance; }

    public float GetTotalPlayerColdResistance() { return playerColdResistance + playerAddColdResistance; }
    public float GetTotalPlayerDefense() { return playerDefense + playerAddDefense; }
    public float GetTotalPlayerSpeed() {  return playerSpeed + playerAddSpeed; }

    public float GetTotalPlayerAttackSpeed() { return playerAttackSpeed + playerAddAttackSpeed; }
    public float GetTotalPlayerGather() { return playerGather + playerAddGather; }
}

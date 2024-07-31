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

        playerAttack = Save.Instance.saveData.playerAttack;
        playerAttackSpeed = Save.Instance.saveData.playerAttackSpeed;
        playerCriticalAttack = Save.Instance.saveData.playerCriticalAttack;
        playerCriticalChance = Save.Instance.saveData.playerCriticalChance;
        playerColdResistance = Save.Instance.saveData.playerColdResistance;
        playerDefense = Save.Instance.saveData.playerDefense;
        playerGather = Save.Instance.saveData.playerGather;
        playerSpeed = Save.Instance.saveData.playerSpeed;
        playerDashSpeed = Save.Instance.saveData.playerDashSpeed;
        playerDecDashGage = 5f;
        playerInvenCount = Save.Instance.saveData.playerInvenCount;

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
        playerAddInvenCount = playerInvenCount == 8 ? 8 : playerInvenCount;
    }

    private void Start() {
        cameraControl = FindObjectOfType<CameraControl>();
        UpdateAblity();
    }

    public void UpdateAblity() {
        playerMove.isSkilled = 
            shelterManager.GetSkill("전력 질주").nowSkillLevel == 1 ? true : false;
        
        playerAddSpeed = shelterManager.GetSkill("속도").GetValue();
        playerMove.SetPlayerMoveSpeed(playerSpeed + playerAddSpeed);
        cameraControl.maxZoom =
            shelterManager.GetSkill("시야 반경").nowSkillLevel == 1 ? 15f : 13f;
        playerAddDecDashGage =
            shelterManager.GetSkill("전력 질주 시간").GetValue();
        playerMove.DecDashGage = playerDecDashGage - playerAddDecDashGage;
        playerStatus.SetPlayerMaxHp(
            playerStatus.GetPlayerMaxHp() + shelterManager.GetSkill("운동").GetValue());

        playerAddAttack =
            shelterManager.GetSkill("근접 공격력").GetValue() +
            shelterManager.GetSkill("원거리 공격력").GetValue() +
            PlayerWeaponEquip.CurrentEquipWeaponAttackPoint;
        playerAddAttackSpeed = shelterManager.GetSkill("공격 속도").GetValue();
        playerMove.GetComponent<PlayerAttack>().SetAttackSpeed(GetTotalPlayerAttackSpeed());

        playerAddCriticalAttack = shelterManager.GetSkill("치명타 공격력").GetValue();
        playerAddCriticalChance = shelterManager.GetSkill("치명타 공격 확률").GetValue();

        playerAddInvenCount = 8 + shelterManager.GetSkill("강화 배낭").GetValue();
        if(playerInvenCount != playerAddInvenCount) {
            InvenUIController invenUIController = FindObjectOfType<InvenUIController>();
            for (int i = 0; i < playerAddInvenCount - playerInvenCount; i++)
                invenUIController.InvenCountUpgrade();
            playerAddInvenCount = playerInvenCount;
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

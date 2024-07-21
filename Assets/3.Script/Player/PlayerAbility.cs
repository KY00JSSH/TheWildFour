using UnityEngine;

public class PlayerAbility : MonoBehaviour {
    private ShelterManager shelterManager;
    private CameraControl cameraControl;
    private PlayerStatus playerStatus;
    private PlayerMove playerMove;

    // �÷��̾� �⺻ �ɷ�ġ
    // �÷��̾� ���� �ÿ��� �����
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

    // �÷��̾� �߰� �ɷ�ġ
    // �÷��̾� ��ų �Ǵ� ��� ���� �����
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
        cameraControl = FindObjectOfType<CameraControl>();
        playerStatus = FindObjectOfType<PlayerStatus>();
        playerMove = FindObjectOfType<PlayerMove>();
    }

    private void Start() {
        //TODO: SAVE ���� �� JSON���� �޾ƿ���
        playerAttack = 2f;
        playerAttackSpeed = 3f;
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

    public void UpdateAblity() {
        playerMove.isSkilled = 
            shelterManager.GetSkill("���� ����").nowSkillLevel == 1 ? true : false;
        playerAddSpeed = shelterManager.GetSkill("�ӵ�").GetValue();
        playerMove.SetPlayerMoveSpeed(playerSpeed + playerAddSpeed);
        cameraControl.maxFOV =
            shelterManager.GetSkill("�þ� �ݰ�").nowSkillLevel == 1 ? 110f : 100f;
        playerAddDecDashGage =
            shelterManager.GetSkill("���� ���� �ð�").GetValue();
        playerMove.DecDashGage = playerDecDashGage - playerAddDecDashGage;
        playerStatus.PlayerMaxHp =
            playerStatus.PlayerMaxHp + shelterManager.GetSkill("�").GetValue();

        playerAddAttack =       //TODO: ���� ����� ���ݷµ� ��������
            shelterManager.GetSkill("���� ���ݷ�").GetValue() +
            shelterManager.GetSkill("���Ÿ� ���ݷ�").GetValue();
        //TODO: �÷��̾� ���� ���� �� ���ݼӵ� ����
        playerAddAttackSpeed = shelterManager.GetSkill("���� �ӵ�").GetValue();
        playerAddCriticalAttack = shelterManager.GetSkill("ġ��Ÿ ���ݷ�").GetValue();
        playerAddCriticalChance = shelterManager.GetSkill("ġ��Ÿ ���� Ȯ��").GetValue();

        playerAddInvenCount = shelterManager.GetSkill("��ȭ �賶").GetValue();
        //TODO: Inven_Bottom_Controll.cs ���� InvenCountUpgrade()  => �׳� ��ư Ŭ���� �������

        //TODO: Gather ��ų ability ����. 0715
        
    }

    public float GetTotalPlayerAttack() { return playerAttack + playerAddAttack; }
    public float GetTotalPlayerCriticalAttack() { return playerCriticalAttack + playerAddCriticalAttack; }
    public float GetTotalPlayerCriticalChance() { return playerCriticalChance + playerAddCriticalChance; }

    public float GetTotalPlayerColdResistance() { return playerColdResistance + playerAddColdResistance; }
    public float GetTotalPlayerDefense() { return playerDefense + playerAddDefense; }
    public float GetTotalPlayerSpeed() {  return playerSpeed + playerAddSpeed; }
}

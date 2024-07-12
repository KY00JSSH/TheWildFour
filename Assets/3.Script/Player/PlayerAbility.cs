using UnityEngine;

public class PlayerAbility : MonoBehaviour {
    private ShelterManager shelterManager;
    private CameraControl2 cameraControl;
    private PlayerStatus playerStatus;
    private PlayerMove playerMove;

    // �÷��̾� �⺻ �ɷ�ġ
    // �÷��̾� ���� �ÿ��� �����
    private float playerAttack;
    private float playerCriticalAttack;
    private float playerDefense;
    private float playerGather;
    private float playerSpeed;
    private float playerDashSpeed;
    private float playerDecDashGage;
    private float playerInvenCount;

    // �÷��̾� �߰� �ɷ�ġ
    // �÷��̾� ��ų �Ǵ� ��� ���� �����
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
        //TODO: SAVE ���� �� JSON���� �޾ƿ���
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
        //playerAttackSpeed =
         //   playerAttackSpeed + shelterManager.GetSkill("���� �ӵ�").GetValue();


        
    }

    public float GetTotalPlayerAttack() { return playerAttack + playerAddAttack; }

}

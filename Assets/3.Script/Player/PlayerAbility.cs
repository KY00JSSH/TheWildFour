using UnityEngine;

public class PlayerAbility : MonoBehaviour {
    private ShelterManager shelterManager;
    private PlayerStatus playerStatus;

    // �÷��̾� �⺻ �ɷ�ġ
    // �÷��̾� ���� �ÿ��� �����
    private float playerAttack;
    private float playerDefense;
    private float playerGather;
    private float playerSpeed;
    private float playerDashSpeed;
    private float playerInvenCount;

    // �÷��̾� �߰� �ɷ�ġ
    // �÷��̾� ��ų �Ǵ� ��� ���� �����
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
        //TODO: SAVE ���� �� JSON���� �޾ƿ���
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

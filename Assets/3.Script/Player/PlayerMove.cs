using UnityEngine;

public class PlayerMove : MonoBehaviour {
    private Rigidbody playerRigid;
    private Vector3 targetPosition;

    private float InputX, InputZ;
    private const float constMoveSpeed = 0.02f;
    [SerializeField] float playerMoveSpeed = 1f, playerDashSpeed = 2.5f;

    public static bool isMove { get; private set; }     // �ܺ� ��ũ��Ʈ���� ���� �̵� ���¸� �� �� �ִ� Flag

    private bool isAvailableDash = true;                // �뽬 �������� ���� �����Ǵ� Flag
    public void SetDash() { isAvailableDash = true; }
    public void ResetDash() { isAvailableDash = false; }

    // TODO : ��� ������ ����. 0707
    private float TotalDashGage, CurrentDashGage, DecDashGage, IncDashGage;
    private float defaultDashGage = 10f, defaultDecDashGage = 8f, defaultIncDashGage = 2f;
    private void Awake() {
        playerRigid = GetComponentInChildren<Rigidbody>();
    }

    private void Start() {
        isMove = false;

        // TODO : JSON ���� �Ǹ� default�� Save�� ������ �ٲ� ��
        TotalDashGage = defaultDashGage;
        CurrentDashGage = TotalDashGage;
        DecDashGage = defaultDecDashGage;
        IncDashGage = defaultIncDashGage;
    }

    private void FixedUpdate() {
        LookatMouse();

        if (isAvailableDash && Input.GetKey(KeyCode.LeftShift)) {
            Dash(true);
            Move(playerDashSpeed);
        }
        else {
            Dash(false);
            Move(playerMoveSpeed);
        }
        Debug.Log(CurrentDashGage);
    }

    private void LookatMouse() {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane GroupPlane = new Plane(Vector3.up, Vector3.zero);

        float rayLength;
        if (GroupPlane.Raycast(cameraRay, out rayLength)) {
            Vector3 pointTolook = cameraRay.GetPoint(rayLength);
            playerRigid.transform.LookAt(
                new Vector3(pointTolook.x, playerRigid.transform.position.y, pointTolook.z + 0.01f));
        }
    }

    private void Dash(bool isDash) {
        if (isDash) CurrentDashGage -= DecDashGage * Time.deltaTime;
        else CurrentDashGage += IncDashGage * Time.deltaTime;
        CurrentDashGage = Mathf.Clamp(CurrentDashGage, 0, TotalDashGage);

        if (CurrentDashGage == 0) isAvailableDash = false;
        else if (CurrentDashGage > TotalDashGage * 0.2f) isAvailableDash = true;
    }

    private void Move(float speed) {
        InputX = Input.GetAxisRaw("Horizontal");
        InputZ = Input.GetAxisRaw("Vertical");

        if (InputX != 0 || InputZ != 0) isMove = true;
        else isMove = false;

        targetPosition = new Vector3(
            playerRigid.position.x + InputX * Time.deltaTime * constMoveSpeed * speed,
            playerRigid.position.y,
            playerRigid.position.z + InputZ * Time.deltaTime * constMoveSpeed * speed);
        playerRigid.MovePosition(targetPosition);
        //transform.position = playerRigid.position;
    }
}

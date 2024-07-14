using UnityEngine;

public class PlayerMove : MonoBehaviour {
    private Rigidbody playerRigid;
    private Vector3 targetPosition;

    private float InputX, InputZ;
    private const float constMoveSpeed = 2f;
    [SerializeField] float playerMoveSpeed = 1f, playerDashSpeed = 2.5f;

    private Animator player_ani; //캐릭터 애니메이션을 위해 추가 - 지훈 수정 240708 10:59

    public static bool isMove { get; private set; }

    private bool isAvailableDash = true;
    public void SetDash() { isAvailableDash = true; }
    public void ResetDash() { isAvailableDash = false; }

    private float TotalDashGage, CurrentDashGage;
    public float DecDashGage, IncDashGage;
    private float defaultDashGage = 10f, defaultDecDashGage = 8f, defaultIncDashGage = 2f;

    public bool isSkilled = false;
    public void SetPlayerMoveSpeed(float speed) { playerMoveSpeed = speed; }
    public float GetTatalDashGage() { return TotalDashGage; }
    public float GetCurrentDashGage() { return CurrentDashGage; }

    public bool isDash { get; private set; }

    private void Awake() {
        playerRigid = GetComponentInChildren<Rigidbody>();
        player_ani = GetComponentInParent<Animator>();
    }

    private void Start() {
        isMove = false;
        isDash = false;

        //TODO : JSON 구현 되면 default를 Save된 값으로 바꿀 것
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
    }

    private void LookatMouse() {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane GroupPlane = new Plane(Vector3.up, Vector3.zero);
        float rotationSpeed = 5f;
        float rayLength;

        if (GroupPlane.Raycast(cameraRay, out rayLength)) {
            Vector3 pointTolook = cameraRay.GetPoint(rayLength);
            Vector3 targetPosition = new Vector3(pointTolook.x, playerRigid.position.y, pointTolook.z + 0.01f);
            Quaternion targetRotation =
                Quaternion.LookRotation(targetPosition - playerRigid.position) *
                Quaternion.Euler(180, 0, 180);
            Debug.Log("Target Position: " + targetPosition);
            Debug.Log("Target Rotation: " + targetRotation);

            transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime);
        }
    }

    private void Dash(bool isDash) {
        this.isDash = isDash;
        if (isDash) CurrentDashGage -= DecDashGage * Time.deltaTime;
        else CurrentDashGage += IncDashGage * Time.deltaTime;
        CurrentDashGage = Mathf.Clamp(CurrentDashGage, 0, TotalDashGage);

        if (CurrentDashGage == 0) ResetDash();
        else if (CurrentDashGage == TotalDashGage) SetDash();
    }

    private void Move(float speed) {
        InputX = Input.GetAxisRaw("Horizontal");
        InputZ = Input.GetAxisRaw("Vertical");

        if (InputX != 0 || InputZ != 0) isMove = true;
        else isMove = false;

        targetPosition = new Vector3(
            playerRigid.transform.position.x + InputX * Time.deltaTime * constMoveSpeed * speed,
            playerRigid.transform.position.y,
            playerRigid.transform.position.z + InputZ * Time.deltaTime * constMoveSpeed * speed);
        playerRigid.MovePosition(targetPosition);
        transform.position = targetPosition;

        float currentSpeed = new Vector3(InputX, 0, InputZ).magnitude * speed;
        //player_ani.SetFloat("Speed", currentSpeed);

    }
}

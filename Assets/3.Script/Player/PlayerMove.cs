using UnityEngine;

public class PlayerMove : MonoBehaviour {
    private Rigidbody playerRigid;
    private Vector3 targetPosition;

    private float InputX, InputZ;
    private const float constMoveSpeed = 2f;
    [SerializeField] float playerMoveSpeed = 1f, playerDashSpeed = 2.5f;

    private Animator player_ani; //캐릭터 애니메이션을 위해 추가 - 지훈 수정 240708 10:59

    public static bool isMove { get; private set; }     // 외부 스크립트에서 현재 이동 상태를 알 수 있는 Flag

    private bool isAvailableDash = true;                // 대쉬 게이지에 따라서 설정되는 Flag
    public void SetDash() { isAvailableDash = true; }
    public void ResetDash() { isAvailableDash = false; }

    // TODO : 대시 게이지 UI 구현. 0707
    private float TotalDashGage, CurrentDashGage, DecDashGage, IncDashGage;
    private float defaultDashGage = 10f, defaultDecDashGage = 8f, defaultIncDashGage = 2f;

    private void Awake() {
        playerRigid = GetComponentInChildren<Rigidbody>();
        player_ani = GetComponentInParent<Animator>(); //캐릭터 애니메이션을 위해 추가 - 지훈 수정 240708 10:59
    }

    private void Start() {
        isMove = false;

        // TODO : JSON 구현 되면 default를 Save된 값으로 바꿀 것
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

        if (CurrentDashGage == 0) ResetDash();
        else if (CurrentDashGage == TotalDashGage) SetDash();
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


        //캐릭터 애니메이션을 위해 추가 - 지훈 수정 240708 10:59
        float currentSpeed = new Vector3(InputX, 0, InputZ).magnitude * speed;
        
        //player_ani.SetFloat("Speed", currentSpeed);

        // Debug.Log($"속도 : {currentSpeed}");
    }
}

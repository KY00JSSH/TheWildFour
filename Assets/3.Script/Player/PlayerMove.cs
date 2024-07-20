using UnityEngine;

public class PlayerMove : MonoBehaviour {
    private CameraControl cameraControl;
    private Rigidbody playerRigid;
    private Vector3 targetPosition;

    private float InputX, InputZ;
    private const float constMoveSpeed = 2f;
    [SerializeField] private float playerMoveSpeed = 1f, playerDashSpeed = 2.5f;

    private Animator playerAnimator; //캐릭터 애니메이션을 위해 추가 - 지훈 수정 240708 10:59

    public void PlayerDieAnimation() {
        playerAnimator.SetTrigger("triggerDie");
    }


    public static bool isMove { get; private set; }

    private bool isAvailableDash = true;
    public void SetDash() { isAvailableDash = true; }
    public void ResetDash() { isAvailableDash = false; }

    private float TotalDashGage, CurrentDashGage;
    public float DecDashGage, IncDashGage;
    private float defaultDashGage = 10f, defaultDecDashGage = 8f, defaultIncDashGage = 2f;

    public bool isSkilled = false;
    public float GetPlayerMoveSpeed() { return playerMoveSpeed; }
    public void SetPlayerMoveSpeed(float speed) { playerMoveSpeed = speed; }
    public float GetTatalDashGage() { return TotalDashGage; }
    public float GetCurrentDashGage() { return CurrentDashGage; }

    public bool isDash { get; private set; }

    private void Awake() {
        playerRigid = GetComponentInParent<Rigidbody>();
        playerAnimator = GetComponentInParent<Animator>();
        cameraControl = FindObjectOfType<CameraControl>();
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
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Create") || PlayerStatus.isDead) return;


        if (isAvailableDash && Input.GetKey(KeyCode.LeftShift)) {
            Dash(true);
            Move(playerDashSpeed);
        }
        else {
            Dash(false);
            Move(playerMoveSpeed);
        }

        TakeFallDamage();
    }
    private void LateUpdate() {
        LookatMouse();
    }

    private float minFallSpeed = -5f, maxFallSpeed = -20f, currentFallSpeed = 0f;
    private bool isFallDamage = false;
    private void TakeFallDamage() {
        if (playerRigid.velocity.y < minFallSpeed) {
            currentFallSpeed = playerRigid.velocity.y;
            isFallDamage = true;
        }
        else if (playerRigid.velocity.y > minFallSpeed && isFallDamage) {
            isFallDamage = false;
            float fallingRate = Mathf.InverseLerp(
                minFallSpeed, maxFallSpeed, currentFallSpeed);
            float damage = 100 * (Mathf.Exp(fallingRate) - 1) / (Mathf.Exp(1) - 1);

            GetComponent<PlayerStatus>().TakeDamage(damage);
            currentFallSpeed = 0f;
        }
    }


    /*
    private Vector3 pastPosition = Vector3.zero;
    private void LookatMouse() {
        if (PlayerStatus.isDead) return;

        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane GroupPlane = new Plane(Vector3.up, Vector3.zero);
        float rotationSpeed = 5f;
        float rayLength;

        Vector3 pointTolook;
        if (GroupPlane.Raycast(cameraRay, out rayLength)) {
            if (isDash) pointTolook = playerRigid.position + Vector3.forward * 5f;
            else pointTolook = cameraRay.GetPoint(rayLength);

            if (isMove) {
                Vector3 pointPosition = new Vector3(pointTolook.x, playerRigid.position.y, pointTolook.z + 0.01f);
                Vector3 targetPosition = Vector3.Slerp(pastPosition, pointPosition, rotationSpeed * Time.deltaTime);
                pastPosition = targetPosition;

                Quaternion targetRotation =
                    Quaternion.LookRotation(targetPosition - playerRigid.position);


                Transform playerSpine = playerAnimator.GetBoneTransform(HumanBodyBones.Spine);
                targetRotation = Quaternion.Euler(0, -cameraControl.rotationDirection, 0) *
                    Quaternion.Euler(playerSpine.eulerAngles.x,
                    playerSpine.eulerAngles.y + targetRotation.eulerAngles.y,
                    playerSpine.eulerAngles.z);
                playerSpine.rotation = targetRotation;
            }
            else {
                Vector3 targetPosition = new Vector3(pointTolook.x, playerRigid.position.y, pointTolook.z + 0.01f);
                Quaternion targetRotation =
                    Quaternion.LookRotation(targetPosition - playerRigid.position);
                playerRigid.rotation = Quaternion.Slerp(
                            playerRigid.rotation,
                            targetRotation,
                            3f * Time.deltaTime);
            }
        }
    }
    */
    private void LookatMouse() {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane GroupPlane = new Plane(Vector3.up, Vector3.zero);

        float rotationSpeed = 2f;
        float rayLength;

        if (GroupPlane.Raycast(cameraRay, out rayLength)) {
            Vector3 pointTolook = cameraRay.GetPoint(rayLength);
            playerRigid.transform.LookAt(
                new Vector3(pointTolook.x, playerRigid.position.y, pointTolook.z + 0.01f));
            Vector3 targetPosition = new Vector3(pointTolook.x, playerRigid.position.y, pointTolook.z + 0.01f);
            Quaternion targetRotation = Quaternion.LookRotation(targetPosition - playerRigid.position);

            playerRigid.rotation = Quaternion.Slerp(
            playerRigid.rotation,
            targetRotation,
            Time.deltaTime);
        }
    }

        private void Dash(bool isDash) {
        this.isDash = isDash;
        if (isDash && isMove) CurrentDashGage -= DecDashGage * Time.deltaTime;
        else CurrentDashGage += IncDashGage * Time.deltaTime;
        CurrentDashGage = Mathf.Clamp(CurrentDashGage, 0, TotalDashGage);

        if (CurrentDashGage == 0) ResetDash();
        else if (CurrentDashGage == TotalDashGage) SetDash();
    }

    private float currentSpeed = 0f;
    private void Move(float speed) {
        InputX = Input.GetAxis("Horizontal");
        InputZ = Input.GetAxis("Vertical");

        if (InputX != 0 || InputZ != 0) isMove = true;
        else {
            isMove = false;
            speed = 0;
        }

        currentSpeed = Mathf.Lerp(currentSpeed, speed, Time.deltaTime * 3f);
        if (currentSpeed < 0.01f) currentSpeed = 0;

        targetPosition = 
            Quaternion.Euler(0, cameraControl.rotationDirection, 0) * 
            new Vector3(InputX, 0, InputZ) * Time.deltaTime * constMoveSpeed * currentSpeed;
        targetPosition += playerRigid.position;

        Quaternion targetRotation = Quaternion.Euler(0, cameraControl.rotationDirection, 0);

        playerRigid.MovePosition(targetPosition);
        playerRigid.rotation = Quaternion.Slerp(
            playerRigid.rotation,
            targetRotation,
            5f * Time.deltaTime);


        playerAnimator.SetFloat("MoveSpeed", currentSpeed);
    }
}

using UnityEditor;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
    private CameraControl cameraControl;
    private Rigidbody playerRigid;
    private Transform playerSpine;

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
        playerSpine = playerAnimator.GetBoneTransform(HumanBodyBones.Spine);
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


    private Vector3 pastPosition = Vector3.zero;
    private float rotationSpeed = 5f;

    private void LookatMouse() {
        if (PlayerStatus.isDead) return;
        Quaternion targetRotation =
            Quaternion.LookRotation(GetLookatPoint() - playerRigid.position);

        if (isMove) {
            // 상반신 회전
            targetRotation = Quaternion.Euler(0, -cameraControl.rotationDirection, 0) *
                Quaternion.Euler(playerSpine.eulerAngles.x,
                playerSpine.eulerAngles.y + targetRotation.eulerAngles.y,
                playerSpine.eulerAngles.z);
            playerSpine.rotation = Quaternion.Euler(0, moveDirection, 0) * targetRotation;


            // 하반신 회전
            targetRotation = Quaternion.Euler(0, cameraControl.rotationDirection, 0) * Quaternion.Euler(0, moveDirection, 0);
            playerRigid.rotation = 
                Quaternion.Slerp(playerRigid.rotation,
                targetRotation, rotationSpeed * Time.deltaTime);

        }
        else {
            targetRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
            playerRigid.rotation = Quaternion.Slerp(
                        playerRigid.rotation,
                        targetRotation,
                        rotationSpeed * Time.deltaTime);
        }
    }

    // key input에 따라서 playerDirection이 있어야 하고.
    // direction에 따라서 모든 방향 수정 로직은 보정을 받아야 함.

    private Vector3 GetLookatPoint() {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane GroupPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        Vector3 pointTolook;
        Vector3 targetPosition = Vector3.zero;
        if (GroupPlane.Raycast(cameraRay, out rayLength)) {
            if (isDash) pointTolook =
                    Quaternion.Euler(0, cameraControl.rotationDirection, 0) * Quaternion.Euler(0, -moveDirection, 0) *
                    new Vector3(InputX, 0, InputZ) * 2f + playerRigid.position;
            else pointTolook = cameraRay.GetPoint(rayLength);
            Vector3 pointPosition = new Vector3(pointTolook.x, playerRigid.position.y, pointTolook.z + 0.01f);

            if (isMove) {
                targetPosition = Vector3.Slerp(pastPosition, pointPosition, rotationSpeed * Time.deltaTime);
                pastPosition = targetPosition;
            }
            else
                targetPosition = new Vector3(pointTolook.x, playerRigid.position.y, pointTolook.z + 0.01f);
        }
        return targetPosition;
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
    private float moveDirection = 0f;
    private void Move(float speed) {
        InputX = Input.GetAxis("Horizontal");
        InputZ = Input.GetAxis("Vertical");
        moveDirection = Mathf.Atan2(InputX, InputZ) * Mathf.Rad2Deg;
        if (moveDirection < 0) moveDirection += 360;

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

        playerAnimator.SetFloat("MoveSpeed", currentSpeed);
    }
}

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
    private float defaultDashGage = 10f, defaultDecDashGage = 5f, defaultIncDashGage = 2f;

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

    private void Update() {
        playerAnimator.SetBool("isSideWalk", isSideWalk);
        playerAnimator.SetBool("isBackWalk", isBackWalk);
    }

    private void FixedUpdate() {
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Create") || PlayerStatus.isDead) return;
        
        if (isAvailableDash && Input.GetKey(KeyCode.LeftShift)) {
            isSideWalk = false;
            isBackWalk = false;
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
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Create") || PlayerStatus.isDead) return;
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
    Quaternion debugTargetRotation;
    private void LookatMouse() {
        Quaternion targetRotation =
            Quaternion.LookRotation(GetLookatPoint() - playerRigid.position);

        if (isMove) {
            // 상반신 회전
            if (!isDash) {
                targetRotation = Quaternion.Euler(0, -cameraControl.rotationDirection, 0) *
                    Quaternion.Euler(playerSpine.eulerAngles.x,
                    playerSpine.eulerAngles.y + targetRotation.eulerAngles.y,
                    playerSpine.eulerAngles.z);

                playerSpine.rotation = Quaternion.Euler(0, moveDirection, 0) * targetRotation;
                SetAnimationDirection(playerSpine.rotation, Quaternion.Euler(0, moveDirection, 0));
                //playerSpine.rotation = Quaternion.Euler(playerSpine.rotation.eulerAngles.x,
                //Mathf.Clamp(playerSpine.rotation.eulerAngles.y, (debugTargetRotation.eulerAngles.y - 90f) % 360, (debugTargetRotation.eulerAngles.y + 90f) % 360),
                //playerSpine.rotation.eulerAngles.z);
                //TODO: ClampAngle 필요. 기준은 debugTargetRotation +- 90 이면 앞 방향. 그 외 옆걸음과 뒷걸음 구현.
            }

            // 하반신 회전
            Debug.Log(isBackWalk);
            targetRotation = Quaternion.Euler(0, cameraControl.rotationDirection, 0) * Quaternion.Euler(0, moveDirection, 0);
            playerRigid.rotation = 
                Quaternion.Slerp(playerRigid.rotation,
                targetRotation, rotationSpeed * Time.deltaTime);
            if (isDash) {
                playerSpine.rotation = Quaternion.Slerp(playerSpine.rotation,
                    Quaternion.Euler(playerSpine.eulerAngles.x, targetRotation.eulerAngles.y - 90f, playerSpine.eulerAngles.z),
                    rotationSpeed * Time.deltaTime);
            }
        }
        else {
            targetRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
            playerRigid.rotation = Quaternion.Slerp(
                        playerRigid.rotation,
                        targetRotation,
                        rotationSpeed * Time.deltaTime);
        }
    }

    private bool isSideWalk = false, isBackWalk = false;
    private void SetAnimationDirection(Quaternion rotation, Quaternion forward) {
        float yFoward = forward.eulerAngles.y + 360f;
        float yRotate = -rotation.eulerAngles.y + 360f - 90f;
        if (yRotate < 360f) yRotate += 360f;
        if (yRotate > 720f) yRotate -= 360f;

        isSideWalk = false;
        isBackWalk = false;
        if ((yRotate > yFoward + 50 && yRotate <= yFoward + 100) || 
            (yRotate < yFoward - 50 && yRotate >= yFoward - 100)) {
            Debug.Log("SIDE");
            isSideWalk = true;
        }
        else if (yRotate > yFoward + 100 || yRotate < yFoward - 100) {
            Debug.Log("BACK");
            isBackWalk = true;
        }
        else {
        }
    }

    private Vector3 GetLookatPoint() {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane GroupPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        Vector3 targetPosition = Vector3.zero;
        if (GroupPlane.Raycast(cameraRay, out rayLength)) {
            Vector3 pointTolook = cameraRay.GetPoint(rayLength);
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
        moveDirection = Mathf.Atan2(-InputX, InputZ) * Mathf.Rad2Deg;
        if (moveDirection < 0) moveDirection += 360;

        if (InputX != 0 || InputZ != 0) isMove = true;
        else {
            isMove = false;
            speed = 0;
            isSideWalk = false;
            isBackWalk = false;
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

    public Quaternion GetRigid() { return playerRigid.rotation; }
    public Quaternion GetSpine() { return playerSpine.rotation; }
    public Quaternion GetDirection() { return Quaternion.Euler(0, moveDirection, 0); }
    public Quaternion GetCameraRotation () { return Quaternion.Euler(0, cameraControl.rotationDirection, 0); }
    public Quaternion GetTargetRotation () { return debugTargetRotation; }
    public Vector3 GetInputXZ() { return new Vector3(InputX, 0, InputZ); }
}

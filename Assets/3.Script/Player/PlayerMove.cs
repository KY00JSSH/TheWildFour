using System.Collections;
using UnityEditor;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
    private CameraControl cameraControl;
    private ShelterManager shelterManager;
    private Rigidbody playerRigid;
    private Transform playerSpine;

    private Vector3 targetPosition;

    private float InputX, InputZ;
    private const float constMoveSpeed = 2f;
    [SerializeField] private float playerMoveSpeed = 1f, playerDashSpeed = 2.5f;

    private Animator playerAnimator; //캐릭터 애니메이션을 위해 추가 - 지훈 수정 240708 10:59

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

    public void SetSideWalk(bool flag) { isSideWalk = flag; }
    public void SetBackWalk(bool flag) { isBackWalk = flag; }

    public bool isDash { get; private set; }

    private void Awake() {
        playerRigid = transform.parent.GetComponent<Rigidbody>();
        playerAnimator = GetComponentInParent<Animator>();
        cameraControl = FindObjectOfType<CameraControl>();
        playerSpine = playerAnimator.GetBoneTransform(HumanBodyBones.Spine);
        shelterManager = FindObjectOfType<ShelterManager>();
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

    public static bool isPlayerBuilding { get; private set; }
    private void Update() {
        playerAnimator.SetBool("isSideWalk", isSideWalk);
        playerAnimator.SetBool("isBackWalk", isBackWalk);
        isPlayerBuilding = playerAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Create");
    }

    private void FixedUpdate() {
        if (isPlayerBuilding || PlayerStatus.isDead) return;
        
        if (isSkilled && isAvailableDash && Input.GetKey(KeyCode.LeftShift)) {
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
        if (isPlayerBuilding || PlayerStatus.isDead) return;
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

            GetComponentInParent<PlayerStatus>().TakeDamage(damage);
            currentFallSpeed = 0f;
        }
    }

    private Vector3 pastPosition = Vector3.zero;
    private float rotationSpeed = 5f;

    private void LookatMouse() {
        Quaternion targetRotation =
            Quaternion.LookRotation(GetLookatPoint() - playerRigid.position);

        if (isMove) {
            // 상반신 회전
            if (!isDash) {
                targetRotation = 
                    Quaternion.Euler(playerSpine.eulerAngles.x,
                    playerSpine.eulerAngles.y + targetRotation.eulerAngles.y,
                    playerSpine.eulerAngles.z);
                playerSpine.rotation = 
                    Quaternion.Euler(0, isBackWalk ? 180 : 0, 0) * Quaternion.Euler(0, -cameraControl.rotationDirection, 0) * 
                    Quaternion.Euler(0, moveDirection, 0) * targetRotation;
                    SetAnimationDirection(playerSpine.rotation, 
                        Quaternion.Euler(0, -cameraControl.rotationDirection, 0) * Quaternion.Euler(0, moveDirection, 0));

            }

            // 하반신 회전
            targetRotation = Quaternion.Euler(0, cameraControl.rotationDirection, 0) * Quaternion.Euler(0, -moveDirection, 0) * (isBackWalk ? Quaternion.Euler(0, 180, 0) : Quaternion.identity);
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
        if (isTransition) return;

        float yFoward = forward.eulerAngles.y + 360f;
        float yRotate = -rotation.eulerAngles.y + 360f - 90f;
        if (yRotate < 360f - 90f) yRotate += 360f;
        if (yRotate > 720f - 90f) yRotate -= 360f;
        if (yFoward % 360 == 0 && yRotate % 360 > 180) yRotate = yRotate % 360 - 360;
        yFoward %= 360; yRotate %= 360;

        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) {
            /*
            // 공격 모션시 허리 회전 각도 조절
            playerSpine.rotation = Quaternion.Euler(playerSpine.eulerAngles.x,
                Mathf.Clamp(yRotate, yFoward - 140, yFoward + 140) + 360, playerSpine.eulerAngles.z);
            */
        }

        else {
            yRotate += 0 - yFoward;
            if (yRotate > 180) yRotate -= 360;
            yFoward = 0;
            if (yRotate > yFoward + 100 || yRotate < yFoward - 100) {
                isBackWalk = true;
            }
            else if ((yRotate > yFoward + 50 && yRotate <= yFoward + 100) ||
                (yRotate < yFoward - 50 && yRotate >= yFoward - 100)) {
                isSideWalk = true;
            }
            else {
                isBackWalk = false;
                isSideWalk = false;
            }
            StartCoroutine(transitionTime());
        }

    }
    private bool isTransition;
    private IEnumerator transitionTime() {
        isTransition = true;
        yield return new WaitForSeconds(0.18f);
        isTransition = false;
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
        else if ((CurrentDashGage == TotalDashGage) &&
            !playerAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) 
            SetDash();
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
        playerRigid.MovePosition(targetPosition);

        playerAnimator.SetFloat("MoveSpeed", currentSpeed);
        EarnMoveExp();
    }

    private void EarnMoveExp () {
        shelterManager.AddMoveExp(Time.deltaTime * currentSpeed * 30f);
    }
}

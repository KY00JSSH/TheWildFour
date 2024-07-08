using UnityEngine;

public class PlayerMove : MonoBehaviour {
    private Rigidbody playerRigid;
    private Vector3 targetPosition;

    private float InputX, InputZ;
    private const float constMoveSpeed = 2f;
    [SerializeField] float playerMoveSpeed = 1f, playerDashSpeed = 2.5f;

    private Animator player_ani; //캐릭터 애니메이션을 위해 추가 - 지훈 수정 240708 10:59

    private bool isAvailableDash = true;    // 대쉬 게이지에 따라서 설정되는 Flag
    public void SetDash() {
        isAvailableDash = true;
    }
    public void ResetDash() {
        isAvailableDash = false;
    }
    

    private void Awake() {
        playerRigid = GetComponentInChildren<Rigidbody>();
        player_ani = GetComponentInParent<Animator>(); //캐릭터 애니메이션을 위해 추가 - 지훈 수정 240708 10:59
    }

    private void FixedUpdate() {
        LookatMouse();

        if (isAvailableDash && Input.GetKey(KeyCode.LeftShift))
            Move(playerDashSpeed);
        else
            Move(playerMoveSpeed);        
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
    

    private void Move(float speed) {
        InputX = Input.GetAxisRaw("Horizontal");
        InputZ = Input.GetAxisRaw("Vertical");

        targetPosition = new Vector3(
            playerRigid.position.x + InputX * Time.deltaTime * constMoveSpeed * speed,
            playerRigid.position.y,
            playerRigid.position.z + InputZ * Time.deltaTime * constMoveSpeed * speed);
        playerRigid.MovePosition(targetPosition);
        //transform.position = playerRigid.position;


        //캐릭터 애니메이션을 위해 추가 - 지훈 수정 240708 10:59
        float currentSpeed = new Vector3(InputX, 0, InputZ).magnitude * speed;
        player_ani.SetFloat("Speed", currentSpeed);

        Debug.Log($"속도 : {currentSpeed}");
    }
}
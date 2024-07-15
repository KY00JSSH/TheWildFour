using UnityEngine;

public class PlayerMove2_test : MonoBehaviour
{
    private Rigidbody playerRigid;
    private Vector3 targetPosition;
    [SerializeField] private Animator player_ani; //캐릭터 애니메이션을 위해 추가 - 지훈 수정 240708 10:59
    [SerializeField] private LayerMask layerMask;/**/

    private float InputX, InputZ;
    private const float constMoveSpeed = 2f;
    [SerializeField] float playerMoveSpeed = 1f, playerDashSpeed = 2.5f;

    public static bool isMove { get; private set; }     // 외부 스크립트에서 현재 이동 상태를 알 수 있는 Flag

    private bool isAvailableDash = true;    // 대쉬 게이지에 따라서 설정되는 Flag

    public void SetDash() { isAvailableDash = true; }
    public void ResetDash() { isAvailableDash = false; }

    // TODO : 대시 게이지 UI 구현. 0707
    private float TotalDashGage, CurrentDashGage, DecDashGage, IncDashGage;
    private float defaultDashGage = 10f, defaultDecDashGage = 8f, defaultIncDashGage = 2f;

    private void Awake()
    {       
        playerRigid = GetComponent<Rigidbody>();
        player_ani = GetComponentInChildren<Animator>(); //캐릭터 애니메이션을 위해 추가 - 지훈 수정 240708 10:59
    }
    private void Start()
    {
        isMove = false;

        // TODO : JSON 구현 되면 default를 Save된 값으로 바꿀 것
        TotalDashGage = defaultDashGage;
        CurrentDashGage = TotalDashGage;
        DecDashGage = defaultDecDashGage;
        IncDashGage = defaultIncDashGage;
    }

    private void FixedUpdate()
    {
        LookatMouse();

        //if (isAvailableDash && Input.GetKey(KeyCode.LeftShift))
        //    Move(playerDashSpeed);
        //else
        //    Move(playerMoveSpeed);

        //Move();        

        if (isAvailableDash && Input.GetKey(KeyCode.LeftShift))
        {
            Dash(true);
            Move();
        }
        else
        {
            Dash(false);
            Move();
        }
    }

    private void LookatMouse()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane GroupPlane = new Plane(Vector3.up, Vector3.zero);

        float rayLength;
        if (GroupPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointTolook = cameraRay.GetPoint(rayLength);
            //playerRigid.transform.LookAt(
            //    new Vector3(pointTolook.x, playerRigid.transform.position.y, pointTolook.z + 0.01f));
            Vector3 direction = (pointTolook - transform.position).normalized;                            /**/
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));  /**/
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);/**/            
        }
    }
    private void Dash(bool isDash)
    {
        if (isDash) CurrentDashGage -= DecDashGage * Time.deltaTime;
        else CurrentDashGage += IncDashGage * Time.deltaTime;
        CurrentDashGage = Mathf.Clamp(CurrentDashGage, 0, TotalDashGage);

        if (CurrentDashGage == 0) ResetDash();
        else if (CurrentDashGage == TotalDashGage) SetDash();
    }

    private void Move(/*float speed*/)
    {
        InputX = Input.GetAxisRaw("Horizontal");
        InputZ = Input.GetAxisRaw("Vertical");

        if (InputX != 0 || InputZ != 0) isMove = true;
        else isMove = false;

        //targetPosition = new Vector3(
        //    playerRigid.position.x + InputX * Time.deltaTime * constMoveSpeed * speed,
        //    playerRigid.position.y,
        //    playerRigid.position.z + InputZ * Time.deltaTime * constMoveSpeed * speed);
        //playerRigid.MovePosition(targetPosition);

        float speed = (isAvailableDash && Input.GetKey(KeyCode.LeftShift)) ? playerDashSpeed : playerMoveSpeed;/**/

       
        Vector3 moveDirection = transform.forward * InputZ + transform.right * InputX;                /**/
        targetPosition = transform.position + moveDirection * constMoveSpeed * speed * Time.deltaTime;/**/
        playerRigid.MovePosition(targetPosition);                                                     /**/

        //캐릭터 애니메이션을 위해 추가 - 지훈 수정 240708 10:59
        //float currentSpeed = new Vector3(InputX, 0, InputZ).magnitude * speed;
        //player_ani.SetFloat("Speed", currentSpeed);

        player_ani.SetFloat("Horizontal", InputX);/**/
        player_ani.SetFloat("Vertical", InputZ);  /**/
        player_ani.SetFloat("Speed", moveDirection.magnitude * speed);  /**/

        //Debug.Log($"이동 속도: {new Vector3(InputX, 0, InputZ).magnitude * speed}");
    }
}
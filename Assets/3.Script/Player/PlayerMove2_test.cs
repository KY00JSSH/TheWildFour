using UnityEngine;

public class PlayerMove2_test : MonoBehaviour
{
    [SerializeField] private Animator player_ani; //캐릭터 애니메이션을 위해 추가 - 지훈 수정 240708 10:59
    private Rigidbody playerRigid;
    [SerializeField] private LayerMask layerMask;/**/
    private Vector3 targetPosition;

    private float InputX, InputZ;
    private const float constMoveSpeed = 2f;
    [SerializeField] float playerMoveSpeed = 1f, playerDashSpeed = 2.5f;

    private bool isAvailableDash = true;    // 대쉬 게이지에 따라서 설정되는 Flag

    public void SetDash()
    {
        isAvailableDash = true;
    }
    public void ResetDash()
    {
        isAvailableDash = false;
    }

    private void Awake()
    {
       
        playerRigid = GetComponent<Rigidbody>();
        player_ani = GetComponentInChildren<Animator>(); //캐릭터 애니메이션을 위해 추가 - 지훈 수정 240708 10:59
    }

    private void FixedUpdate()
    {
        LookatMouse();

        //if (isAvailableDash && Input.GetKey(KeyCode.LeftShift))
        //    Move(playerDashSpeed);
        //else
        //    Move(playerMoveSpeed);

        Move();        
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

    private void Move(/*float speed*/)
    {
        InputX = Input.GetAxisRaw("Horizontal");
        InputZ = Input.GetAxisRaw("Vertical");

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

        Debug.Log($"이동 속도: {new Vector3(InputX, 0, InputZ).magnitude * speed}");
    }
}
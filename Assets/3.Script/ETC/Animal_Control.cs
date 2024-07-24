using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal_Control : MonoBehaviour
{
    /*
    루트모션을 이용한 오브젝트 이동
    자동으로 돌아다니는 스크립트 또는 AI 
    */
    private Animator animator;

    public float idleDuration = 3f; //Idle 상태의 지속 시간
    public float moveDuration = 3f; //이동 상태의 지속 시간

    //public float MAX_Speed = 0f; //최고속도, 도망칠 때
    //public float wander_Speed = 0f; //일반속도, 일반적인 상태
    //public float current_Speed = 0f;

    private bool isRunning = false; //플레이어가 감지되었는지 여부
    private float idleTimer; //Idle 상태의 타이머
    private float moveTimer; //이동 상태의 타이머
    

    private Transform player; //플레이어 위치 저장, 동물이 플레이어를 감지하고 도망치게 하기 위함
    private Vector3 moveDirection; //동물이 이동하는 방향

    private CapsuleCollider playerDetectorCollider; //자식오브젝트의 플레이어 감지를 위한 캡슐콜라이더

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.applyRootMotion = true;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerDetectorCollider = GetComponentInChildren<CapsuleCollider>(); //자식오브젝트의 플레이어 감지를 위한 캡슐콜라이더
    }

    private void Start()
    {
        idleTimer = 0;
        moveTimer = 0;
    }

    private void Update()
    {
        if(isRunning)
        {
            RunFromPlayer();
        }    
        else
        {
            Random_Idle_Or_Walk();
        }
    }    

    public void OnChildTriggerEnter(Collider other) //토끼의 플레이어감지 콜라이더에 플레이어가 감지됐을 경우
    {
        if(other.CompareTag("Player"))
        {
            isRunning = true;
            moveDirection = (transform.position - player.position).normalized;
            animator.SetBool("isRunning", true);
        }
    }

    public void OnChildTriggerExit(Collider other)
    {
        if(!other.CompareTag("Player"))
        {
            Random_Idle_Or_Walk();
        }

        if(other.CompareTag("Player"))
        {
            isRunning = false;
            animator.SetBool("isRunning", false);
        }
    }

    private void Random_Idle_Or_Walk()
    {
        
        if (idleTimer > 0) //Idle이 재생될 때
        {
            idleTimer -= Time.deltaTime;
            animator.SetFloat("MoveSpeed", 0);
        }
        else
        {
            moveTimer -= Time.deltaTime; // Idle 재생이 아닐 때는 걸어다니면서 moveTimer를 소비
            if (moveTimer > 0)
            {
                animator.SetFloat("Idle_SetFloat", 7);
                //transform.Translate(Vector3.forward * wander_Speed * Time.deltaTime);
            }
            else
            {
                idleTimer = idleDuration;
                moveTimer = moveDuration;
                animator.SetFloat("Idle_SetFloat", Random.Range(0, 7));
            }
        }
    }

    private void RunFromPlayer() //플레이어로부터 도망
    {
        if(isRunning)
        { 
            animator.SetBool("isRunning", true);
            transform.forward = moveDirection;

            //플레이어가 계속 감지 범위 내에 있는지 확인
            float detectionRadius = playerDetectorCollider.radius; //자식오브젝트의 플레이어 감지를 위한 캡슐콜라이더
            if (Vector3.Distance(transform.position, player.position) < detectionRadius)
            {
                moveDirection = (transform.position - player.position).normalized;
            }
            else
            {
                isRunning = false;
                animator.SetBool("isRunning", false);
            }
        }
    }
}

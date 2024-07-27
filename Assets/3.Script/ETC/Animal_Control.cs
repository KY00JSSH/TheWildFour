using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal_Control : MonoBehaviour
{
    /*
    루트모션을 이용한 오브젝트 이동
    자동으로 돌아다니는 스크립트 또는 AI 
    */

    public float MAXHP;
    public float currentHP;

    private NavMeshAgent agent;
    public Transform navMeshSurface;

    private Animator animator;

    public float idleDuration = 3f; //Idle 상태의 지속 시간
    public float moveDuration = 3f; //이동 상태의 지속 시간

    private bool isRunning = false; //플레이어가 감지되었는지 여부
    private float idleTimer; //Idle 상태의 타이머
    private float moveTimer; //이동 상태의 타이머
    private float runTimer; //이동 상태의 타이머
    

    private Transform player; //플레이어 위치 저장, 동물이 플레이어를 감지하고 도망치게 하기 위함
    private Vector3 moveDirection; //동물이 이동하는 방향

    public CapsuleCollider playerDetector_Capsule; //자식오브젝트의 플레이어 감지를 위한 캡슐콜라이더
    public BoxCollider playerDetector_Box; //자식오브젝트의 플레이어 감지를 위한 캡슐콜라이더

    private void Awake()
    {
        animator = GetComponent<Animator>(); 
        animator.applyRootMotion = true; //루트 모션 적용
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        //agent.updateRotation = false;

        playerDetector_Capsule = GetComponentInChildren<CapsuleCollider>(); //자식오브젝트의 플레이어 감지를 위한 캡슐콜라이더
        playerDetector_Box = GetComponentInChildren<BoxCollider>(); //자식오브젝트의 플레이어 감지를 위한 박스콜라이더

        navMeshSurface = FindObjectOfType<NavMeshSurface>().transform;

        GetComponentInChildren<Animal_Detecting_Player>().parentControl = this; /**/
    }

    private void Start()
    {       
        idleTimer = 0;
        moveTimer = 0;
        runTimer = 0;
    }

    private void Update()
    {
        CheckDistance(); //네비메쉬 동적 베이크를 위한 거리계산 메서드

        if (isRunning)
        {
            RunFromPlayer(); //플레이어로부터 도망
        }
        else
        {
            Random_Idle_Or_Walk(); //제자리 Idle  재생 또는 이동 메서드
        }
    }    

    void CheckDistance() //네비메쉬 동적 베이크를 위한 거리계산 메서드
    {
        //if(Vector3.Distance(this.transform.position, navMeshSurface.position) > 10f)
        //{
        //    navMeshSurface.transform.position = this.transform.position;
        //    navMeshSurface.GetComponent<NavMeshSurface>().BuildNavMesh();
        //}

        float distance = Vector3.Distance(this.transform.position, navMeshSurface.position);
        if (distance > 10f)
        {
            navMeshSurface.transform.position = this.transform.position;
            NavMeshSurface navMeshSurfaceComponent = navMeshSurface.GetComponent<NavMeshSurface>();
            if (navMeshSurfaceComponent != null)
            {
                navMeshSurfaceComponent.BuildNavMesh();
            }
        }
    }      
    
    private void Random_Idle_Or_Walk() //제자리 Idle  재생 또는 이동 메서드
    {     
        if (idleTimer > 0)
        {
            // 제자리 Idle 재생
            idleTimer -= Time.deltaTime;
        }
        else if (moveTimer > 0)
        {
            // 이동 Idle 재생
            animator.SetFloat("Idle_SetFloat", 7);
            if (!agent.hasPath)
            {
                Vector3 randomDirection = Random.insideUnitSphere * 10f;
                randomDirection += transform.position;
                NavMeshHit hit;
                NavMesh.SamplePosition(randomDirection, out hit, 10f, 1);
                agent.SetDestination(hit.position);
            }
            moveTimer -= Time.deltaTime;
        }
        else
        {
            idleTimer = idleDuration;
            moveTimer = moveDuration;
            animator.SetFloat("Idle_SetFloat", Random.Range(0, 7));
        }
    }    

    private void RunFromPlayer() //플레이어로부터 도망
    {        
        if(runTimer > 0)
        { 
            moveDirection = (transform.position - player.position).normalized;
            Vector3 runTo = transform.transform.position + moveDirection * 10f;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(runTo, out hit, 10f, 1))
            {
                agent.SetDestination(hit.position);
            }
            runTimer -= Time.deltaTime;
        }

        else if (Vector3.Distance(transform.position, player.position) > playerDetector_Capsule.radius &&
            Vector3.Distance(transform.position, player.position) > playerDetector_Box.size.magnitude)        
        {
            isRunning = false;
            animator.SetBool("isRunning", false);
            agent.ResetPath();
        }
    }

    public void PlayerDetected() //플레이어가 검출됐을 때
    {
        isRunning = true;
        animator.SetBool("isRunning", true);
        agent.ResetPath();
        runTimer = 5f;
    }

    public void PlayerLost() //플레이어가 범위에서 벗어났을 때
    {
        isRunning = false;
        animator.SetBool("isRunning", false);
        agent.ResetPath();
    }   
}

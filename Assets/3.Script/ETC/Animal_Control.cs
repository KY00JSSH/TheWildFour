using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal_Control : MonoBehaviour
{
    /*
    ��Ʈ����� �̿��� ������Ʈ �̵�
    �ڵ����� ���ƴٴϴ� ��ũ��Ʈ �Ǵ� AI 
    */

    private NavMeshAgent agent;
    public Transform navMeshSurface;

    private Animator animator;

    public float idleDuration = 3f; //Idle ������ ���� �ð�
    public float moveDuration = 3f; //�̵� ������ ���� �ð�

    //public float MAX_Speed = 0f; //�ְ�ӵ�, ����ĥ ��
    //public float wander_Speed = 0f; //�Ϲݼӵ�, �Ϲ����� ����
    //public float current_Speed = 0f;

    private bool isRunning = false; //�÷��̾ �����Ǿ����� ����
    private float idleTimer; //Idle ������ Ÿ�̸�
    private float moveTimer; //�̵� ������ Ÿ�̸�
    

    private Transform player; //�÷��̾� ��ġ ����, ������ �÷��̾ �����ϰ� ����ġ�� �ϱ� ����
    private Vector3 moveDirection; //������ �̵��ϴ� ����

    private CapsuleCollider playerDetectorCollider; //�ڽĿ�����Ʈ�� �÷��̾� ������ ���� ĸ���ݶ��̴�

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.applyRootMotion = true;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerDetectorCollider = GetComponentInChildren<CapsuleCollider>(); //�ڽĿ�����Ʈ�� �÷��̾� ������ ���� ĸ���ݶ��̴�
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        agent.updateRotation = false;

        GetComponentInChildren<Animal_Detecting_Player>().parentControl = this; /**/
    }

    private void Start()
    {
        idleTimer = 0;
        moveTimer = 0;        
    }

    private void Update()
    {
        CheckDistance();

        if (isRunning)
        {
            RunFromPlayer();
        }    
        else
        {
            Random_Idle_Or_Walk();
        }

        //NavMeshAgent�� ��ġ�� Animator�� ����ȭ
        if(agent.remainingDistance > agent.stoppingDistance)
        {
            animator.SetFloat("MoveSpeed", agent.velocity.magnitude);
        }
        else
        {
            animator.SetFloat("MoveSpeed", 0);
        }
    }    

    void CheckDistance()
    {
        if(Vector3.Distance(this.transform.position, navMeshSurface.position) > 10f)
        {
            navMeshSurface.transform.position = this.transform.position;
            navMeshSurface.GetComponent<NavMeshSurface>().BuildNavMesh();
        }
    }

    public void OnChildTriggerEnter(Collider other) //�䳢�� �÷��̾�� �ݶ��̴��� �÷��̾ �������� ���
    {
        if(other.CompareTag("Player"))
        {
            isRunning = true;
            moveDirection = (transform.position - player.position).normalized;
            animator.SetBool("isRunning", true);
            Debug.Log("��Ȳí!");
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
            Debug.Log("��Ҵ�...");
        }
    }

    private void Random_Idle_Or_Walk()
    {
        
        if (idleTimer > 0) //Idle�� ����� ��
        {
            idleTimer -= Time.deltaTime;
            animator.SetFloat("MoveSpeed", 0);
        }
        else
        {
            moveTimer -= Time.deltaTime; // Idle ����� �ƴ� ���� �ɾ�ٴϸ鼭 moveTimer�� �Һ�
            //if (moveTimer > 0)
            //{
            //    animator.SetFloat("Idle_SetFloat", 7);
            //    //transform.Translate(Vector3.forward * wander_Speed * Time.deltaTime);
            //}
            //else
            //{
            //    idleTimer = idleDuration;
            //    moveTimer = moveDuration;
            //    animator.SetFloat("Idle_SetFloat", Random.Range(0, 7));
            //}
            if (moveTimer > 0)
            {
                animator.SetFloat("Idle_SetFloat", 7);
                if (!agent.hasPath)
                {
                    Vector3 randomDirection = Random.insideUnitSphere * 10f;
                    randomDirection += transform.position;
                    NavMeshHit hit;
                    NavMesh.SamplePosition(randomDirection, out hit, 10f, 1);
                    agent.SetDestination(hit.position);
                }
            }
            else
            {
                idleTimer = idleDuration;
                moveTimer = moveDuration;
                animator.SetFloat("Idle_SetFloat", Random.Range(0, 7)); 
            }
        }
    }

    private void RunFromPlayer() //�÷��̾�κ��� ����
    {
        if(isRunning)
        {
            animator.SetBool("isRunning", true);
            //transform.forward = moveDirection;

            //�÷��̾ ��� ���� ���� ���� �ִ��� Ȯ��
            float detectionRadius = playerDetectorCollider.radius; //�ڽĿ�����Ʈ�� �÷��̾� ������ ���� ĸ���ݶ��̴�
            if (Vector3.Distance(transform.position, player.position) < detectionRadius)
            {
                moveDirection = (transform.position - player.position).normalized;
                Vector3 runTo = transform.transform.position + moveDirection * 10f; /**/
                NavMeshHit hit;                                                     /**/
                NavMesh.SamplePosition(runTo, out hit, 10f, 1);                     /**/
                agent.SetDestination(hit.position);                                 /**/
            }
            else
            {
                isRunning = false;
                animator.SetBool("isRunning", false);
                agent.ResetPath();
            }
        }
    }
}

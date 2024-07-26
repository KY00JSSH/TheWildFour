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

    public float MAXHP;
    public float currentHP;

    private NavMeshAgent agent;
    public Transform navMeshSurface;

    private Animator animator;

    public float idleDuration = 3f; //Idle ������ ���� �ð�
    public float moveDuration = 3f; //�̵� ������ ���� �ð�

    private bool isRunning = false; //�÷��̾ �����Ǿ����� ����
    private float idleTimer; //Idle ������ Ÿ�̸�
    private float moveTimer; //�̵� ������ Ÿ�̸�
    private float runTimer; //�̵� ������ Ÿ�̸�
    

    private Transform player; //�÷��̾� ��ġ ����, ������ �÷��̾ �����ϰ� ����ġ�� �ϱ� ����
    private Vector3 moveDirection; //������ �̵��ϴ� ����

    public CapsuleCollider playerDetector_Capsule; //�ڽĿ�����Ʈ�� �÷��̾� ������ ���� ĸ���ݶ��̴�
    public BoxCollider playerDetector_Box; //�ڽĿ�����Ʈ�� �÷��̾� ������ ���� ĸ���ݶ��̴�

    private void Awake()
    {
        animator = GetComponent<Animator>(); 
        animator.applyRootMotion = true; //��Ʈ ��� ����
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        agent.updateRotation = false;

        playerDetector_Capsule = GetComponentInChildren<CapsuleCollider>(); //�ڽĿ�����Ʈ�� �÷��̾� ������ ���� ĸ���ݶ��̴�
        playerDetector_Box = GetComponentInChildren<BoxCollider>(); //�ڽĿ�����Ʈ�� �÷��̾� ������ ���� �ڽ��ݶ��̴�

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
        CheckDistance(); //�׺�޽� ���� ����ũ�� ���� �Ÿ���� �޼���

        if (isRunning)
        {
            RunFromPlayer(); //�÷��̾�κ��� ����
        }
        else
        {
            Random_Idle_Or_Walk(); //���ڸ� Idle  ��� �Ǵ� �̵� �޼���
        }
    }    

    void CheckDistance() //�׺�޽� ���� ����ũ�� ���� �Ÿ���� �޼���
    {
        if(Vector3.Distance(this.transform.position, navMeshSurface.position) > 10f)
        {
            navMeshSurface.transform.position = this.transform.position;
            navMeshSurface.GetComponent<NavMeshSurface>().BuildNavMesh();
        }
    }      
    
    private void Random_Idle_Or_Walk() //���ڸ� Idle  ��� �Ǵ� �̵� �޼���
    {     
        if (idleTimer > 0)
        {
            // ���ڸ� Idle ���
            idleTimer -= Time.deltaTime;
        }
        else if (moveTimer > 0)
        {
            // �̵� Idle ���
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

    private void RunFromPlayer() //�÷��̾�κ��� ����
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

    public void PlayerDetected() //�÷��̾ ������� ��
    {
        isRunning = true;
        animator.SetBool("isRunning", true);
        agent.ResetPath();
        runTimer = 5f;
    }

    public void PlayerLost() //�÷��̾ �������� ����� ��
    {
        isRunning = false;
        animator.SetBool("isRunning", false);
        agent.ResetPath();
    }   
}

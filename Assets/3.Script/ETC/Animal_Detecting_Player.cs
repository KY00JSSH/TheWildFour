using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal_Detecting_Player : MonoBehaviour
{
    /*
    1. �� ���� ���� �÷��̾��±� ������Ʈ�� ������ ���, �÷��̾��� �ݴ� �������� �پ '������ �Ÿ�'�� �̵�
    2. ���� �þ� ���� ���� �÷��̾��±� ������Ʈ�� ������ ���, �÷��̾��� �ݴ� �������� �پ '������ �Ÿ�'�� �̵�
    */



    [SerializeField] private Collider detectingCircle;
    [SerializeField] private Collider detectingSight;

    private Animal_Control parentControl;

    private void Awake()
    {
        parentControl = GetComponentInParent<Animal_Control>();
    }

    private void OnTriggerEnter(Collider other)
    {
        parentControl.OnChildTriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        parentControl.OnChildTriggerExit(other);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}

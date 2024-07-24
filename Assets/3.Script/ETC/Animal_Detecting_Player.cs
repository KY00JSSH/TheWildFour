using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal_Detecting_Player : MonoBehaviour
{
    /*
    1. 원 범위 내에 플레이어태그 오브젝트가 감지될 경우, 플레이어의 반대 방향으로 뛰어서 '일정한 거리'를 이동
    2. 전방 시야 범위 내에 플레이어태그 오브젝트가 감지될 경우, 플레이어의 반대 방향으로 뛰어서 '일정한 거리'를 이동
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

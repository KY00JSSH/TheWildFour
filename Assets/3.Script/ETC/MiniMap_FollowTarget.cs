using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap_FollowTarget : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.transform.position + offset;    
    }
}

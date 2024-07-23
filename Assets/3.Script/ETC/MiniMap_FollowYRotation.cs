using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap_FollowYRotation : MonoBehaviour
{
    public Transform target;

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
        transform.rotation = Quaternion.Euler(90, 0, target.eulerAngles.y * -1);
    }
}

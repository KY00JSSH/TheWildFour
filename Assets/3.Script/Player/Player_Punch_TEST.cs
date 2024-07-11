using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Punch_TEST : MonoBehaviour
{
    [SerializeField] private Animator player_ani;
    [SerializeField] private float punchInterval = 0.1f;

    private bool isPunch = false;
    private float nextPunchTime = 0f;

    private void Update()
    {
        if(Input.GetMouseButton(0) && Time.time >= nextPunchTime)
        {
            Punch();
            nextPunchTime = Time.time + punchInterval;
        }
    }

    private void Punch()
    {
        isPunch = true;

        float randomPunch = Random.Range(0f, 1f);
        player_ani.SetFloat("Punch_float", randomPunch);

        player_ani.SetTrigger("Punch_Trigger");
    }


}

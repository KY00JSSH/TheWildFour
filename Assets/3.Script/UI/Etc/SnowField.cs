using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowField : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
        UpdateSnowFieldPosition();
    }

    private void Update() {
        if (PlayerMove.isMove) UpdateSnowFieldPosition();
    }

    private void UpdateSnowFieldPosition() {
        transform.position = new Vector3(player.transform.position.x - 19f, player.transform.position.y + 20f, player.transform.position.z);
    }

}

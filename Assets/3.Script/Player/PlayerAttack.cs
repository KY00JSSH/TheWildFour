using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAttack : MonoBehaviour {
    private PlayerMove playerMove;
    private Animator playerAnimator;
    private bool isAttack, isEquip;

    private float moveSpeed;

    private void Awake() {
        playerAnimator = GetComponentInParent<Animator>();
        playerMove = GetComponent<PlayerMove>();
        moveSpeed = playerMove.GetPlayerMoveSpeed();
    }

    private void Update() {
        playerAnimator.SetBool("isAttack", isAttack);
        playerAnimator.SetBool("isEquip", isEquip);

        CheckAttack();
    }

    private void CheckAttack() {
        SetMoveSpeedOnAttack();
        if (Input.GetMouseButton(0)) {
            if (!EventSystem.current.IsPointerOverGameObject()) {
                if (!isAttack) {
                    isAttack = true;
                    moveSpeed = playerMove.GetPlayerMoveSpeed();
                }
            }
        }
        else 
            isAttack = false;
        

    }
    private void SetMoveSpeedOnAttack() {
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) {
            playerMove.SetPlayerMoveSpeed(moveSpeed * 0.4f);
        }
        else playerMove.SetPlayerMoveSpeed(moveSpeed);
    }
}

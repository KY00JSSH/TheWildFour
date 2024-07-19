using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAttack : MonoBehaviour {
    [SerializeField] Collider[] fistCollider;
    private Animator playerAnimator;
    private PlayerMove playerMove;

    private bool isAttack, isEquip, isLeftFist;

    private float moveSpeed;
    public float attackSpeed { get; private set; }

    private void Awake() {
        playerAnimator = GetComponentInParent<Animator>();
        playerMove = GetComponent<PlayerMove>();
        moveSpeed = playerMove.GetPlayerMoveSpeed();
    }

    private void Update() {
        playerAnimator.SetBool("isAttack", isAttack);
        playerAnimator.SetBool("isEquip", isEquip);
        playerAnimator.SetBool("isLeftFist", isLeftFist);
        playerAnimator.SetFloat("AttackSpeed", attackSpeed);

        CheckAttack();
    }

    private void CheckAttack() {
        SetMoveSpeedOnAttack();
        if (Input.GetMouseButton(0)) {
            if (!EventSystem.current.IsPointerOverGameObject()) {
                if (!isAttack) {
                    if (!playerAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
                        moveSpeed = playerMove.GetPlayerMoveSpeed();
                    isLeftFist = Random.Range(0, 2) == 0 ? true : false;
                    isAttack = true;

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAttack : MonoBehaviour {
    [SerializeField] private Collider[] fistCollider;
    private PlayerAbility playerAbility;
    private Animator playerAnimator;
    private PlayerMove playerMove;
    private PlayerWeaponEquip playerWeaponEquip;

    private bool isAttack, isEquip, isLeftFist;
    public void SetEquip(bool flag) { isEquip = flag; }

    private float moveSpeed;
    public float attackSpeed { get; private set; }
    public void SetAttackSpeed(float attackspeed) { attackSpeed = attackspeed; }

    private void Awake() {
        playerAbility = GetComponent<PlayerAbility>();
        playerAnimator = GetComponentInParent<Animator>();
        playerMove = GetComponent<PlayerMove>();
        moveSpeed = playerMove.GetPlayerMoveSpeed();
        fistCollider = GetComponentsInChildren<Collider>();
    }

    private void Start() {
        SetAttackSpeed(GetComponent<PlayerAbility>().GetTotalPlayerAttackSpeed());
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
                    if (!playerAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) {
                        moveSpeed = playerMove.GetPlayerMoveSpeed();
                        playerMove.SetDash();
                    }
                    playerMove.SetSideWalk(false);
                    playerMove.SetBackWalk(false);
                    playerMove.ResetDash();

                    isAttack = true;
                    isLeftFist = Random.Range(0, 2) == 0 ? true : false;
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

    private void OnTriggerEnter(Collider other) {
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) {
            //other.GetComponent<ObjAttack>().GetAttack(
            //    playerAbility.GetTotalPlayerAttack, playerAbility.GetTotalPlayerGather());
        }
    }

}

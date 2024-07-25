using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAttack : MonoBehaviour {
    private PlayerWeaponEquip playerWeaponEquip;
    [SerializeField] private Collider[] fistCollider;
    
    private PlayerAbility playerAbility;
    private PlayerMove playerMove;
    private LayerMask targetAttacklayer;

    private Animator playerAnimator;
    private AnimatorStateInfo animatorState;
    private int currentClip = 0;

    private bool isAttack, isEquip, isLeftFist;
    public void SetEquip(bool flag) { Debug.Log("LWOW"); isEquip = flag; }

    private float moveSpeed;
    public float attackSpeed { get; private set; }
    public void SetAttackSpeed(float attackspeed) { attackSpeed = attackspeed; }

    private void Awake() {
        playerWeaponEquip = FindObjectOfType<PlayerWeaponEquip>();
        playerAnimator = GetComponentInParent<Animator>();
        playerAbility = GetComponent<PlayerAbility>();
        playerMove = GetComponent<PlayerMove>();

        fistCollider = GetComponentsInChildren<SphereCollider>();
        moveSpeed = playerMove.GetPlayerMoveSpeed();
    }

    private void Start() {
        SetAttackSpeed(GetComponent<PlayerAbility>().GetTotalPlayerAttackSpeed());
        targetAttacklayer =
            (1 << LayerMask.NameToLayer("Animal")) | (1 << LayerMask.NameToLayer("Stone")) | 
            (1 << LayerMask.NameToLayer("Tree"));
    }

    private void Update() {
        playerAnimator.SetBool("isAttack", isAttack);
        playerAnimator.SetBool("isEquip", isEquip);
        playerAnimator.SetBool("isLeftFist", isLeftFist);
        playerAnimator.SetFloat("AttackSpeed", attackSpeed);

        CheckAttack();
    }

    private bool keepReseted;
    private void CheckAttack() {
        SetMoveSpeedOnAttack();
        if (Input.GetMouseButton(0)) {
            if (!EventSystem.current.IsPointerOverGameObject()) {
                if (!isAttack) {
                    if (!playerAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) {
                        moveSpeed = playerMove.GetPlayerMoveSpeed();
                    }
                    isAttack = true;

                    playerMove.SetSideWalk(false);
                    playerMove.SetBackWalk(false);
                    playerMove.ResetDash();
                isLeftFist = Random.Range(0, 2) == 0 ? true : false;
                    //TODO: isLeftFist 매 공격시마다 초기화하기. 아마 Movement BlendTree 아니면 Exit 에서?
                }
            }
        }
        else {
            isAttack = false;
            if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) {
                playerMove.ResetDash();
                keepReseted = true;

            }
            else if(keepReseted) {
                keepReseted = false;
                playerMove.SetDash();
                currentClip = 0;
            }
        }


    }

    private void SetMoveSpeedOnAttack() {
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) {
            playerMove.SetPlayerMoveSpeed(moveSpeed * 0.4f);
        }
        else playerMove.SetPlayerMoveSpeed(moveSpeed);
    }


    private void OnTriggerEnter(Collider other) {
        animatorState = playerAnimator.GetCurrentAnimatorStateInfo(0);
        if (animatorState.IsTag("Attack") &&
            ((targetAttacklayer.value & (1 << other.gameObject.layer)) != 0)) {
            //Debug.Log($"{currentClip} == {GetCurrentClip()} ? : {currentClip == GetCurrentClip()}");
            if (currentClip == GetCurrentClip()) return;
            currentClip = GetCurrentClip();

            if(other.gameObject.layer == LayerMask.NameToLayer("Animal")) {
                // 동물 Attack 처리
            }
            else {
                // 나무 돌 Attack 처리
            }


        }
    }

    private int GetCurrentClip() {
        return animatorState.shortNameHash + (isLeftFist ? 1 : 2);
    }
}

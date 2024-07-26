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

    public bool isNowDrag;
    public void SetEquip(bool flag) { isEquip = flag; }

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
        if (isNowDrag) return;
        animatorState = playerAnimator.GetCurrentAnimatorStateInfo(0);
        SetMoveSpeedOnAttack();
        if (Input.GetMouseButton(0)) {
            if (!EventSystem.current.IsPointerOverGameObject()) {
                if (!isAttack) {
                    if (!animatorState.IsTag("Attack")) {
                        moveSpeed = playerMove.GetPlayerMoveSpeed();
                        isLeftFist = Random.Range(0, 2) == 0 ? true : false;
                    }
                    isAttack = true;

                    playerMove.SetSideWalk(false);
                    playerMove.SetBackWalk(false);
                    playerMove.ResetDash();
                }
            }
        }
        else {
            isAttack = false;
            if (animatorState.IsTag("Attack")) {
                playerMove.ResetDash();
                keepReseted = true;

            }
            else if(keepReseted) {
                keepReseted = false;
                playerMove.SetDash();
                currentClip = 0;
            }
        }

        if(animatorState.IsName("Punching2") && animatorState.normalizedTime >= 0.93f) {
            isLeftFist = Random.Range(0, 2) == 0 ? true : false;
        }
    }

    private void SetMoveSpeedOnAttack() {
        if (animatorState.IsTag("Attack")) {
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
                // 悼拱 Attack 贸府
            }
            else {
                // 唱公 倒 Attack 贸府
            }


        }
    }

    private int GetCurrentClip() {
        return animatorState.shortNameHash + (isLeftFist ? 1 : 2);
    }
}

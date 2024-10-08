using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAttack : MonoBehaviour {
    private PlayerWeaponEquip playerWeaponEquip;
    [SerializeField] private Collider[] fistCollider;

    private ShelterManager shelterManager;
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

        shelterManager = FindObjectOfType<ShelterManager>();
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
        if (animatorState.IsName("Punching1") && animatorState.normalizedTime <= 0.5f) {
            AudioManager.instance.PlaySFX(AudioManager.Sfx.PlayerAttack);
        }
        else if (animatorState.IsName("Punching2") && animatorState.normalizedTime <= 0.3f) {
            AudioManager.instance.PlaySFX(AudioManager.Sfx.PlayerAttack);
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
                // 동물 Attack 처리
                //other.gameObject.GetComponent<AnmlAttack>().GetAttack(playerAbility.GetTotalPlayerAttack());
                EarnAttackExp();
                Debug.Log(playerAbility.GetTotalPlayerAttack());    // 전달해야 할 공격력
            }
            else {
                // 나무 돌 Attack 처리
                other.gameObject.GetComponent<ObjAttack>().GetAttack(playerAbility.GetTotalPlayerAttack(), playerAbility.GetTotalPlayerGather());
                EarnGatherExp();
                Debug.Log(playerAbility.GetTotalPlayerGather());    // 전달해야 할 채집량
            }
        }
    }

    private void EarnAttackExp() {
        shelterManager.AddAttackExp(
            playerAbility.GetTotalPlayerAttack() * Random.Range(0.1f, 0.6f) * 50f);
    }

    private void EarnGatherExp() {
        shelterManager.AddGatherExp(
            playerAbility.GetTotalPlayerGather() * Random.Range(0.1f, 0.4f) * 50f);
    }

    private int GetCurrentClip() {
        return animatorState.shortNameHash + (isLeftFist ? 1 : 2);
    }
}

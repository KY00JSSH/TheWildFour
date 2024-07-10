using System.Collections;
using UnityEngine;



public class Campfire : FireObject {
    private ParticleSystem fireEffect;

    protected override void Awake() {
        base.Awake();

        fireEffect = GetComponentInChildren<ParticleSystem>();
        fireEffect.Stop();

        LightOff();
    }

    private void Start() {
        // 총 지속시간 100 . 1초 당 감소시간 5
        // 기본 20초 지속
        totalTime = 100f;
        currentTime = 0f;
        tickTime = 2f;
    }

    private void Update() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, HeatRange);

        isPlayerNear = false;
        foreach (Collider collider in colliders) {
            if (collider.CompareTag("Player")) {
                if (collider.transform.parent.
                    TryGetComponent(out PlayerStatus playerStatus)) {
                    isPlayerNear = true;
                    StatusControl.Instance.GiveStatus(Status.Heat, playerStatus);
                    break;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && isPlayerNear) {
            AddWood();
        }
        LightUp(Mathf.InverseLerp(0, totalTime, currentTime) * 4f);
    }

    protected override void AddWood() {
        base.AddWood();
        if (currentTime > 0 && !isBurn) {
            StartCoroutine(Burn());
        }
    }

    private IEnumerator Burn() {
        isBurn = true;
        fireEffect.Play();

        while (currentTime > 0) {
            currentTime -= tickTime;
            yield return new WaitForSeconds(1f);
        }
        currentTime = 0;
        LightOff();
        isBurn = false;
        fireEffect.Stop();
    }

}

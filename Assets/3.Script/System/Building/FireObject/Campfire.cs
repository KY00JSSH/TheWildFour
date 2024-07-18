using System.Collections;
using UnityEngine;



public class Campfire : FireObject {
    private ParticleSystem fireEffect;
    private CampfireUI campfireUI;

    protected override void OnCreated() {
        base.OnCreated();
        campfireUI.FireSliderInit();
        fireEffect = GetComponentInChildren<ParticleSystem>();
        fireEffect.Stop();
        LightOff();
    }

    protected override void Awake() {
        base.Awake();
        campfireUI = GetComponent<CampfireUI>();
        OnCreated();
    }

    private void Start() {
        totalTime = 100f;
        currentTime = 0f;
        tickTime = 2f;
    }

    private void Update() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, HeatRange);

        isPlayerNear = false;
        foreach (Collider collider in colliders) {
            if (collider.CompareTag("Player")) {
                isPlayerNear = true;
                PlayerStatus playerStatus = collider.GetComponentInChildren<PlayerStatus>();
                StatusControl.Instance.GiveStatus(Status.Heat, playerStatus);
                break;
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
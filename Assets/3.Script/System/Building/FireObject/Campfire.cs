using System.Collections;
using UnityEngine;



public class Campfire : FireObject {
    private ParticleSystem fireEffect;
    private CampfireUI campfireUI;

    protected override void OnCreated() {
        base.OnCreated();
        campfireUI.SliderInit();
        fireEffect = GetComponentInChildren<ParticleSystem>();
        fireEffect.Stop();
        LightOff();
    }

    protected override void Awake() {
        base.Awake();
        campfireUI = GetComponent<CampfireUI>();
        OnCreated();
    }

    private void OnEnable() {
        OnCreated();
    }

    private void Start() {
        totalTime = 100f;
        currentTime = 0f;
        tickTime = 2f;
    }

    private void Update() {
        if (currentTime > 0) {
            LightUp(Mathf.InverseLerp(0, totalTime, currentTime) * 4f);
            Collider[] colliders = Physics.OverlapSphere(transform.position, HeatRange);
            foreach (Collider collider in colliders) {
                if (collider.CompareTag("Player")) {
                    PlayerStatus playerStatus = collider.GetComponentInChildren<PlayerStatus>();
                    StatusControl.Instance.GiveStatus(Status.Heat, playerStatus);
                    break;
                }
            }
        }
    }
    
    public override void AddWood() {
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

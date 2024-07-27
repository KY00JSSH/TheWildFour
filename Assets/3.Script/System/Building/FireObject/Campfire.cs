using System.Collections;
using UnityEngine;

public class Campfire : FireObject {
    private CampfireUI campfireUI;

    protected override void OnCreated() {
        base.OnCreated();
        campfireUI.SliderInit();
        fireEffect = GetComponentInChildren<ParticleSystem>();
        fireEffect.Stop();
        LightOff();
    }

    private void Awake() {
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

    protected override void Update() {
        base.Update();
        LightUp(Mathf.InverseLerp(0, totalTime, currentTime) * 4f);
    }
}

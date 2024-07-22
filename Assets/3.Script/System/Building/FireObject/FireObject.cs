using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireObject : MonoBehaviour, IFireLight {
    protected float totalTime, currentTime, tickTime;
    protected float HeatRange = 5f;
    protected bool isBurn = false;

    protected bool isBuilding = true;

    private Light fireLight;

    public float GetTotalTime() { return totalTime; }
    public float GetCurrentTime() { return currentTime; }

    protected virtual void Awake() {
    }

    public void LightUp(float intensity) {
        if (!isBuilding)
            fireLight.intensity = intensity;
    }

    public void LightOff() {
        fireLight.intensity = 0;
    }

    public virtual void AddWood() {
        IncreaseTime(10f);
    }

    protected void IncreaseTime(float time) {
        currentTime += time;
        if (currentTime > totalTime) currentTime = totalTime;
    }

    protected virtual void OnCreated() {
        isBuilding = false;
        fireLight = GetComponentInChildren<Light>();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireObject : MonoBehaviour, IFireLight {
    protected float totalTime, currentTime, tickTime;
    protected float HeatRange = 5f;
    protected bool isBurn = false;

    // isPlayerNaer : 플레이어가 근처에 있을 때 true.
    //TODO : 추후 모닥불이 Lock 되었을 때로 변경 필요. 0708
    protected bool isPlayerNear = false;
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

    protected virtual void AddWood() {
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

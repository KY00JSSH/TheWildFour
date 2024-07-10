using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireObject : MonoBehaviour, IFireLight {
    protected float totalTime, currentTime, tickTime;
    protected float HeatRange = 5f;
    protected bool isBurn = false;

    // isPlayerNaer : �÷��̾ ��ó�� ���� �� true.
    // TODO : ���� ��ں��� Lock �Ǿ��� ���� ���� �ʿ�. 0708
    protected bool isPlayerNear = false;

    private Light fireLight;

    public float GetTotalTime() { return totalTime; }
    public float GetCurrentTime() { return currentTime; }

    protected virtual void Awake() {
        fireLight = GetComponentInChildren<Light>();
    }

    public void LightUp(float intensity) {
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
    }

}

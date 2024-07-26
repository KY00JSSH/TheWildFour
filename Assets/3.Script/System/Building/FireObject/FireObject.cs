using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireObject : MonoBehaviour, IFireLight {
    protected float totalTime, currentTime, tickTime;
    protected float HeatRange = 5f;
    protected bool isBurn = false;

    protected bool isBuilding = true;

    private Light fireLight;
    protected ParticleSystem fireEffect;
    private InvenController invenCont;

    public float GetTotalTime() { return totalTime; }
    public float GetCurrentTime() { return currentTime; }

    public void LightUp(float intensity) {
        if (!isBuilding)
            fireLight.intensity = intensity;
    }

    public void LightOff() {
        fireLight.intensity = 0;
    }

    public virtual void AddWood() {
        invenCont = FindObjectOfType<InvenController>();
        if (invenCont.isInItem(1)) {
            invenCont.removeItemCount(1, 1);
            invenCont.updateInvenInvoke();
            IncreaseTime(10f);
            if (currentTime > 0 && !isBurn) {
                StartCoroutine(Burn());
            }
        }
    }

    private IEnumerator Burn() {
        isBurn = true;
        fireEffect?.Play();

        while (currentTime > 0) {
            currentTime -= tickTime;
            yield return new WaitForSeconds(1f);
        }
        currentTime = 0;
        LightOff();
        isBurn = false;
        fireEffect?.Stop();
    }

    protected void IncreaseTime(float time) {
        currentTime += time;
        if (currentTime > totalTime) currentTime = totalTime;
    }

    protected virtual void OnCreated() {
        isBuilding = false;
        fireLight = GetComponentInChildren<Light>();
    }

    protected virtual void Update() {
        if (currentTime > 0) {
            Collider[] colliders = Physics.OverlapSphere(transform.position, HeatRange);
            foreach (Collider collider in colliders) {
                if (collider.CompareTag("Player")) {
                    PlayerStatus playerStatus = collider.GetComponentInParent<PlayerStatus>();
                    StatusControl.Instance.GiveStatus(Status.Heat, playerStatus);
                    break;
                }
            }
        }
    }
}

using System.Collections;
using UnityEngine;

public class Furnace : FireObject {
    private FurnaceUI furnaceUI;
    private float currentIntensity, targetIntensity;
    protected override void OnCreated() {
        base.OnCreated();
        furnaceUI.SliderInit();
        fireEffect = null;        
        LightOff();
    }

    private void Awake() {
        furnaceUI = GetComponent<FurnaceUI>();
        OnCreated();
    }

    private void OnEnable() {
        OnCreated();
    }

    private void Start() {
        totalTime = 100f;
        currentTime = 0f;
        tickTime = 1.1f;

        targetIntensity = 10f;
        currentIntensity = 0f;
    }

    protected override void Update() {
        base.Update();
        LightUp(currentIntensity);
        if (currentTime > 0) {
            if (Mathf.Abs(targetIntensity - currentIntensity) > 0.01f) {
                currentIntensity +=
                    Time.deltaTime * 10f * (targetIntensity - currentIntensity > 0 ? 1 : -1);
            }
            else {
                targetIntensity = Random.Range(2f, 8f);
            }
        }
        else
            currentIntensity = 0;
    }

}

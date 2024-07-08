using System.Collections;
using UnityEngine;


public class Campfire : MonoBehaviour, IFireLight {
    private ParticleSystem fireEffect;
    private Light fireLight;

    private float totalTime, currentTime, tickTime;
    private float HeatRange = 5f;
    private bool isBurn = false;

    // isPlayerNaer : 플레이어가 근처에 있을 때 true.
    // TODO : 추후 모닥불이 Lock 되었을 때로 변경 필요. 0708
    private bool isPlayerNear = false;

    private void Awake() {
        fireEffect = GetComponentInChildren<ParticleSystem>();
        fireEffect.Stop();

        fireLight = GetComponentInChildren<Light>();
        LightOff();
    }

    private void Start() {
        // 총 지속시간 100 . 1초 당 감소시간 5
        // 기본 20초 지속
        totalTime = 100f;
        currentTime = 0f;
        tickTime = 5f;
    }

    public void AddWood() {
        IncreaseTime(10f);
        // TODO : 빛을 밝히는 로직 필요. 0708

        if (currentTime > 0 && !isBurn) {
            StartCoroutine(Burn());
        }
    }

    public void IncreaseTime(float time) {
        currentTime += time;
    }

    public void LightUp(float intensity) {
        fireLight.intensity = intensity;
    }

    public void LightOff() {
        fireLight.intensity = 0;
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

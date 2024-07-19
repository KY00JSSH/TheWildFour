using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCoolTimeUI : MonoBehaviour {
    public Image img;
    public Button btn;
    [SerializeField] private float cooltime = 10f;
    [SerializeField] private bool isClicked = false;

    public float CoolTime { get { return leftTime; } } // ShelterManager에서 받아가는 용도
    public bool isBuildComplete { get; private set; }

    private float leftTime = 10f;
    [SerializeField] private float speed = 5.0f;
    private ShelterManager shelterManager;
    private Tooltip_Shelter tooltip_Shelter;
    private Tooltip_Workshop tooltip_Workshop;

    public float ratio { get; private set; }

    private void OnDisable() {
        isClicked = false;
        ratio = 1;
        img.fillAmount = 1;
    }

    private void Start() {
        if (tooltip_Shelter == null) tooltip_Shelter = FindObjectOfType<Tooltip_Shelter>();
        if (tooltip_Workshop == null) tooltip_Workshop = FindObjectOfType<Tooltip_Workshop>();
        if (shelterManager == null) shelterManager = FindObjectOfType<ShelterManager>();
        if (img == null) img = gameObject.GetComponent<Image>();
        if (btn == null) btn = gameObject.GetComponent<Button>();
    }

    public void Update() {
        if (isClicked) {
            if (leftTime > 0) {
                leftTime -= Time.deltaTime;
                if (leftTime < 0) {
                    leftTime = 0f;
                    if (btn) btn.enabled = true;
                    isClicked = false;
                    isBuildComplete = true;
                }

                ratio = 1.0f - (leftTime / cooltime);
                if (img) img.fillAmount = ratio;
            }
        }
    }

    public void StartSleepCooltime() {
        if (btn.name.Contains("Move") && shelterManager.MovePoint <= 0) return;
        else if (btn.name.Contains("Attack") && shelterManager.AttackPoint <= 0) return;
        else if (btn.name.Contains("Gather") && shelterManager.GatherPoint <= 0) return;

        leftTime = cooltime;
        leftTime = 10f;
        isBuildComplete = false;
        isClicked = true;
        if (btn) btn.enabled = false;
    }
    public void StartUpgradeCooltime() {
        Debug.Log(" build 확인" + tooltip_Shelter.isUpgradeAvailable);
        if (!tooltip_Shelter.isUpgradeAvailable) return;

        leftTime = cooltime;
        leftTime = 10f;
        isBuildComplete = false;
        isClicked = true;
        if (btn) btn.enabled = false;
    }

    public void StartUpgradeCooltime_WS() {
        //Debug.Log(" build 확인" + tooltip_Workshop.isUpgradeAvailable);
        //if (!tooltip_Workshop.isUpgradeAvailable) return;

        leftTime = cooltime;
        leftTime = 10f;
        isBuildComplete = false;
        isClicked = true;
        if (btn) btn.enabled = false;
    }
}

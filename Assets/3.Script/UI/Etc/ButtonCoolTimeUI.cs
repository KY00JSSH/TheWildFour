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
    public bool isBuildComplete = false;
    private bool isRatioOpposite = false;

    private float leftTime;
    [SerializeField] private float speed = 5.0f;
    private ShelterManager shelterManager;
    private WorkshopManager workshopManager;
    private Tooltip_Shelter tooltip_Shelter;
    private Tooltip_Workshop tooltip_Workshop;

    public float ratio { get; private set; }

    private void OnDisable() {
        if (btn) btn.enabled = true;
        img.enabled = false;
        leftTime = cooltime;
        isClicked = false;
        ratio = 1.0f - (leftTime / cooltime);
        if (isRatioOpposite) ratio = 1.0f - ratio;
        img.fillAmount = ratio;

    }

    private void Start() {
        leftTime = cooltime;
        if (tooltip_Shelter == null) tooltip_Shelter = FindObjectOfType<Tooltip_Shelter>();
        if (tooltip_Workshop == null) tooltip_Workshop = FindObjectOfType<Tooltip_Workshop>();
        if (shelterManager == null) shelterManager = FindObjectOfType<ShelterManager>();
        if (workshopManager == null) workshopManager = FindObjectOfType<WorkshopManager>();
        if (img == null) img = gameObject.transform.GetChild(0).GetComponent<Image>();
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
                    Debug.Log("쿨타임 확인 : " + isBuildComplete);
                    isBuildComplete = true;
                    OnCoolTimeEnd();
                    img.enabled = false;
                }

                ratio = 1.0f - (leftTime / cooltime);
                if (isRatioOpposite) ratio = 1.0f - ratio;
                if (img) img.fillAmount = ratio;
            }
        }
    }

    public void OnCoolTimeEnd() {
        if (this.name == "Ws_Upgrade" || this.name == "Shlt_Upgrade") {
            InvenController invenController = FindObjectOfType<InvenController>();
            TooltipNum tooltipNum = FindObjectOfType<TooltipNum>();
            UIInfo UIinfo;

            UpgradeType buildingType = UpgradeType.Workshop;
            int buildingLevel = 0;

            UIinfo = GetComponentInParent<ShelterUI>();
            if (UIinfo != null) {
                buildingType = UpgradeType.Shelter;
                buildingLevel = FindObjectOfType<ShelterManager>().ShelterLevel + 1;
            }
            else {
                UIinfo = GetComponentInParent<WorkShopUI>();
                if(UIinfo!=null) {
                    buildingType = UpgradeType.Workshop;
                    buildingLevel = FindObjectOfType<WorkshopManager>().WorkshopLevel + 1;
                }
            }

            if (UIinfo != null) {
                invenController.buildingCreateUseItem(
                    tooltipNum.UpgradeItemCheck(buildingType, buildingLevel).needItems);
            }
        }
    }


    public void StartSkillCooltime_Shelter() {
        ShelterUI shelterUI = FindObjectOfType<ShelterUI>();
        Debug.Log("????????????? / " + shelterUI.isShleterSkillAvailable);
        if (!shelterUI.isShleterSkillAvailable) return;
        StartButtonInit();
        isRatioOpposite = true;
    }
    public void StartSleepCooltime() {
        StartButtonInit();
        isRatioOpposite = true;
    }
    public void StartUpgradeCooltime() {
        if (shelterManager.ShelterLevel == shelterManager.MaxShelterLevel) return;
        Debug.Log(" build 확인" + tooltip_Shelter.isUpgradeAvailable);
        if (!tooltip_Shelter.isUpgradeAvailable) return;
        StartButtonInit();
    }

    public void StartUpgradeCooltime_WS() {
        if (workshopManager.WorkshopLevel == workshopManager.MaxWorkshopLevel) return;
        Debug.Log(" build 확인" + tooltip_Workshop.isWSUpgradeAvailable);
        if (!tooltip_Workshop.isWSUpgradeAvailable) return;
        StartButtonInit();
    }
    public void StartItemCooltime_WS() {
        if (!tooltip_Workshop.isWSSkillAvailable) return;
        StartButtonInit();
    }


    private void StartButtonInit() {
        img.enabled = true;
        leftTime = cooltime;
        isBuildComplete = false;
        isClicked = true;
        if (btn) btn.enabled = false;
    }

}

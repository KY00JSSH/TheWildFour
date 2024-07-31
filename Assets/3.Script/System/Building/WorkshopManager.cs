using System.Collections;
using UnityEngine;

public class WorkshopManager : MonoBehaviour { 
    public int WorkshopLevel { get; private set; }
    public int MaxWorkshopLevel { get; private set; }

    private TooltipNum tooltipNum;
    private InvenController invenCont;

    WorkshopCreate workshop;
    private void Awake() {
        workshop = GetComponent<WorkshopCreate>();
        tooltipNum = FindObjectOfType<TooltipNum>();
        invenCont = FindObjectOfType<InvenController>();
    }

    public void LevelUp() {
        //TODO: Workshop upgrade item 부족하면 return
        if (WorkshopLevel == MaxWorkshopLevel) return;

        Destroy(workshop.Building.GetComponent<Rigidbody>());
        StartCoroutine(WaitForUpgrade());
    }

    [SerializeField] ButtonCoolTimeUI upgradeCooltime;
    private IEnumerator WaitForUpgrade() {
        while (upgradeCooltime.CoolTime > 0)
            yield return null;

        Transform shelterPosition = workshop.Building.transform;
        workshop.Building.SetActive(false);
        WorkshopLevel++;
        workshop.Building.transform.position = shelterPosition.position;
        workshop.Building.transform.rotation = shelterPosition.rotation;
        workshop.Building.SetActive(true);
    }

    private void Start() {
        WorkshopLevel = Save.Instance.saveData.workshopLevel;
        MaxWorkshopLevel = 5;
    }
}

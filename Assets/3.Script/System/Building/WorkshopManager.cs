using System.Collections;
using UnityEngine;

public class WorkshopManager : MonoBehaviour { 
    public int WorkshopLevel { get; private set; }

    WorkshopCreate workshop;
    private void Awake() {
        workshop = GetComponent<WorkshopCreate>();
    }

    public void LevelUp() {
        //TODO: Workshop upgrade item 부족하면 return

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
        //TODO: SAVE 구현 시 JSON에서 받아오기
        WorkshopLevel = 1;
    }
}
//TODO: 작업장 UI 생성 이후 CoolTime 인스펙터에서 매칭시켜줘야 함

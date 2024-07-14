using System.Collections;
using UnityEngine;

public class WorkshopManager : MonoBehaviour { 
    public int WorkshopLevel { get; private set; }
    public void LevelUp() {
        StartCoroutine(WaitForUpgrade());
    }

    [SerializeField] ButtonCoolTimeUI upgradeCooltime;
    private IEnumerator WaitForUpgrade() {
        ShelterCreate shelter = GetComponent<ShelterCreate>();

        while (upgradeCooltime.CoolTime > 0)
            yield return null;

        Transform shelterPosition = shelter.Building.transform;
        shelter.Building.SetActive(false);
        WorkshopLevel++;
        shelter.Building.transform.position = shelterPosition.position;
        shelter.Building.transform.rotation = shelterPosition.rotation;
        shelter.Building.SetActive(true);
    }

    private void Start() {
        //TODO: SAVE 구현 시 JSON에서 받아오기
        WorkshopLevel = 1;
    }
}
//TODO: 작업장 UI 생성 이후 CoolTime 인스펙터에서 매칭시켜줘야 함

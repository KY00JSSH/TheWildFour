using System.Collections;
using UnityEngine;

public class WorkshopManager : MonoBehaviour { 
    public int WorkshopLevel { get; private set; }

    ShelterCreate shelter;
    private void Awake() {
        shelter = GetComponent<ShelterCreate>();
    }

    public void LevelUp() {
        //TODO: Workshop upgrade item �����ϸ� return

        Destroy(shelter.Building.GetComponent<Rigidbody>());
        StartCoroutine(WaitForUpgrade());
    }

    [SerializeField] ButtonCoolTimeUI upgradeCooltime;
    private IEnumerator WaitForUpgrade() {
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
        //TODO: SAVE ���� �� JSON���� �޾ƿ���
        WorkshopLevel = 1;
    }
}
//TODO: �۾��� UI ���� ���� CoolTime �ν����Ϳ��� ��Ī������� ��

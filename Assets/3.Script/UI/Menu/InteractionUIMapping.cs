using UnityEngine;

// 240719 JHH
// BuildingInteraction Component에 UI GameObject 할당해주기 위한 스크립트
public class InteractionUIMapping : MonoBehaviour {
    [SerializeField] GameObject shelterUI, workshopUI;
    public GameObject ShelterUI { get { return shelterUI; } }
    public GameObject WorkShopUI { get { return workshopUI; } }

}
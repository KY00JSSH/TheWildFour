using UnityEngine;

// 240719 JHH
// BuildingInteraction Component�� UI GameObject �Ҵ����ֱ� ���� ��ũ��Ʈ
public class InteractionUIMapping : MonoBehaviour {
    [SerializeField] GameObject shelterUI, workshopUI;
    public GameObject ShelterUI { get { return shelterUI; } }
    public GameObject WorkShopUI { get { return workshopUI; } }
}
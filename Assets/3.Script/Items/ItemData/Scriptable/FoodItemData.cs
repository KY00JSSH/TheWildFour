using UnityEngine;

[CreateAssetMenu(fileName = "FoodItemData", menuName = "Items/FoodItemData", order = 3)]
public class FoodItemData : CountableItemData {
    [SerializeField] private float healTime;            //ȸ�� �ð�
    [SerializeField] private float totalDecayTime;      //��ü ���� �ð�
    [SerializeField] private bool isRoasted;            //���� �������� ����. ���� �ʰ� ������ �ִ� �������̾ true

    public float HealTime { get { return healTime; } }
    public float TotalDecayTime { get { return totalDecayTime; } }
    public bool IsRoasted { get { return isRoasted; } }
}

using UnityEngine;

[CreateAssetMenu(fileName = "MedcItemData", menuName = "Items/MedcItemData", order = 5)]
public class MedicItemData : CountableItemData {
    [SerializeField] private float healTime;        //ȸ�� �ð�
    [SerializeField] private int[] materialKey;     //��� ������ key
    [SerializeField] private int[] materialCount;   //��� ������ ����
    [SerializeField] private int level;             //����

    public float HealTime { get { return healTime; } }
    public int[] MaterialKey { get { return materialKey; } }
    public int[] MaterialCount { get { return materialCount; } }
    public int Level { get { return level; } }
}

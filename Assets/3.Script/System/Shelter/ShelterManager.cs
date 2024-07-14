using UnityEngine;

// ############ ���� ���� ���� ############ //

public class ShelterManager : MonoBehaviour {
    public int ShelterLevel { get; private set; }
    private Vector3 LastPlayerPosition; //TODO: ��ó ����, ����� ����� ��ġ��

    public int MoveLevel { get; private set; }
    public int AttackLevel { get; private set; }
    public int GatherLevel { get; private set; }

    public int MovePoint { get; private set; }
    public int AttackPoint { get; private set; }
    public int GatherPoint { get; private set; }

    public float MoveTotalExp { get; private set; }
    public float AttackTotalExp { get; private set; }
    public float GatherTotalExp { get; private set; }

    public float MoveCurrentExp { get; private set; }
    public float AttackCurrentExp { get; private set; }
    public float GatherCurrentExp { get; private set; }

    private void Start() {
        //TODO: SAVE ���� �� JSON���� �޾ƿ���
        ShelterLevel = 1;

        MoveLevel = 0;
        AttackLevel = 0;
        GatherLevel = 0;
        
        MovePoint = 0;
        AttackPoint = 0;
        GatherPoint = 0;

        MoveTotalExp = 120f;
        AttackTotalExp = 100f;
        GatherTotalExp = 300f;

        MoveCurrentExp = 0;
        AttackCurrentExp = 0;
        GatherCurrentExp = 0;
    }

    public void AddMoveExp(float exp) {
        MoveCurrentExp += exp;
        if(MoveCurrentExp > MoveTotalExp) {
            MoveCurrentExp -= MoveTotalExp;
            MovePoint++;
            MoveLevel++;
            MoveTotalExp += MoveLevel * 4;
        }
    }

    public void AddAttackExp(float exp) {
        AttackCurrentExp += exp;
        if(AttackCurrentExp > AttackTotalExp) {
            AttackCurrentExp -= AttackTotalExp;
            AttackPoint++;
            AttackLevel++;
            AttackTotalExp += AttackLevel * 4;
        }
    }

    public void AddGatherExp(float exp) {
        GatherCurrentExp += exp;
        if(GatherCurrentExp > GatherTotalExp) {
            GatherCurrentExp -= GatherTotalExp;
            GatherPoint++;
            GatherLevel++;
            GatherTotalExp += GatherLevel * 4;
        }
    }


    //TODO: UI > ��ó ���� ��ư '���׷��̵�' ��ư Onclicked => LevelUp();
    public void LevelUp() {     // ��ó ������
        ShelterCreate shelter = GetComponent<ShelterCreate>();
        //TODO: UI > �ڷ�ƾ���� '���׷��̵�' ��ư �����̴� ä��� ����
        shelter.Building.SetActive(false);
        ShelterLevel++;
        shelter.Building.SetActive(true);
    }

    public Skill[] skillMove = new Skill[5];
    public Skill[] skillAttack = new Skill[5];
    public Skill[] skillGather = new Skill[5];


    public void OnSkillMoveButton(int index) {
        if (MovePoint <= 0) return;
        if (index > ShelterLevel) return;
        skillMove[index].LevelUp();
        MovePoint--;
    }

    public void OnSkillAttackButton(int index) {
        if (AttackPoint <= 0) return;
        if (index > ShelterLevel) return;
        skillAttack[index].LevelUp();
        AttackPoint--;
    }

    public void OnSkillGatherButton(int index) {
        if (GatherPoint <= 0) return;
        if (index > ShelterLevel) return;
        skillGather[index].LevelUp();
        GatherPoint--;
    }

    public Skill GetSkill(string name) {
        foreach (Skill skill in skillMove)
            if (skill.skillName == name) return skill;
        foreach (Skill skill in skillAttack)
            if (skill.skillName == name) return skill;
        foreach (Skill skill in skillGather)
            if (skill.skillName == name) return skill;
        return null;
    }
}

// ############ ���� ���� ���� ############ //

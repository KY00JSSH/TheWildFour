using UnityEngine;

// ############ ���� ���� ���� ############ //

public class ShelterManager : MonoBehaviour {
    public int ShelterLevel { get; private set; }
    private Vector3 LastPlayerPosition; //TODO: ��ó ����, ����� ����� ��ġ��

    //TODO: ��ų �� ����Ʈ, ����Ʈ �� ����ġ
    //TODO: ����Ʈ ����ġ�� ��ų �������� �䱸�� ����.
    public int skillMoveLevel, skillAttackLevel, skillGatherLevel;
    public float skillMoveTotalExp, skillAttackTotalExp, skillGatherTotalExp;
    public float skillMoveCurrentExp, skillAttackCurrentExp, skillGatherCurrentExp;
    public bool isShelterLevelUp = false; // ����� 24 07 12 - 16 : 58
    private void Start() {
        //TODO: SAVE ���� �� JSON���� �޾ƿ���
        ShelterLevel = 1;
        skillMoveLevel = 0;
        skillAttackLevel = 0;
        skillGatherLevel = 0;
        skillMoveTotalExp = 0;
        skillAttackTotalExp = 0;
        skillGatherTotalExp = 0;
        skillMoveCurrentExp = 0;
        skillAttackCurrentExp = 0;
        skillGatherCurrentExp = 0;
    }

    //TODO: UI > ��ó ���� ��ư '���׷��̵�' ��ư Onclicked => LevelUp();
    public void LevelUp() {     // ��ó ������
        isShelterLevelUp = true;
        ShelterCreate shelter = GetComponent<ShelterCreate>();
        //TODO: UI > �ڷ�ƾ���� '���׷��̵�' ��ư �����̴� ä��� ����
        shelter.Shelter().SetActive(false);
        ShelterLevel++;
        shelter.Shelter().SetActive(true);
    }

    public Skill[] skillMove = new Skill[8];
    public Skill[] skillAttack = new Skill[8];
    public Skill[] skillGather = new Skill[8];


    public void OnSkillMoveButton(int index) {
        if (index > ShelterLevel) return;
        skillMove[index].LevelUp();
    }

    public void OnSkillAttackButton(int index) {
        if (index > ShelterLevel) return;
        skillAttack[index].LevelUp();
    }

    public void OnSkillGatherButton(int index) {
        if (index > ShelterLevel) return;
        skillGather[index].LevelUp();
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

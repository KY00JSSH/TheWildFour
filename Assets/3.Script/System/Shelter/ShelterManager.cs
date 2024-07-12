using UnityEngine;

// ############ 임의 수정 금지 ############ //

public class ShelterManager : MonoBehaviour {
    public int ShelterLevel { get; private set; }
    private Vector3 LastPlayerPosition; //TODO: 거처 입장, 퇴장시 사용할 위치값

    //TODO: 스킬 당 포인트, 포인트 당 경험치
    //TODO: 포인트 경험치는 스킬 레벨마다 요구량 증가.

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
        //TODO: SAVE 구현 시 JSON에서 받아오기
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



    //TODO: UI > 거처 내부 버튼 '업그레이드' 버튼 Onclicked => LevelUp();
    public void LevelUp() {     // 거처 레벨업
        ShelterCreate shelter = GetComponent<ShelterCreate>();
        //TODO: UI > 코루틴으로 '업그레이드' 버튼 슬라이더 채우기 로직
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

// ############ 임의 수정 금지 ############ //

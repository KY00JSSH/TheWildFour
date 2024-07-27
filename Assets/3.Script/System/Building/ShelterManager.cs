using System.Collections;
using UnityEngine;

// ############ 임의 수정 금지 ############ //

public class ShelterManager : MonoBehaviour {
    private PlayerAbility playerAbility;
    public int ShelterLevel { get; private set; }
    public int MaxShelterLevel { get; private set; }

    private Vector3 LastPlayerPosition;

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

    private TooltipNum tooltipNum;
    private InvenController invenCont;

    private void Start() {
        playerAbility = FindObjectOfType<PlayerAbility>();
        tooltipNum = FindObjectOfType<TooltipNum>();
        invenCont = FindObjectOfType<InvenController>();

        //TODO: SAVE 구현 시 JSON에서 받아오기
        ShelterLevel = 1;
        MaxShelterLevel = 5;

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
        Debug.Log(GatherPoint);
    }

    public void LevelUp() {     // 거처 레벨업
        if (ShelterLevel == MaxShelterLevel) return;
        // 24 07 18 김수주 Shelter upgrade item 부족하면 return

        Tooltip_Shelter tooltip_Shelter = FindObjectOfType<Tooltip_Shelter>();
        if (!tooltip_Shelter.isUpgradeAvailable) return;

        StartCoroutine(WaitForUpgrade());
    }

    [SerializeField] ButtonCoolTimeUI upgradeCooltime;
    private IEnumerator WaitForUpgrade() {
        ShelterCreate shelter = GetComponent<ShelterCreate>();

        while (upgradeCooltime.CoolTime > 0)
            yield return null;
        Transform shelterPosition = shelter.Building.transform;
        shelter.Building.SetActive(false);
        ShelterLevel++;
        shelter.Building.transform.position = shelterPosition.position;
        shelter.Building.transform.rotation = shelterPosition.rotation;
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
        playerAbility.UpdateAblity();
    }

    public void OnSkillAttackButton(int index) {
        if (AttackPoint <= 0) return;
        if (index > ShelterLevel) return;
        skillAttack[index].LevelUp();
        AttackPoint--;
        playerAbility.UpdateAblity();
    }

    public void OnSkillGatherButton(int index) {
        if (GatherPoint <= 0) return;
        if (index > ShelterLevel) return;
        skillGather[index].LevelUp();
        GatherPoint--;
        playerAbility.UpdateAblity();
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

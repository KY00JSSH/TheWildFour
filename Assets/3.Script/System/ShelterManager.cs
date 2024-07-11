using UnityEngine;

// ############ 임의 수정 금지 ############ //

public class ShelterManager : MonoBehaviour { 
    public int ShelterLevel { get; private set; }
    public void LevelUp() { ShelterLevel++; }

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
}

// ############ 임의 수정 금지 ############ //

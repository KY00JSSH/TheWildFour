[System.Serializable]
public class Skill {
    public string skillName;
    public int maxSkillLevel;
    public int nowSkillLevel;
    public float skillValue;

    public bool LevelUp() {
        if (nowSkillLevel == maxSkillLevel) return false;
        nowSkillLevel++;
        return true;
    }

    public float GetValue() {
        if (skillValue == -1) return 0;
        return skillValue * nowSkillLevel;
    }
}

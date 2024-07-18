using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

[System.Serializable]
public class NeedItem {
    public int ItemKey;
    public string ItemName;
    public int ItemNeedNum;
}

[System.Serializable]
public enum SkillType {
    Move,
    Attack,
    Gather,
    Null
}

[System.Serializable]
public enum UpgradeType {
    Shelter,
    Workshop
}

[System.Serializable]
public class SkillDetail {
    public SkillType skillType;
    public int skillNum;
    public string name;
    public string description;
}

[System.Serializable]
public class BuildDetail {
    public int buttonNum;
    public string name;
    public int buildLevel;
    public string description;
    public NeedItem[] needItems;
}

[System.Serializable]
public class UpgradeDetail {
    public UpgradeType upgradeType;
    public string name;
    public int upgradeLevel;
    public string description;
    public NeedItem[] needItems;
}

[System.Serializable]
public class SleepDetail {
    public string name;
    public string description;
    public NeedItem[] needItems;
    public int totalValue;
    public int currentValue;
}

[System.Serializable]
public class PackingDetail {
    public string name;
    public string description;
}

[System.Serializable]
public class SkillDetailList {
    public List<SkillDetail> skillDetails;
}

public class BuildDetailList {
    public List<BuildDetail> buildDetails;
}

[System.Serializable]
public class UpgradeDetailList {
    public List<UpgradeDetail> upgradeDetails;
}

[System.Serializable]
public class SleepDetailList {
    public List<SleepDetail> sleepDetails;
}

[System.Serializable]
public class PackingDetailList {
    public List<PackingDetail> packingDetails;
}

public class TooltipDetail : MonoBehaviour {
    private string skillDetailFileName = "Detail/skillDetail";
    private string buildDetailFileName = "Detail/buildDetail";
    private string upgradeDetailFileName = "Detail/upgradeDetail";
    private string sleepDetailFileName = "Detail/sleepDetail";
    private string packingDetailFileName = "Detail/packingDetail";

    public SkillDetailList skillDetailList;
    public BuildDetailList buildDetailList;
    public UpgradeDetailList upgradeDetailList;
    public SleepDetailList sleepDetailList;
    public PackingDetailList packingDetailList;

    private void Start() {
        TextAsset skillDetails = Resources.Load<TextAsset>(skillDetailFileName);
        if (skillDetails != null) {
            string skillDetailsAsJson = skillDetails.text;
            skillDetailList = JsonUtility.FromJson<SkillDetailList>(skillDetailsAsJson);
        }
        else {
            Debug.LogError("skillDetailsAsJson 파일 없음");
        }

        TextAsset buildDetails = Resources.Load<TextAsset>(buildDetailFileName);
        if (buildDetails != null) {
            string buildDetailsAsJson = buildDetails.text;
            buildDetailList = JsonUtility.FromJson<BuildDetailList>(buildDetailsAsJson);
        }
        else {
            Debug.LogError("buildDetailsAsJson 파일 없음");
        }

        TextAsset upgradeDetails = Resources.Load<TextAsset>(upgradeDetailFileName);
        if (upgradeDetails != null) {
            string upgradeDetailsAsJson = upgradeDetails.text;
            upgradeDetailList = JsonUtility.FromJson<UpgradeDetailList>(upgradeDetailsAsJson);
        }
        else {
            Debug.LogError("upgradeDetailsAsJson 파일 없음");
        }

        TextAsset sleepDetails = Resources.Load<TextAsset>(sleepDetailFileName);
        if (sleepDetails != null) {
            string sleepDetailsAsJson = sleepDetails.text;
            sleepDetailList = JsonUtility.FromJson<SleepDetailList>(sleepDetailsAsJson);
        }
        else {
            Debug.LogError("sleepDetailsAsJson 파일 없음");
        }

        TextAsset packingDetails = Resources.Load<TextAsset>(packingDetailFileName);
        if (packingDetails != null) {
            string packingDetailsAsJson = packingDetails.text;
            packingDetailList = JsonUtility.FromJson<PackingDetailList>(packingDetailsAsJson);
        }
        else {
            Debug.LogError("packingDetailsAsJson 파일 없음");
        }
    }

}

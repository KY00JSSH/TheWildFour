using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

[System.Serializable]
public class NeedItem {
    int ItemKey;
    int ItemNeedNum;
}

[System.Serializable]
public class SkillDetail {
    public string name;
    public string description;
}

[System.Serializable]
public class BuildDetail {
    public string name;
    public string description;
    public NeedItem[] needItems;
}

[System.Serializable]
public class UpgradeDetail {
    public string name;
    public string description;
    public int upgradeLevel;
    public NeedItem[] needItems;
}

[System.Serializable]
public class SleepDetail {
    public string name;
    public string description;
    public NeedItem[] needItems;
    public int totalValue;
    public int currentValue;
    public Slider sleepTime;
}

[System.Serializable]
public class PackingDetail {
    public string name;
    public string description;
}


public class TooltipText : MonoBehaviour
{

}

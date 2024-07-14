using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;


[System.Serializable]
public class SkillInfo {
    public string Title;
    public string MainText;

    public SkillInfo(string Title, string MainText) {
        this.Title = Title;
        this.MainText = MainText;
    }
}


public class Shelter_Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public int dictionaryKey;

    [Header("Tootip ����")]
    [SerializeField] private ShelterManager shelterManager;
    [SerializeField] private GameObject ShelterTooltip;
    private Image tooltipImg;
    private Text tooltipTitle;
    private Text tooltipMain;
    private Text tooltipAdditionalText;

    [Space((int)2)]
    [Header("Tootip Function �߰�")]
    // Function �� �߰�
    [SerializeField] private Slider sleepTime;
    [SerializeField] private GameObject sleepFunc;
    [SerializeField] private GameObject itemimgs;
    [SerializeField] private GameObject itemtexts;

    //[SerializeField] private Button[] sleepOrAwake;
    private string[] additionalText = { "�ʿ� ����Ʈ 1", "���� �ʿ� ����" };
    private Vector2[] textPositionSave;
    private Vector2[][] itemNeedPositionSave;

    [Space((int)2)]
    [Header("Skill Info")]
    Dictionary<int, SkillInfo> shelterTooltip = new Dictionary<int, SkillInfo>(); public TextAsset textFile_Skill;
    [SerializeField] private List<SkillInfo> skilltexts = new List<SkillInfo>();

    // Function �� �߰�
    [Space((int)2)]
    [Header("Funcion Info")]
    public TextAsset textFile_Func;
    [SerializeField]
    private Dictionary<int, Dictionary<int, ShelterFunctionInfo>> shelterFuncTooltip
        = new Dictionary<int, Dictionary<int, ShelterFunctionInfo>>(){
        { 0, new Dictionary<int, ShelterFunctionInfo>() },
        { 1, new Dictionary<int, ShelterFunctionInfo>() },
        { 2, new Dictionary<int, ShelterFunctionInfo>() }
    };

    private void Awake() {
        TextRead();
        TextReadFunc();
        if (tooltipImg == null) tooltipImg = ShelterTooltip.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        if (tooltipTitle == null) tooltipTitle = ShelterTooltip.transform.GetChild(1).GetComponent<Text>();
        if (tooltipMain == null) tooltipMain = ShelterTooltip.transform.GetChild(0).GetChild(1).GetComponent<Text>();
        if (tooltipAdditionalText == null) tooltipAdditionalText = ShelterTooltip.transform.GetChild(0).GetChild(2).GetComponent<Text>();
        textPositionSave = new Vector2[2];

        itemNeedPositionSave = new Vector2[2][];
        itemNeedPositionSave[0] = new Vector2[4];
        itemNeedPositionSave[1] = new Vector2[4];
        // ��ġ ����Ǵ� Text ��ġ ����
        SaveTextPositions();
        SaveTextPositions_Func();
    }

    private void SaveTextPositions() {
        RectTransform tooltipMainRe = tooltipMain.GetComponent<RectTransform>();
        textPositionSave[0] = tooltipMainRe.anchoredPosition;
        RectTransform tooltipAdditionalTextRe = tooltipAdditionalText.GetComponent<RectTransform>();
        textPositionSave[1] = tooltipAdditionalTextRe.anchoredPosition;
    }
    private void SaveTextPositions_Func() {
        for (int i = 0; i < itemtexts.transform.childCount; i++) {
            RectTransform eachRe = itemtexts.transform.GetChild(i).GetComponent<RectTransform>();
            itemNeedPositionSave[0][i] = eachRe.anchoredPosition;
        }
        for (int i = 0; i < itemimgs.transform.childCount; i++) {
            RectTransform eachRe = itemimgs.transform.GetChild(i).GetComponent<RectTransform>();
            itemNeedPositionSave[1][i] = eachRe.anchoredPosition;
        }
    }

    // ����� Text ��ġ �ҷ����� + Function ��ü�� ��Ȱ��ȭ
    private void LoadTextPositions() {
        // ��� ���� �ʱ�ȭ
        RectTransform tooltipBg = ShelterTooltip.transform.GetChild(0).GetComponent<RectTransform>();
        tooltipBg.sizeDelta = new Vector2(tooltipBg.sizeDelta.x, 350);
        tooltipBg.anchoredPosition = new Vector2(tooltipBg.anchoredPosition.x, 0);

        // Text ��ġ ũ�� �ʱ�ȭ
        RectTransform tooltipMainRe = tooltipMain.GetComponent<RectTransform>();
        tooltipMainRe.anchoredPosition = textPositionSave[0];
        tooltipMainRe.sizeDelta = new Vector2(270, tooltipMainRe.sizeDelta.y);

        RectTransform tooltipAdditionalTextRe = tooltipAdditionalText.GetComponent<RectTransform>();
        tooltipAdditionalTextRe.anchoredPosition = new Vector2(textPositionSave[1].x, 60);

        sleepTime.gameObject.SetActive(false);
        itemimgs.SetActive(false);
        itemtexts.SetActive(false);
    }
    private void LoadTextPositions_Func() {
        for (int i = 0; i < itemtexts.transform.childCount; i++) {
            RectTransform eachRe = itemtexts.transform.GetChild(i).GetComponent<RectTransform>();
            eachRe.anchoredPosition = itemNeedPositionSave[0][i];
        }
        for (int i = 0; i < itemimgs.transform.childCount; i++) {
            RectTransform eachRe = itemimgs.transform.GetChild(i).GetComponent<RectTransform>();
            eachRe.anchoredPosition = itemNeedPositionSave[1][i];
        }
    }


    // �����  dictionary�� ����
    private void TextRead() {
        string[] lines = textFile_Skill.text.Split('\n').Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();

        for (int i = 0; i < lines.Length; i += 1) {
            string title = lines[i].Split('-')[0].Trim();
            string mainText = lines[i].Split('-')[1].Trim();
            SkillInfo newSkillInfo = new SkillInfo(title, mainText);
            skilltexts.Add(newSkillInfo);
            shelterTooltip.Add(i, skilltexts[i]);
        }
    }


    // ���� ��ư ���콺 ��ġ Ȯ��
    public void OnPointerEnter(PointerEventData eventData) {
        if (eventData.pointerEnter != null) {
            Button btn = eventData.pointerEnter.GetComponent<Button>();
            if (btn != null) ShelterTooltipShow(btn.gameObject);
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (eventData.pointerCurrentRaycast.gameObject != null) {
            Button btn = eventData.pointerCurrentRaycast.gameObject.GetComponent<Button>();
            if (btn != null) dictionaryKey = 0;
        }

    }

    // dictionary�� key �� Ȯ��
    // ã���� ������Ʈ�� �̸��� �޾Ƽ� Ư�� ���ԵǾ������� �� ���ؼ� ���� 
    private int FindDictionaryKey(GameObject btn) {
        int DictionKey = 0;
        char lastChar = btn.name[btn.name.Length - 1];
        int skillnum = 0;

        if (btn.name.Contains("Move")) DictionKey = 0;
        else if (btn.name.Contains("Attack")) DictionKey = 5;
        else if (btn.name.Contains("Gather")) DictionKey = 10;

        // Function �߰�
        else if (btn.name.Contains("Sleep")) DictionKey = 60;
        else if (btn.name.Contains("Upgrade")) DictionKey = 70;
        else if (btn.name.Contains("Packing")) DictionKey = 80;
        else return -1;
        if (dictionaryKey <= 50) {
            if (char.IsDigit(lastChar)) skillnum = lastChar - '1';
            else Debug.Log("Last character is not a number: " + lastChar);

            DictionKey += skillnum;
            return DictionKey;
        }
        else return DictionKey;
    }

    // dictionary�� key ������ tooltip�� �ֱ�
    //TODO: ���� ���缭 �ϴ� �߰������� ǥ��
    public void ShelterTooltipShow(GameObject btn) {
        dictionaryKey = FindDictionaryKey(btn.gameObject);
        Debug.Log("dictionaryKey" + dictionaryKey);

        if (dictionaryKey == -1) return;

        // skill �����
        if (dictionaryKey <= 50) {
            // �̹��� ����
            LoadTextPositions();
            tooltipImg.gameObject.SetActive(true);
            tooltipImg.sprite = btn.gameObject.transform.GetChild(1).GetComponent<Image>().sprite;
            tooltipTitle.text = shelterTooltip[dictionaryKey].Title;
            tooltipMain.text = shelterTooltip[dictionaryKey].MainText;
            AdditionalText(dictionaryKey);
        }
        //TODO: Function �߰� ����
        else {

            LoadTextPositions_Func();
            FunctionTooltip(btn, dictionaryKey);
        }
    }

    /* �߰� ������
     * �� ��� : ��ư�� dictionaryKey vs ���� ���� ����
     * ����Ʈ �������� �� : �ش� ��ư�� Ű�� ������ �ش� ����Ʈ ������ 
    1. ������ ���� ��� : �ּ� ���� -> ���� �۾�
    2. ������ ������ ����Ʈ�� ���� ��� : �ʿ� ����Ʈ -> ���� �۾�
    3. ���� ������ ����Ʈ ���� : �ʿ� ����Ʈ -> �Ͼ� �۾�
    */

    private void AdditionalText(int dictionaryKey) {
        if ((dictionaryKey % 5 + 1) > shelterManager.ShelterLevel)
            tooltipAdditionalText.text = string.Format("<color=red>{0} : {1}</color>", additionalText[1], dictionaryKey % 5 + 1);
        else {
            string textColor = "white";
            if (0 <= dictionaryKey && dictionaryKey < 5) { textColor = shelterManager.MovePoint > 0 ? "white" : "red"; }
            else if (5 <= dictionaryKey && dictionaryKey < 10) { textColor = shelterManager.AttackPoint > 0 ? "white" : "red"; }
            else if (10 <= dictionaryKey && dictionaryKey < 15) { textColor = shelterManager.GatherPoint > 0 ? "white" : "red"; }

            tooltipAdditionalText.text = string.Format("<color={1}>{0}</color>", additionalText[0], textColor);
        }
    }



    // ============ ShelterFunction Test ============

    // Function �� �ؽ�Ʈ ���� �б� �߰�
    private void TextReadFunc() {
        string[] lines = textFile_Func.text.Split('\n').Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
        List<ShelterFunctionInfo> sleepFunc = new List<ShelterFunctionInfo>();
        List<ShelterFunctionInfo> shelterFunc = new List<ShelterFunctionInfo>();
        List<ShelterFunctionInfo> packingFunc = new List<ShelterFunctionInfo>();

        for (int i = 0; i < lines.Length; i++) {

            // Tooltip ���� - ������
            string[] parts = lines[i].Split('/');
            string[] titleAndText = parts[0].Trim().Split('-');

            if (lines[i].Contains("Sleep")) {
                // 1��° - ���� / ����
                string title = titleAndText[1].Trim();
                string mainText = titleAndText[2].Trim();

                // 2��° - ������
                string[] resources = parts[1].Trim().Split('-');
                int woodneed = int.Parse(resources[0].Trim());
                int stoneneed = int.Parse(resources[1].Trim());
                int[] itemneed = { woodneed, stoneneed };

                // 1. �ڱ�
                ShelterFunctionInfo newFunc = new ShelterFunctionInfo(title, mainText, itemneed, sleepTime);
                sleepFunc.Add(newFunc);
                shelterFuncTooltip[0].Add(sleepFunc.Count - 1, newFunc);
            }
            else if (lines[i].Contains("Shelter")) {
                // 1��° - ���� / ����
                string title = titleAndText[1].Trim();
                string mainText = titleAndText[2].Trim();

                // 2��° - ������
                string[] resources = parts[1].Trim().Split('-');
                int woodneed = int.Parse(resources[0].Trim());
                int stoneneed = int.Parse(resources[1].Trim());
                int leatherneed = int.Parse(resources[2].Trim());
                int steelneed = int.Parse(resources[3].Trim());
                int[] itemneed = { woodneed, stoneneed, leatherneed, steelneed };

                // 2. ��ó ������Ʈ
                ShelterFunctionInfo newFunc = new ShelterFunctionInfo(title, mainText, itemimgs, itemneed);
                shelterFunc.Add(newFunc);
                shelterFuncTooltip[1].Add(shelterFunc.Count - 1, newFunc);
            }
            else if (lines[i].Contains("Packing")) {
                // 1��° - ���� / ����
                string title = titleAndText[1].Trim();
                string mainText = titleAndText[2].Trim();

                // 3. �� �α�
                ShelterFunctionInfo newFunc = new ShelterFunctionInfo(title, mainText);
                packingFunc.Add(newFunc);
                shelterFuncTooltip[2].Add(packingFunc.Count - 1, newFunc);
            }
        }
    }
    private void FunctionTooltip(GameObject btn, int dictionaryKey) {
        /*
         * 0. ��� ���� ����
         1. ���� ��ų������ �̹��� ��Ȱ��ȭ 
         2. dictionaryKey ����
         3. ���� Text ��ġ ����
         */

        tooltipImg.gameObject.SetActive(false);
        sleepTime.gameObject.SetActive(false);
        itemimgs.gameObject.SetActive(false);
        itemtexts.gameObject.SetActive(false);

        // ��� ���� ����
        RectTransform tooltipBg = ShelterTooltip.transform.GetChild(0).GetComponent<RectTransform>();
        Vector2 BgnewPosition = tooltipBg.anchoredPosition;
        Vector2 BgSize = tooltipBg.sizeDelta;
        BgSize.y = 450;
        tooltipBg.sizeDelta = BgSize;
        BgnewPosition.y = 0;
        tooltipBg.anchoredPosition = BgnewPosition;

        // ���� Text ��ġ ����
        RectTransform tooltipMainRe = tooltipMain.GetComponent<RectTransform>();
        Vector2 MainnewPosition = tooltipMainRe.anchoredPosition;
        MainnewPosition.x = 0;
        tooltipMainRe.anchoredPosition = MainnewPosition;
        Vector2 MainnewSize = tooltipMainRe.sizeDelta;
        MainnewSize.x = 350;
        tooltipMainRe.sizeDelta = MainnewSize;

        // �߰� ���� Text ��ġ ����
        RectTransform tooltipAddRe = tooltipAdditionalText.GetComponent<RectTransform>();
        Vector2 AddnewPosition = tooltipAddRe.anchoredPosition;
        AddnewPosition.y = 150;
        tooltipAddRe.anchoredPosition = AddnewPosition;

        switch (dictionaryKey) {
            case 60:
                // 1. �ڱ�
                tooltipTitle.text = shelterFuncTooltip[0][0].Title;
                tooltipMain.text = shelterFuncTooltip[0][0].MainText;
                tooltipAdditionalText.text = "";
                // �������� Ȱ��ȭ
                sleepTime.gameObject.SetActive(true);
                break;

            case 70:
                // 2. ���׷��̵�
                tooltipTitle.text = shelterFuncTooltip[1][shelterManager.ShelterLevel].Title;
                tooltipMain.text = shelterFuncTooltip[1][shelterManager.ShelterLevel].MainText;
                tooltipAdditionalText.text = "�ʿ� ����ǰ";

                //TODO: ������ Inven���� �޾ƿ;���
                // ������ �̹��� Ȱ��ȭ
                UpgradeFunc_ItemTextInit();
                // ������ �ؽ�Ʈ ����
                UpgradeFunc_ItemTextPosition(UpgradeFunc_ItemText());
                break;

            case 80:
                // 3. �� �α�
                tooltipTitle.text = shelterFuncTooltip[2][0].Title;
                tooltipMain.text = shelterFuncTooltip[2][0].MainText;
                tooltipAdditionalText.text = "";
                break;
            default:
                Debug.Log("FunctionTooltip ������");
                break;
        }

    }
    private void UpgradeFunc_ItemTextInit() {
        itemimgs.gameObject.SetActive(true);
        foreach (Transform item in itemimgs.transform) {
            item.gameObject.SetActive(true);
        }
        itemtexts.gameObject.SetActive(true);
        foreach (Transform item in itemtexts.transform) {
            item.gameObject.SetActive(true);
        }
    }

    private List<GameObject>[] UpgradeFunc_ItemText() {
        List<GameObject>[] needItems = new List<GameObject>[2];
        needItems[0] = new List<GameObject>();
        needItems[1] = new List<GameObject>();

        for (int i = 0; i < itemtexts.transform.childCount; i++) {
            int needItem = shelterFuncTooltip[1][shelterManager.ShelterLevel].ItemNeedNum[i];
            if (needItem == 0) {
                itemtexts.transform.GetChild(i).gameObject.SetActive(false);
                itemimgs.transform.GetChild(i).gameObject.SetActive(false);
                continue;
            }
            else {
                int currentItem = ItemNumCheck_Func(itemtexts.transform.GetChild(i).name);
                Text text = itemtexts.transform.GetChild(i).GetComponent<Text>();
                string textColor = "white";
                textColor = currentItem >= needItem ? "white" : "red";
                text.text = string.Format("<color={0}>{1} / {2}</color>", textColor, currentItem, needItem);
                needItems[0].Add(text.gameObject);
                needItems[1].Add(itemimgs.transform.GetChild(i).gameObject);
            }
        }
        return needItems;
    }
    private void UpgradeFunc_ItemTextPosition(List<GameObject>[] needItems) {
        if (needItems == null) return;

        for (int i = 0; i < needItems.Length; i++) {
            for (int j = 0; j < needItems[i].Count; j++) {
                RectTransform eachRe = needItems[i][j].GetComponent<RectTransform>();
                eachRe.anchoredPosition = new Vector2(eachRe.anchoredPosition.x + 40 * (4 - needItems[i].Count), eachRe.anchoredPosition.y);
            }
        }

    }

    // InvenController ��ũ��Ʈ�� ��ü �κ��丮�� �������� �̸� Ȯ���ؼ� ���� Ȯ��
    private int ItemNumCheck_Func(string itemname) {
        InvenController inven = FindObjectOfType<InvenController>();
        int itemTotalNum = 0;
        if (inven.Inventory != null) {
            foreach (ItemData each in inven.Inventory) {
                if (each.ItemName == itemname) {
                    if (each is CountableItemData countItem) itemTotalNum += countItem.CurrStackCount;
                }
            }
            return itemTotalNum;
        }
        return -1;
    }

    public void SleepFuncOnClick() {
        StopCoroutine("SleepSliderValue");
        Text text = sleepTime.GetComponentInChildren<Text>();
        //Todo: �ڸ�ƾ �����̴� �� ����
        StartCoroutine(SleepSliderValue(text, sleepTime));
    }

    private IEnumerator SleepSliderValue(Text text, Slider sleepTime) {
        // �ð�...
        string[] timeParts = text.text.Split(':');
        int minutes = int.Parse(timeParts[0]);
        int seconds = int.Parse(timeParts[1]);
        float totalSeconds = minutes * 60 + seconds;
        float decrement = 1 / totalSeconds;

        while (totalSeconds > 0) {
            yield return new WaitForSeconds(1f);
            totalSeconds--;
            minutes = (int)(totalSeconds / 60);
            seconds = (int)(totalSeconds % 60);
            sleepTime.value -= decrement;
            text.text = string.Format("{0}:{1:D2}", minutes, seconds);
        }
        sleepTime.value = 0;
    }

}

// ============ ShelterFunction Test ============

public class ShelterFunctionInfo {
    /* shelter ���
     * 1. �ڱ� 2. ���׷��̵� 3. ���α� 
     - ���� text�� ��� ������ �ٸ� : List �迭�� ���� ��� ��
     */
    public string Title;
    public string MainText;
    public GameObject ItemNeeds;
    public int[] ItemNeedNum;
    public Slider sleepTime;
    //public Button[] sleepOrAwake;

    // 1. �ڱ�
    public ShelterFunctionInfo(string Title, string MainText, int[] ItemNeedNum, Slider sleepTime) {
        this.Title = Title;
        this.MainText = MainText;
        this.ItemNeedNum = ItemNeedNum;
        this.sleepTime = sleepTime;
    }

    // 2. ���׷��̵�
    public ShelterFunctionInfo(string Title, string MainText, GameObject ItemNeeds, int[] ItemNeedNum) {
        this.Title = Title;
        this.MainText = MainText;
        this.ItemNeeds = ItemNeeds;
        this.ItemNeedNum = ItemNeedNum;
    }

    // 3. ���α� 
    public ShelterFunctionInfo(string Title, string MainText) {
        this.Title = Title;
        this.MainText = MainText;
    }

}


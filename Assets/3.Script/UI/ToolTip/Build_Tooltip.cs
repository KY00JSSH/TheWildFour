using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class MenuBuildInfo {
    public string Title;
    public string MainText;
    public GameObject ItemNeeds;
    public int[] ItemNeedNum;

    public MenuBuildInfo(string Title, string MainText, GameObject ItemNeeds, int[] ItemNeedNum) {
        this.Title = Title;
        this.MainText = MainText;
        this.ItemNeeds = ItemNeeds;
        this.ItemNeedNum = ItemNeedNum;
    }

}

public class Build_Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    /*
     *  Build Tooltip
     1. Text�� ���� �޾ƿ���
     2. �ʿ��� �������̹���
     3. ���� ������ ���� / �ʿ��� ������ ���� 

    ==========
    1. ���� ��ü�� ��ũ��Ʈ ����
    2. ���� ��ư�� ������ �迭�� ����
    3. Ŭ���̺�Ʈ�� ��ư Ȯ�� �� ���� ��ư���� Ȯ��
    4. ������ �ε��� Ȯ�� dictionaryKey ���� 
    5. dictionaryKey ���

    ========== 
    ���� ������ ���� 
    - �κ� ����Ʈ ���鼭 ���� Ȯ�� �� bool�� ����

     */


    [SerializeField] private Menu_Controll menuControll;
    [SerializeField] private Button[] buttons;

    public GameObject tooltipbox;
    [SerializeField] private Text tooltipTitle;   // ������ �̸� �ؽ�Ʈ
    [SerializeField] private Text tooltipMain; // ������ ���� �ؽ�Ʈ


    [SerializeField] private GameObject itemimgs;
    [SerializeField] private GameObject itemtexts;


    [Space((int)2)]
    [Header("BuildTooltip")]
    Dictionary<int, MenuBuildInfo> BuildTooltip = new Dictionary<int, MenuBuildInfo>();
    [SerializeField] private int dictionaryKey;

    [SerializeField] private List<MenuBuildInfo> buildtexts = new List<MenuBuildInfo>();
    public TextAsset textFile;


    //TODO: ������ üũ�� �ð� ������ ���� ��ũ��Ʈ �̵� ����
    public bool isStartBuildingNumCheck = false;

    private void Awake() {
        TextRead();
        if (buttons == null) buttons = transform.GetComponentsInChildren<Button>();
        menuControll = FindObjectOfType<Menu_Controll>();

        if (tooltipTitle == null) tooltipTitle = tooltipbox.transform.GetChild(1).GetComponent<Text>();
        if (tooltipMain == null) tooltipMain = tooltipbox.transform.GetChild(0).GetChild(0).GetComponent<Text>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (eventData.pointerEnter != null) {
            Button btn = eventData.pointerEnter.GetComponent<Button>();
            if (btn != null) {
                Debug.Log(btn.gameObject.name + " - Mouse enter");

                BuildTooltipShow(btn);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (eventData.pointerCurrentRaycast.gameObject != null) {
            Button btn = eventData.pointerCurrentRaycast.gameObject.GetComponent<Button>();
            if (btn != null) {
                Debug.Log(btn.gameObject.name + " - Mouse exit");
                dictionaryKey = 0;
            }
        }
    }
    private int FindDictionaryKey(Button btn) {
        int DictionKey = 0;
        for (int i = 0; i < buttons.Length; i++) {
            if (buttons[i].name == btn.name) {
                DictionKey = i;
                break;
            }
        }
        return DictionKey;
    }

    private void BuildTooltipShow(Button btn) {
        menuControll.ButtonMove(400, false);
        tooltipbox.gameObject.SetActive(true);

        dictionaryKey = FindDictionaryKey(btn);
        tooltipTitle.text = BuildTooltip[dictionaryKey].Title;
        tooltipMain.text = BuildTooltip[dictionaryKey].MainText;

        UpgradeFunc_ItemTextInit();
        UpgradeFunc_ItemText();

    }

    // �ؽ�Ʈ �ʱ�ȭ
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
        int buildingCheckCount = 0;
        for (int i = 0; i < itemtexts.transform.childCount; i++) {
            int needItem = BuildTooltip[dictionaryKey].ItemNeedNum[i];
            if (needItem == 0) {
                isStartBuildingNumCheck = true;
                itemtexts.transform.GetChild(i).gameObject.SetActive(false);
                itemimgs.transform.GetChild(i).gameObject.SetActive(false);
                buildingCheckCount++;
                continue;
            }
            else {
                int currentItem = ItemNumCheck_Func(itemtexts.transform.GetChild(i).name);
                Text text = itemtexts.transform.GetChild(i).GetComponent<Text>();
                string textColor = "white";
                textColor = currentItem >= needItem ? "white" : "red";
                buildingCheckCount = currentItem >= needItem ? buildingCheckCount++ : 0;
                text.text = string.Format("<color={0}>{1} / {2}</color>", textColor, currentItem, needItem);
                needItems[0].Add(text.gameObject);
                needItems[1].Add(itemimgs.transform.GetChild(i).gameObject);
            }
        }

        // 24 07 16 ����� �Ǽ� ��ġ bool�߰� -> �κ� ������ ���� Ȯ��
        if (buildingCheckCount == itemtexts.transform.childCount)
            isStartBuildingNumCheck = true;
        else isStartBuildingNumCheck = false;

        return needItems;
    }

    // InvenController ��ũ��Ʈ�� ��ü �κ��丮�� �������� �̸� Ȯ���ؼ� ���� Ȯ��
    private int ItemNumCheck_Func(string itemname) {
        InvenController inven = FindObjectOfType<InvenController>();
        int itemTotalNum = 0;
        if (inven.Inventory != null) {
            foreach (Item each in inven.Inventory) {
                if (each.name == itemname) {
                    if (each is CountableItem countItem) itemTotalNum += countItem.CurrStackCount;
                }
            }
            return itemTotalNum;
        }
        return -1;
    }


    private void TextRead() {
        string[] lines = textFile.text.Split('\n').Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();

        for (int i = 0; i < lines.Length; i += 1) {
            string title = lines[i].Split('-')[0].Trim();
            string mainText = lines[i].Split('-')[1].Trim();
            string wood = lines[i].Split('-')[2].Trim();
            int woodneed = int.Parse(wood);
            string stone = lines[i].Split('-')[3].Trim();
            int stoneneed = int.Parse(stone);
            int[] itemneed = { woodneed, stoneneed };

            MenuBuildInfo newbuildInfo = new MenuBuildInfo(title, mainText, itemimgs, itemneed);
            buildtexts.Add(newbuildInfo);
            BuildTooltip.Add(i, buildtexts[i]);
        }

    }
}

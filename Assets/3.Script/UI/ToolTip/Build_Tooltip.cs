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
     1. Text로 정보 받아오기
     2. 필요한 아이템이미지
     3. 현재 아이템 개수 / 필요한 아이템 개수 

    ==========
    1. 상위 객체에 스크립트 붙임
    2. 하위 버튼의 정보를 배열로 담음
    3. 클릭이벤트로 버튼 확인 후 하위 버튼인지 확인
    4. 있으면 인덱스 확인 dictionaryKey 저장 
    5. dictionaryKey 출력

    ========== 
    현재 아이템 개수 
    - 인벤 리스트 돌면서 개수 확인 후 bool값 리턴

     */


    [SerializeField] private Menu_Controll menuControll;
    [SerializeField] private Button[] buttons;

    public GameObject tooltipbox;
    [SerializeField] private Text tooltipTitle;   // 아이템 이름 텍스트
    [SerializeField] private Text tooltipMain; // 아이템 설명 텍스트


    [SerializeField] private GameObject itemimgs;
    [SerializeField] private GameObject itemtexts;


    [Space((int)2)]
    [Header("BuildTooltip")]
    Dictionary<int, MenuBuildInfo> BuildTooltip = new Dictionary<int, MenuBuildInfo>();
    [SerializeField] private int dictionaryKey;

    [SerializeField] private List<MenuBuildInfo> buildtexts = new List<MenuBuildInfo>();
    public TextAsset textFile;


    //TODO: 아이템 체크는 시간 남으면 별도 스크립트 이동 예정
    public bool isBuildAvailable = false;

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
                BuildTooltipShow(btn);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (eventData.pointerCurrentRaycast.gameObject != null) {
            Button btn = eventData.pointerCurrentRaycast.gameObject.GetComponent<Button>();
            if (btn != null) {
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

    // 텍스트 초기화
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
                isBuildAvailable = true;
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
        Debug.Log("아이템 인벤토리 개수 확인" + buildingCheckCount + " / " + itemtexts.transform.childCount);
        // 24 07 16 김수주 건설 설치 bool추가 -> 인벤 아이템 개수 확인
        if (buildingCheckCount == itemtexts.transform.childCount)
            isBuildAvailable = true;
        else isBuildAvailable = false;

        Debug.Log("아이템 인벤토리 bool 값 확인" + isBuildAvailable);
        return needItems;
    }

    // InvenController 스크립트의 전체 인벤토리에 아이템이 이름 확인해서 갯수 확인
    //TODO: 아이템 검출 이상함! => 이름 말고 샤이니 KEY 를 찾아야함
    private int ItemNumCheck_Func(string itemname) {
        InvenController inven = FindObjectOfType<InvenController>();
        int itemTotalNum = 0;
        if (inven.Inventory != null) {


            for (int i = 0; i < inven.Inventory.Count; i++) {
                if (inven.Inventory[i] is CountableItem countItem) {
                    if (countItem.itemData.ItemName == itemname) {
                        itemTotalNum += countItem.CurrStackCount;
                    }
                    else {
                        Debug.Log("아이템의 이름이 같지 않음" + inven.Inventory[i].itemData.ItemName);
                    }
                }
                else {
                    Debug.Log("아이템의 종류가 같지 않음" + inven.Inventory[i].itemData.Type);
                }
            }
            /*
            foreach (Item each in inven.Inventory) {
                if (each.name == itemname) {
                    if (each is CountableItem countItem) itemTotalNum += countItem.CurrStackCount;
                }
                else Debug.Log("아이템의 이름이 같지 않음" + each.name);
            }
            */
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

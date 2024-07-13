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
    public int woodNeed;
    public int stoneNeed;

    public MenuBuildInfo(string Title, string MainText, int woodNeed, int stoneNeed) {
        this.Title = Title;
        this.MainText = MainText;
        this.woodNeed = woodNeed;
        this.stoneNeed = stoneNeed;
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


     */


    [SerializeField] private Menu_Controll menuControll;

    [SerializeField] private Button[] buttons;

    public GameObject tooltipbox;
    [SerializeField] private Text tooltipTitle;   // 아이템 이름 텍스트
    [SerializeField] private Text tooltipMain; // 아이템 설명 텍스트
    [SerializeField] private Text woodhave;
    [SerializeField] private Text stonehave;

    //[SerializeField] private Image[] itemNeed; // 아이템 설명 텍스트


    [Space((int)2)]
    [Header("BuildTooltip")]
    Dictionary<int, MenuBuildInfo> BuildTooltip = new Dictionary<int, MenuBuildInfo>();
    [SerializeField] private int dictionaryKey;

    [SerializeField] private List<MenuBuildInfo> buildtexts = new List<MenuBuildInfo>();
    public TextAsset textFile;


    private void Awake() {
        TextRead();
        if (buttons == null) buttons = transform.GetComponentsInChildren<Button>();
        menuControll = FindObjectOfType<Menu_Controll>();

        if(tooltipTitle ==null) tooltipTitle = tooltipbox.transform.GetChild(1).GetComponent<Text>();
        if(tooltipMain == null) tooltipMain = tooltipbox.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        if(woodhave == null) woodhave = tooltipbox.transform.GetChild(0).GetChild(2).GetComponentInChildren<Text>();
        if(stonehave == null) stonehave = tooltipbox.transform.GetChild(0).GetChild(3).GetComponentInChildren<Text>();

        Debug.Log("Build_Tooltip 스크립트 buttons 갯수 확인 " + buttons.Length);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        Debug.Log("OnPointerEnter called");
        if (eventData.pointerEnter != null) {
            Button btn = eventData.pointerEnter.GetComponent<Button>();
            if (btn != null) {
                Debug.Log(btn.gameObject.name + " - Mouse enter");

                BuildTooltipShow(btn);

                Debug.Log("dictionaryKey 확인" + dictionaryKey); ;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        Debug.Log("OnPointerExit called");
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

        //TODO: 현재 아이템 중 나무 돌 확인하여 들고 와야함 + 아이템 갯수확인하여 글자 색 변경
        //TODO: 

        Image wood = tooltipbox.transform.GetChild(0).GetChild(2).GetComponent<Image>();
        Image stone = tooltipbox.transform.GetChild(0).GetChild(3).GetComponent<Image>();
        if (BuildTooltip[dictionaryKey].woodNeed == 0) {
            woodhave.gameObject.SetActive(false);
            wood.gameObject.SetActive(false);
        }
        else {
            woodhave.gameObject.SetActive(true);
            wood.gameObject.SetActive(true);
            woodhave.text = string.Format("{0} / {0}", BuildTooltip[dictionaryKey].woodNeed);
        }
        if (BuildTooltip[dictionaryKey].stoneNeed == 0) {
            stonehave.gameObject.SetActive(false);
            stone.gameObject.SetActive(false);
        }
        else {
            stonehave.gameObject.SetActive(true);
            stone.gameObject.SetActive(true);
            stonehave.text = string.Format("{0} / {0}", BuildTooltip[dictionaryKey].stoneNeed);
        }
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
            MenuBuildInfo newbuildInfo = new MenuBuildInfo(title, mainText, woodneed, stoneneed);
            buildtexts.Add(newbuildInfo);
            BuildTooltip.Add(i, buildtexts[i]);
        }

    }
}

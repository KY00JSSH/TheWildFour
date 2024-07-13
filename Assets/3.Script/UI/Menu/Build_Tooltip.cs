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


     */


    [SerializeField] private Menu_Controll menuControll;

    [SerializeField] private Button[] buttons;

    public GameObject tooltipbox;
    [SerializeField] private Text tooltipTitle;   // ������ �̸� �ؽ�Ʈ
    [SerializeField] private Text tooltipMain; // ������ ���� �ؽ�Ʈ
    [SerializeField] private Text woodhave;
    [SerializeField] private Text stonehave;

    //[SerializeField] private Image[] itemNeed; // ������ ���� �ؽ�Ʈ


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

        Debug.Log("Build_Tooltip ��ũ��Ʈ buttons ���� Ȯ�� " + buttons.Length);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        Debug.Log("OnPointerEnter called");
        if (eventData.pointerEnter != null) {
            Button btn = eventData.pointerEnter.GetComponent<Button>();
            if (btn != null) {
                Debug.Log(btn.gameObject.name + " - Mouse enter");

                BuildTooltipShow(btn);

                Debug.Log("dictionaryKey Ȯ��" + dictionaryKey); ;
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

        //TODO: ���� ������ �� ���� �� Ȯ���Ͽ� ��� �;��� + ������ ����Ȯ���Ͽ� ���� �� ����
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

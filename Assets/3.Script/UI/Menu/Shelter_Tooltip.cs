using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;
public class ButtonTest : MonoBehaviour, IPointerClickHandler {
    public void OnPointerClick(PointerEventData eventData) {
        Debug.Log("Button Clicked!");
    }
}

public class SkillInfo {
    public string Title;
    public string MainText;

    public SkillInfo(string Title, string MainText) {
        this.Title = Title;
        this.MainText = MainText;
    }
}


public class Shelter_Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    Dictionary<int, SkillInfo> shelterTooltip = new Dictionary<int, SkillInfo>();
    public int dictionaryKey;

    [SerializeField] private GameObject ShelterTooltip;
    private Image tooltipImg;
    private Text tooltipTitle;
    private Text tooltipMain;

    public TextAsset textFile;
    [SerializeField] private List<SkillInfo> skilltexts = new List<SkillInfo>();

    private void Awake() {
        TextRead();
        tooltipImg = ShelterTooltip.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        tooltipTitle = ShelterTooltip.transform.GetChild(1).GetComponent<Text>();
        tooltipMain = ShelterTooltip.transform.GetChild(0).GetChild(1).GetComponent<Text>();
    }



    // �����  dictionary�� ����
    private void TextRead() {
        string[] lines = textFile.text.Split('\n').Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();

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
        Debug.Log("OnPointerEnter called");
        if (eventData.pointerEnter != null) {
            Button btn = eventData.pointerEnter.GetComponent<Button>();
            if (btn != null) {
                Debug.Log(btn.gameObject.name + " - Mouse enter");
                ShelterTooltipShow(btn.gameObject);

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

    // dictionary�� key �� Ȯ��
    // ã���� ������Ʈ�� �̸��� �޾Ƽ� Ư�� ���ԵǾ������� �� ���ؼ� ���� 
    private int FindDictionaryKey(GameObject btn) {
        int DictionKey = 0;
        char lastChar = btn.name[btn.name.Length - 1];
        int skillnum = 0;

        if (btn.name.Contains("Move")) DictionKey = 0;
        else if (btn.name.Contains("Attack")) DictionKey = 5;
        else if (btn.name.Contains("Gather")) DictionKey = 10;

        if (char.IsDigit(lastChar)) skillnum = lastChar - '1';
        else Debug.Log("Last character is not a number: " + lastChar);

        DictionKey += skillnum;
        return DictionKey;
    }

    // dictionary�� key ������ tooltip�� �ֱ�
    public void ShelterTooltipShow(GameObject btn) {

        // �̹��� ����
        tooltipImg.sprite = btn.gameObject.transform.GetChild(1).GetComponent<Image>().sprite;

        dictionaryKey = FindDictionaryKey(btn.gameObject);
        tooltipTitle.text = shelterTooltip[dictionaryKey].Title;
        tooltipMain.text = shelterTooltip[dictionaryKey].MainText;
    }


}

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
    Dictionary<int, SkillInfo> shelterTooltip = new Dictionary<int, SkillInfo>();
    public int dictionaryKey;

    [SerializeField] private ShelterManager shelterManager;
    [SerializeField] private GameObject ShelterTooltip;
    private Image tooltipImg;
    private Text tooltipTitle;
    private Text tooltipMain;
    private Text tooltipAdditionalText;
    private string[] additionalText = { "�ʿ� ����Ʈ 1", "���� �ʿ� ����" };

    public TextAsset textFile;
    [SerializeField] private List<SkillInfo> skilltexts = new List<SkillInfo>();

    private void Awake() {
        TextRead();
        tooltipImg = ShelterTooltip.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        tooltipTitle = ShelterTooltip.transform.GetChild(1).GetComponent<Text>();
        tooltipMain = ShelterTooltip.transform.GetChild(0).GetChild(1).GetComponent<Text>();
        tooltipAdditionalText = ShelterTooltip.transform.GetChild(0).GetChild(2).GetComponent<Text>();
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
    //TODO: ���� ���缭 �ϴ� �߰������� ǥ��
    public void ShelterTooltipShow(GameObject btn) {

        // �̹��� ����
        tooltipImg.sprite = btn.gameObject.transform.GetChild(1).GetComponent<Image>().sprite;

        dictionaryKey = FindDictionaryKey(btn.gameObject);
        tooltipTitle.text = shelterTooltip[dictionaryKey].Title;
        tooltipMain.text = shelterTooltip[dictionaryKey].MainText;
        AdditionalText(dictionaryKey);
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

}

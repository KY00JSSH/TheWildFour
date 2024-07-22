using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipInfo : MonoBehaviour {
    public GameObject Tooltip_S;

    // Tooltip_S의 구성요소
    protected Text S_TextTitle;
    protected Text S_TextMain;
    protected Text S_TextAdd;
    protected Text S_TextResult;

    protected GameObject S_ItemTexts;
    protected GameObject S_ItemImgs;

    protected void S_Tooltip_Init() {
        S_TextTitle = Tooltip_S.transform.GetChild(1).GetChild(0).GetComponent<Text>();
        S_TextMain = Tooltip_S.transform.GetChild(1).GetChild(1).GetComponent<Text>();
        S_TextAdd = Tooltip_S.transform.GetChild(1).GetChild(2).GetComponent<Text>();
        S_TextResult = Tooltip_S.transform.GetChild(1).GetChild(3).GetComponent<Text>();


        S_ItemTexts = Tooltip_S.transform.GetChild(2).GetChild(0).gameObject;
        S_ItemImgs = Tooltip_S.transform.GetChild(2).GetChild(1).gameObject;
    }

    public GameObject Tooltip_L;

    // Tooltip_L의 구성요소
    protected Text L_TextTitle;
    protected Text L_TextMain;
    protected Text L_TextAdd;
    protected Text L_TextResult;
    protected Image L_ItemImg;

    protected GameObject L_ItemTexts;
    protected GameObject L_ItemImgs;

    protected GameObject L_StatsTexts;
    protected GameObject L_StatsImgs;
    protected void L_Tooltip_Init() {
        L_TextTitle = Tooltip_L.transform.GetChild(1).GetChild(0).GetComponent<Text>();
        L_TextMain = Tooltip_L.transform.GetChild(1).GetChild(1).GetComponent<Text>();
        L_TextAdd = Tooltip_L.transform.GetChild(1).GetChild(2).GetComponent<Text>();
        L_TextResult = Tooltip_L.transform.GetChild(1).GetChild(3).GetComponent<Text>();
        L_ItemImg = Tooltip_L.transform.GetChild(1).GetChild(4).GetComponent<Image>();

        L_ItemTexts = Tooltip_L.transform.GetChild(2).GetChild(0).gameObject;
        L_ItemImgs = Tooltip_L.transform.GetChild(2).GetChild(1).gameObject;

        L_StatsTexts = Tooltip_L.transform.GetChild(3).GetChild(0).gameObject;
        L_StatsImgs = Tooltip_L.transform.GetChild(3).GetChild(1).gameObject;

    }
    public Text LevelText;

    // 변경될 Stats 스프라이트 
    public Sprite[] sprites;
    protected Dictionary<int, Sprite> L_StatsSprites { get; private set; }

    protected void L_Tooltip_Sprites_Init() {
        L_StatsSprites = new Dictionary<int, Sprite>();  // 딕셔너리 초기화
        foreach (Sprite each in sprites) {
            if (each.name.Contains("Attack")) {
                L_StatsSprites.Add(1, each);
            }
            else if (each.name.Contains("Defence")) {
                L_StatsSprites.Add(2, each);
            }
            else if (each.name.Contains("Heal")) {
                L_StatsSprites.Add(3, each);
            }
            else if (each.name.Contains("Warm")) {
                L_StatsSprites.Add(4, each);
            }
            else if (each.name.Contains("Durability")) {
                L_StatsSprites.Add(5, each);
            }
            else if (each.name.Contains("wood")) {
                L_StatsSprites.Add(6, each);
            }
            else if (each.name.Contains("ston")) {
                L_StatsSprites.Add(7, each);
            }
            else if (each.name.Contains("steel")) {
                L_StatsSprites.Add(8, each);
            }
            else if (each.name.Contains("iron")) {
                L_StatsSprites.Add(9, each);
            }
            else if (each.name.Contains("leather")) {
                L_StatsSprites.Add(10, each);
            }
            else {
                Debug.LogWarning("스프라이트 오류");
            }
        }
    }

    protected virtual void Awake() {
        S_Tooltip_Init();
        L_Tooltip_Init();
        L_Tooltip_Sprites_Init();

        PosInit(S_ItemImgs, ref S_itemNeedPosSave);
        PosInit(L_StatsImgs, ref StatsPosSave);
        PosInit(L_ItemImgs, ref L_itemNeedPosSave);

        SaveTextPositions_Func(S_ItemTexts, S_ItemImgs, S_itemNeedPosSave);
        SaveTextPositions_Func(L_ItemTexts, L_ItemImgs, L_itemNeedPosSave);
        SaveTextPositions_Func(L_StatsTexts, L_StatsImgs, StatsPosSave);

    }

    private void PosInit(GameObject Obj, ref Vector2[][] pos) {
        pos = new Vector2[2][];
        pos[0] = new Vector2[Obj.transform.childCount];
        pos[1] = new Vector2[Obj.transform.childCount];
    }

    protected virtual void OnEnable() {
        Tooltip_S.SetActive(false);
        Tooltip_L.SetActive(false);
    }

    protected virtual void OnDisable() {
        Tooltip_S.SetActive(false);
        Tooltip_L.SetActive(false);
    }

    protected Vector2[][] S_itemNeedPosSave;
    protected Vector2[][] L_itemNeedPosSave;
    protected Vector2[][] StatsPosSave;

    private void SaveTextPositions_Func(GameObject texts, GameObject imgs, Vector2[][] Position) {
        if (texts == null || imgs == null) {

            Debug.LogWarning("Texts or Imgs is null");
            return;
        }
        for (int i = 0; i < texts.transform.childCount; i++) {
            RectTransform eachRe = texts.transform.GetChild(i).GetComponent<RectTransform>();
            Position[0][i] = eachRe.anchoredPosition;
        }
        for (int i = 0; i < imgs.transform.childCount; i++) {
            RectTransform eachRe = imgs.transform.GetChild(i).GetComponent<RectTransform>();
            Position[1][i] = eachRe.anchoredPosition;
        }
    }

    protected void LoadTextPositions_Func(GameObject texts, GameObject imgs, Vector2[][] Position) {
        for (int i = 0; i < texts.transform.childCount; i++) {
            RectTransform eachRe = texts.transform.GetChild(i).GetComponent<RectTransform>();
            eachRe.anchoredPosition = Position[0][i];
        }
        for (int i = 0; i < imgs.transform.childCount; i++) {
            RectTransform eachRe = imgs.transform.GetChild(i).GetComponent<RectTransform>();
            eachRe.anchoredPosition = Position[1][i];
        }
    }

    protected void TextImgActiveInit(GameObject texts, GameObject imgs) {
        foreach (Transform each in texts.transform) {
            each.gameObject.SetActive(true);
        }
        foreach (Transform each in imgs.transform) {
            each.gameObject.SetActive(true);
        }
    }
    protected void WorkshopNeedItemDisappear(int itemNum) {
        for (int i = 0; i < L_StatsTexts.transform.childCount; i++) {
            if (i > itemNum - 1) {
                L_ItemTexts.transform.GetChild(i).gameObject.SetActive(false);
                L_ItemImgs.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }


}

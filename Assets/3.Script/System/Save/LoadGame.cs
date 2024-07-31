using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LoadGame : MonoBehaviour {
    [SerializeField] private GameObject buttonSaveData;
    [SerializeField] private Transform buttonParent;

    [SerializeField] private Sprite[] playerIcons;
    [SerializeField] private Text textNoFile;

    private List<GameObject> buttonList = new List<GameObject>();
    private SaveDataList saveDataList;

    private void OnEnable() {
        saveDataList = Save.Instance.Load();
        if (saveDataList != null) {
            textNoFile.gameObject.SetActive(false);
            int count = 0;
            saveDataList.Data.Reverse();
            foreach (var saveData in saveDataList.Data) {
                GameObject eachSaveData = Instantiate(buttonSaveData, buttonParent);
                eachSaveData.SetActive(true);
                Text[] texts = eachSaveData.GetComponentsInChildren<Text>();
                texts[0].text = saveData.SaveName;
                texts[1].text = saveData.isExtreme ? "어려움" : "보통";
                texts[2].text = saveData.saveTime;
                texts[3].text = saveData.TotalDay.ToString();
                eachSaveData.GetComponentsInChildren<Image>()[2].sprite =
                    playerIcons[(int)saveData.playerType];

                eachSaveData.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 250 - 90 * count);
                eachSaveData.GetComponent<CheckSelectedLoad>().count = count;
                count++;
            }
        }
        else
            textNoFile.gameObject.SetActive(true);
    }

    public void SelectLoad() {
        CheckSelectedLoad[] saveDatas = buttonParent.GetComponentsInChildren<CheckSelectedLoad>();
        foreach(var each in saveDatas) {
            if (each.isSelected) {
                Save.Instance.saveData = saveDataList.Data[each.count];
                break;
            }
        }
    }

    public void ClearSelected() {
        CheckSelectedLoad[] saveDatas = buttonParent.GetComponentsInChildren<CheckSelectedLoad>();
        foreach(var each in saveDatas) {
            each.isSelected = false;
            foreach(var eachText in each.GetComponentsInChildren<Text>()) {
                eachText.color = Color.white;
            }
        }
    }
}
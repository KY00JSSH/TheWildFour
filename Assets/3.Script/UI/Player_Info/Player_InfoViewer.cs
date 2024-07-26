using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player_InfoViewer : MonoBehaviour {
    //UI -> slider
    [SerializeField] private Slider[] player_Infos;
    //slider Text 
    [SerializeField] private Text[] slider_Texts;


    // player의 정보를 ui쪽에서 update 할 경우 각 정보 스크립트 필요함
    //[SerializeField] private PlayerHP player;

    private void Start() {
        List<Slider> slidersList = new List<Slider>();
        List<Text> slidersTextList = new List<Text>();

        foreach (Transform child in transform) {
            Slider slider = child.GetComponent<Slider>();
            if (slider != null) {
                slidersList.Add(slider);
                Text t = child.GetComponentInChildren<Text>();
                if (t != null)
                    slidersTextList.Add(t);
                else
                    Debug.Log("못찾음?");
            }
            else {
                Debug.LogWarning($"슬라이더 없음 {child.name}");
            }
        }
    }

    private void FixedUpdate() {
        playerInfoCheck();
    }

    private void playerInfoCheck() {
        for(int i=0; i < player_Infos.Length; i++) {
            if (player_Infos[i].value <= 25) slider_Texts[i].color = Color.red;
            else slider_Texts[i].color = Color.white;
        }
    }

    // player의 정보를 타 스크립트에서 들고가서 사용할 경우
    public void SetPlayerHp(int value) {
        for (int i = 0; i < player_Infos.Length; i++) {
            if (player_Infos[i].name == "HP") {
                player_Infos[i].value = value;
                slider_Texts[i].text = value.ToString();
            }
        }
    }

    public void SetPlayerHunger(int value) {
        for (int i = 0; i < player_Infos.Length; i++) {
            if (player_Infos[i].name == "Hunger") {
                player_Infos[i].value = value;
                slider_Texts[i].text = value.ToString();
            }
        }
    }

    public void SetPlayerWarm(int value) {
        for (int i = 0; i < player_Infos.Length; i++) {
            if (player_Infos[i].name == "Warm") {
                player_Infos[i].value = value;
                slider_Texts[i].text = value.ToString();
            }
        }
    }
}

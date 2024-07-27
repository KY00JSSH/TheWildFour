using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player_InfoViewer : MonoBehaviour {
    //UI -> slider
    [SerializeField] private Slider[] player_Infos;
    //slider Text 
    [SerializeField] private Text[] slider_Texts;
    // BGImg
    [SerializeField] private Image BGImgChange;

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

    // 플레이어 상태값이 일정 값이하로 떨어졌을 경우 텍스트 이미지 변화
    private void playerInfoCheck() {
        for (int i = 0; i < player_Infos.Length; i++) {
            if (player_Infos[i].value <= 25) {
                slider_Texts[i].color = Color.red;
                playerInfoBGImgChange(i);
            }
            else {
                BGImgChange.gameObject.SetActive(false);
                Color color = BGImgChange.color;
                color = Color.white;
                color.a = 0;
                BGImgChange.color = color;
                slider_Texts[i].color = Color.white;
            }
        }
    }
    // 이미지 
    private void playerInfoBGImgChange(int i) {
        BGImgChange.gameObject.SetActive(true);
        Color color = BGImgChange.color;
        if (i == 0) {
            // i = 0 : 체력일 경우 -> 색상 붉은색 
            color = Color.red;
        }
        else {
            if(color == Color.red) return;
            // i = 1,2 : 허기, 온도일 경우 -> 색상 회색 
            color = Color.gray;
        }
        // 체력 값에 따라 알파값 조절
        //TODO: 알파값 확인해봐야할듯
        float colorValue = (25 - player_Infos[i].value) * 0.03f;
        color.a = Mathf.Min(colorValue, 0.4f);
        BGImgChange.color = color;

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

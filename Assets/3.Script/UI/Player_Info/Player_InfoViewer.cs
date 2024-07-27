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
        if (playerInfoCheck()) {
            BGImgChange.gameObject.SetActive(true);
            if (player_Infos[0].value <= 25) {
                playerInfoBGImgChange(player_Infos[0].value, Color.red);
            }
            else {
                float value = Mathf.Min(player_Infos[1].value, player_Infos[2].value);
                playerInfoBGImgChange(value, Color.gray);
            }
        }
        else {
            playerInfoBGImgInit();
        }
    }

    // 플레이어 상태값이 일정 값이하로 떨어졌을 경우 텍스트 변화
    private bool playerInfoCheck() {
        for (int i = 0; i < player_Infos.Length; i++) {
            if (player_Infos[i].value <= 25) {
                slider_Texts[i].color = Color.red;
                return true;
            }
            else {
                slider_Texts[i].color = Color.white;
            }
        }
        return false;
    }

    // 음영 이미지 초기화
    private void playerInfoBGImgInit() {
        BGImgChange.gameObject.SetActive(false);
        Color color = BGImgChange.color;
        color.a = 0;
        BGImgChange.color = color;
    }

    // 플레이어 기본값이 일정값 이하일경우 배경 이미지 변화
    private void playerInfoBGImgChange(float value, Color colorname) {
        float colorValue = (25 - value) * 0.03f;
        colorname.a = Mathf.Min(colorValue, 0.4f);
        BGImgChange.color = colorname;
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

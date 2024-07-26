using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player_InfoViewer : MonoBehaviour {
    //UI -> slider
    [SerializeField] private Slider[] player_Infos;
    //slider Text 
    [SerializeField] private Text[] slider_Texts;


    // player�� ������ ui�ʿ��� update �� ��� �� ���� ��ũ��Ʈ �ʿ���
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
                    Debug.Log("��ã��?");
            }
            else {
                Debug.LogWarning($"�����̴� ���� {child.name}");
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

    // player�� ������ Ÿ ��ũ��Ʈ���� ����� ����� ���
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

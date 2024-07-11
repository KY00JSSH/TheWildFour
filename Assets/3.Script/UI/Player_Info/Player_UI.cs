using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_UI : MonoBehaviour
{
    /*
     1. 대시 게이지
    2. 스킬 게이지

    =====
    1. 현재 게이지 / 전체 게이지 = 슬라이더 값

     */
    [SerializeField] private Slider playerSlider;
    private PlayerMove playerMove;

    private void Awake() {
        playerMove = GetComponent<PlayerMove>();
    }

    private void Update() {
        if (playerMove.isDash) {
            DashSliderSetting();
        }
        else {
            playerSlider.gameObject.SetActive(false);
        }
    }

    private void DashSliderSetting() {
        playerSlider.gameObject.SetActive(true);
        float sliderValue = playerMove.CurrentDashGage / playerMove.TotalDashGage;
        playerSlider.value = sliderValue;

    }
    


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_UI : MonoBehaviour
{
    /*
     1. ��� ������
    2. ��ų ������

    =====
    1. ���� ������ / ��ü ������ = �����̴� ��

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

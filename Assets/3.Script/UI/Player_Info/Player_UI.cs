using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_UI : MonoBehaviour {
    /*
     1. 대시 게이지
    2. 스킬 게이지

    =====
    1. 현재 게이지 / 전체 게이지 = 슬라이더 값
    2. 현재 게이지 == 전체 게이지 -> 천천히 알파값을 줄임 3초 뒤 
    3. 플레이어 위치 따라 갈 것

     */
    [SerializeField] private Slider playerSlider;
    private PlayerMove playerMove;
    private Coroutine fadeCoroutine;
    private bool isStart = false;
    [SerializeField] private int Slider_AddY = 150;

    private void Awake() {
        playerMove = GetComponent<PlayerMove>();
    }

    private void Update() {
        if (Input.GetKey(KeyCode.LeftShift)) {
            isStart = true;
            if (!playerSlider.gameObject.activeSelf)
                playerSlider.gameObject.SetActive(true);
        }
        else isStart = false;

        if (isStart) SliderAlphaInit();

        DashSliderSetting();

        if (playerSlider.gameObject.activeSelf && !isStart)
            SliderDisappear();
        //if(playerMove.isDash) SliderDisappear();
    }

    private void DashSliderSetting() {
        SettingSliderPosition();
        float sliderValue = playerMove.GetCurrentDashGage() / playerMove.GetTatalDashGage();
        playerSlider.value = sliderValue;
    }
    private void SliderDisappear() {
        if (playerSlider.value == 1) {
            if (fadeCoroutine != null) {
                StopCoroutine(fadeCoroutine);
                fadeCoroutine = null; // 코루틴 추적을 위해 null로 설정
            }
            else
                fadeCoroutine = StartCoroutine(SliderDisappear_co());
        }
    }

    private IEnumerator SliderDisappear_co() {
        Image fillImage = playerSlider.fillRect.GetComponent<Image>();  // Fill 이미지 가져오기
        Color newColor = fillImage.color;

        while (newColor.a > 0.5f) {  // 알파값이 0보다 클 동안 반복
            newColor.a -= 0.01f;  // 알파값을 감소
            fillImage.color = newColor;
            yield return null;
        }
        playerSlider.gameObject.SetActive(false);
    }

    private void SliderAlphaInit() {
        Image fillImage = playerSlider.fillRect.GetComponent<Image>();  // Fill 이미지 가져오기
        Color newColor = fillImage.color;
        newColor.a = 1;
        fillImage.color = newColor;
    }
    private void SettingSliderPosition() {
        RectTransform sliderPosition = playerSlider.GetComponent<RectTransform>();
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.GetChild(0).position);

        screenPosition.y += Slider_AddY;

        sliderPosition.position = screenPosition;
        
    }

}

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


    private Transform playerTransform;
    public Canvas canvas;
    [SerializeField] private int Slider_AddY = 150;
    [SerializeField] private int sliderStabilization = 1000;

    private bool isInitialSetupDone = false;

    private void Awake() {
        playerMove = GetComponent<PlayerMove>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
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

    //TODO: 위치 보정값 이상함
    private void SettingSliderPosition() {

        // 슬라이더 위치 
        RectTransform sliderPosition = playerSlider.GetComponent<RectTransform>();

        // 플레이어 위치 
        Vector3 playerPosition = playerTransform.position;
        Vector3 playerscreenPo = Camera.main.WorldToScreenPoint(playerPosition);

        // 캔버스 RectTransform 가져오기
        RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, playerscreenPo, null, out localPoint);

        if (!isInitialSetupDone || (sliderPosition.anchoredPosition.x - localPoint.x > sliderStabilization) || (sliderPosition.anchoredPosition.y - localPoint.y > sliderStabilization)) {

            // 슬라이더의 위치를 조정하여 플레이어 위치에 맞춤
            sliderPosition.anchoredPosition = localPoint;
            sliderPosition.anchoredPosition += new Vector2(0, Slider_AddY);
            isInitialSetupDone = true;

        }

    }

}

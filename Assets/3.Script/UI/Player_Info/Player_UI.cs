using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_UI : MonoBehaviour {
    /*
     1. ��� ������
    2. ��ų ������

    =====
    1. ���� ������ / ��ü ������ = �����̴� ��
    2. ���� ������ == ��ü ������ -> õõ�� ���İ��� ���� 3�� �� 
    3. �÷��̾� ��ġ ���� �� ��

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
                fadeCoroutine = null; // �ڷ�ƾ ������ ���� null�� ����
            }
            else
                fadeCoroutine = StartCoroutine(SliderDisappear_co());
        }
    }

    private IEnumerator SliderDisappear_co() {
        Image fillImage = playerSlider.fillRect.GetComponent<Image>();  // Fill �̹��� ��������
        Color newColor = fillImage.color;

        while (newColor.a > 0.5f) {  // ���İ��� 0���� Ŭ ���� �ݺ�
            newColor.a -= 0.01f;  // ���İ��� ����
            fillImage.color = newColor;
            yield return null;
        }
        playerSlider.gameObject.SetActive(false);
    }

    private void SliderAlphaInit() {
        Image fillImage = playerSlider.fillRect.GetComponent<Image>();  // Fill �̹��� ��������
        Color newColor = fillImage.color;
        newColor.a = 1;
        fillImage.color = newColor;
    }

    //TODO: ��ġ ������ �̻���
    private void SettingSliderPosition() {

        // �����̴� ��ġ 
        RectTransform sliderPosition = playerSlider.GetComponent<RectTransform>();

        // �÷��̾� ��ġ 
        Vector3 playerPosition = playerTransform.position;
        Vector3 playerscreenPo = Camera.main.WorldToScreenPoint(playerPosition);

        // ĵ���� RectTransform ��������
        RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, playerscreenPo, null, out localPoint);

        if (!isInitialSetupDone || (sliderPosition.anchoredPosition.x - localPoint.x > sliderStabilization) || (sliderPosition.anchoredPosition.y - localPoint.y > sliderStabilization)) {

            // �����̴��� ��ġ�� �����Ͽ� �÷��̾� ��ġ�� ����
            sliderPosition.anchoredPosition = localPoint;
            sliderPosition.anchoredPosition += new Vector2(0, Slider_AddY);
            isInitialSetupDone = true;

        }

    }

}

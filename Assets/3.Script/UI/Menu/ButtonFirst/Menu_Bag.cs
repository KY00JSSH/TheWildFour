using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Bag : MonoBehaviour, IMenuButton {
    [SerializeField] private GameObject TextBox_Bag;
    private Menu_Controll menuControll;


    public Canvas canvas;
    private RectTransform canvasRectTransform;
    private GameObject player;
    private GameObject playerClone;
    [SerializeField] private Vector2 playerPos;
    [SerializeField] private float scaleFactor;
    private void Awake() {
        menuControll = FindObjectOfType<Menu_Controll>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerPos = new Vector2(350, -150);
        scaleFactor = 100f;
        canvasRectTransform = canvas.GetComponent<RectTransform>();
        playerCloneInit();
    }
    private void playerCloneInit() {

        // UI ��ҷ� ǥ���� ��ü�� ����
        playerClone = Instantiate(player, TextBox_Bag.transform);
        playerClone.layer = LayerMask.NameToLayer("UI");
        // Ŭ�е� �÷��̾��� ������ ���� ����
        Rigidbody rb = playerClone.GetComponent<Rigidbody>();
        if (rb != null) {
            rb.useGravity = false;
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.FreezePosition;
        }

        // Ŭ�е� �÷��̾��� RectTransform ����
        RectTransform playerCloneRect = playerClone.AddComponent<RectTransform>();
        playerCloneRect.SetParent(TextBox_Bag.transform, false);
        playerCloneRect.anchoredPosition = playerPos;

        playerCloneRect.localScale = playerCloneRect.localScale * scaleFactor;

        // Ŭ�е� �÷��̾ ��Ȱ��ȭ
        playerClone.SetActive(false);
    }


    private void PlayerRigidbodyOff() {
        // Ŭ�е� �÷��̾��� ������ ���� ����
        Rigidbody rb = playerClone.GetComponent<Rigidbody>();
        if (rb != null) {
            rb.useGravity = false;
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.FreezePosition;
        }
        else {
            Debug.Log("?????????????????????");
        }

    }
    private void OnEnable() {
        PlayerRigidbodyOff();
    }

    // ���� ��ư���� �����
    public void I_ButtonOffClick() {
        TextBoxBagOff();
    }

    public void I_ButtonOnClick() {
        playerClone.SetActive(true);
        TextBoxBagActive();
    }

    public void ButtonOffClick() {
        TextBoxBagOff();
    }

    public void ButtonOnClick() {
        menuControll.CloseUI();
        playerClone.SetActive(true);
        TextBoxBagActive();
    }

    private void TextBoxBagActive() {
        TextBox_Bag.transform.parent.gameObject.SetActive(true);
        TextBox_Bag.SetActive(true);
    }

    private void TextBoxBagOff() {
        TextBox_Bag.transform.parent.gameObject.SetActive(false);
        TextBox_Bag.SetActive(false);
    }

}

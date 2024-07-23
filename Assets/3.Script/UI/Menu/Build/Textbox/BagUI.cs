using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagUI : MonoBehaviour {
    private PlayerAbility playerAbility;

    [SerializeField] private GameObject statusTexts;
    private Text Attack;
    private Text Cold;
    private Text Defence;
    private Text Speed;
    static public bool isBagUIOpen { get { return _isBagUIOpen; } }
    static private bool _isBagUIOpen = false;


    private void Awake() {
        playerAbility = FindObjectOfType<PlayerAbility>();

        Attack = statusTexts.transform.GetChild(0).GetComponent<Text>();
        Cold = statusTexts.transform.GetChild(1).GetComponent<Text>();
        Defence = statusTexts.transform.GetChild(2).GetComponent<Text>();
        Speed = statusTexts.transform.GetChild(3).GetComponent<Text>();


        player = GameObject.FindGameObjectWithTag("Player");
        playerPos = new Vector2(350, -150);
        scaleFactor = 100f;
        canvasRectTransform = canvas.GetComponent<RectTransform>();
        playerCloneInit();
    }

    private void OnEnable() {
        // Ȱ��ȭ �ɶ��� ����ϰ� �� ��
        _isBagUIOpen = true;
        playerClone.SetActive(true);
        playerStatusMatching();
        PlayerRigidbodyOff();
    }
    private void OnDisable() {
        _isBagUIOpen = false;
    }


    private void playerStatusMatching() {
        Attack.text = string.Format("{0}", (int)playerAbility.GetTotalPlayerAttack());
        Cold.text = string.Format("{0}", (int)playerAbility.GetTotalPlayerColdResistance());
        Defence.text = string.Format("{0}", (int)playerAbility.GetTotalPlayerDefense());
        Speed.text = string.Format("{0}", (int)playerAbility.GetTotalPlayerSpeed());
    }




    public Canvas canvas;
    private RectTransform canvasRectTransform;
    private GameObject player;
    private GameObject playerClone;
    [SerializeField] private Vector2 playerPos;
    [SerializeField] private float scaleFactor;

    private void playerCloneInit() {

        // UI ��ҷ� ǥ���� ��ü�� ����
        playerClone = Instantiate(player, transform);
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
        playerCloneRect.SetParent(transform, false);
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
}

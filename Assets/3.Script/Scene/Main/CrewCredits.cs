using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CrewCredits : MonoBehaviour
{
    public GameObject creditPanel;
    private GameObject creditMove;
    private bool isCreditPanelMoving;
    [SerializeField] private float moveSpeed;
    private Vector3 credicPanelDefaltPos;

    public GameObject gitIcon;
    // 깃 링크
    private string giturl = "https://github.com/KY00JSSH/TheWildFour.git";
    private void Awake() {
        creditMove = creditPanel.transform.Find("CreditMove").gameObject;
        credicPanelDefaltPos = creditMove.transform.position;
        creditPanelOff();
    }

    private void Update() {
        // 판넬이 움직이고 있을 경우 아무곳이나 좌클릭하면 판넬 꺼지기
        if (isCreditPanelMoving) {
            if (Input.GetMouseButtonDown(0)) {
                // git button이 클릭된 상태를 제외하고 실행되어야함
                if (EventSystem.current.currentSelectedGameObject != gitIcon) {
                    creditPanelOff();
                    isCreditPanelMoving = false;
                }
            }
            else {
                Vector3 newPosition = creditMove.transform.position;
                newPosition.y += moveSpeed * Time.deltaTime;
                creditMove.transform.position = newPosition;
                // 크래딧이 끝까지 올라갔을 경우 off
                if (creditMove.transform.position.y >= 110f) {
                    isCreditPanelMoving = false;
                    creditPanelOff();
                }

            }
        }
    } 

    // crew button 클릭시 판넬이동
    public void moveCreditPanelOnClick() {
        isCreditPanelMoving = true;
        creditPanel.SetActive(true);
        StartCoroutine(moveCreditPanelAlphaChange_Co());
    }
    // panel alpha...
    private IEnumerator moveCreditPanelAlphaChange_Co() {

        Color creditColor = creditPanel.GetComponent<Image>().color;
        while (creditColor.a <= 0.588f ) {
            creditColor.a += Time.deltaTime;
            creditPanel.GetComponent<Image>().color = creditColor;
            yield return null;
        }
        creditColor.a = 0.588f;
    }

    // 판넬 위치 원위치 + 끄기
    private void creditPanelOff() {
        creditPanel.SetActive(false);
        Color creditColor = creditPanel.GetComponent<Image>().color;
        creditColor.a = 0;
        creditPanel.GetComponent<Image>().color = creditColor;
        creditMove.transform.position = credicPanelDefaltPos;
    }

    // git button url connect
    public void gitIconButton() {
        Application.OpenURL(giturl);
    }

    private void OnDisable() {
        creditPanelOff();
    }
}

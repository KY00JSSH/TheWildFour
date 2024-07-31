using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainTtBtnHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public Text highlightTexts;
    private List<Text> highlightbtnsTexts;

    private void Awake() {
        highlightbtnsTexts = new List<Text>();
        foreach (Transform child in transform) {
            if (child.TryGetComponent(out Button btn)) {
                highlightbtnsTexts.Add(child.GetComponentInChildren<Text>());
            }
        }
    }

    private void HighlightButton(Text pointbtn) {
        for (int i = 0; i < highlightbtnsTexts.Count; i++) {
            if (pointbtn == highlightbtnsTexts[i]) {
                highlightTexts.gameObject.SetActive(true);

                Vector3 newPosition = highlightTexts.transform.position;
                newPosition.y = highlightbtnsTexts[i].transform.position.y;
                highlightTexts.transform.position = newPosition;

                highlightTexts.text = highlightbtnsTexts[i].text;
            }
        }
    }
    private void HighlightOffButton() {
        highlightTexts.gameObject.SetActive(false);
        highlightTexts.text = "";
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (eventData.pointerEnter != null) {
            Text pointtext = eventData.pointerEnter.GetComponent<Text>();
            if (pointtext != null) {
                HighlightButton(pointtext);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (eventData.pointerEnter != null) {
            Text pointtext = eventData.pointerEnter.GetComponent<Text>();
            if (pointtext != null) HighlightOffButton();
        }
    }
}

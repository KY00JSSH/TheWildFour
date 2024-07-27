using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerSelectInfo : MonoBehaviour, IPointerExitHandler {
    private GameObject playerTexts;
    private void Awake() {
        playerTexts = transform.GetChild(0).gameObject;
        playerTexts.SetActive(false);
    }
    public void TooltipShow() {
        playerTexts.SetActive(true);
    }

    public void TooltipHide() {
        playerTexts.SetActive(false);
    }

    public void OnPointerExit(PointerEventData eventData) {
        TooltipHide();
    }
}

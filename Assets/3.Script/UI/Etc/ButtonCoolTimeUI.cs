using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCoolTimeUI : MonoBehaviour
{
    public Image img;
    public Button btn;
    [SerializeField] private float cooltime = 10f;
    [SerializeField] private bool isClicked = false;

    private float leftTime = 10f;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private ShelterManager shelterManager;

    public float ratio { get; private set; }

    private void Start() {
        if (shelterManager == null) shelterManager = FindObjectOfType<ShelterManager>();
        if (img == null) img = gameObject.GetComponent<Image>();
        if (btn == null) btn = gameObject.GetComponent<Button>();
    }

    public void Update() {
        if (isClicked) {
            if (leftTime > 0) {
                leftTime -= Time.deltaTime;
                if (leftTime < 0) {
                    leftTime = 0;
                    if (btn) btn.enabled = true;
                    isClicked = true;
                }

                ratio = 1.0f - (leftTime / cooltime);
                if (img) img.fillAmount = ratio;
            }
        }
    }

    public void StartCooltime() {

        if (btn.name.Contains("Move") && shelterManager.MovePoint <= 0) return;
        else if (btn.name.Contains("Attack") && shelterManager.AttackPoint <= 0) return;
        else if (btn.name.Contains("Gather") && shelterManager.GatherPoint <= 0) return;

        leftTime = cooltime;
        isClicked = true;
        if (btn) btn.enabled = false;
    }
}

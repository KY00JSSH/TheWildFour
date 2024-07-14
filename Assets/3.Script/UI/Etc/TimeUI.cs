using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TimeUI : MonoBehaviour {
    private TimeManager timeManager;
    public Text[] time;
    public Slider timeSlider;
    public Image timeIcon;
    public Sprite[] timeIcons;
    private int worldHour = 0;
    private int surviveDay = 0;
    private float silderValue = 0;

    private void Awake() {
        timeManager = FindObjectOfType<TimeManager>();
    }
    private void Update() {
        worldHour = (int)timeManager.GetWorldHour();
        surviveDay = timeManager.GetSurviveDay();
        if (worldHour > 12) { // π„
            worldHour -= 12;
            time[0].text = string.Format("{0} PM", worldHour);
            
        }
        else {// ≥∑
            time[0].text = string.Format("{0} AM", worldHour);
            timeIcon.sprite = timeIcons[0];
        }
        time[1].text = string.Format("{0} ¿œ", surviveDay);

        if (timeManager.isDay()) { // ≥∑

            silderValue = timeManager.GetWorldHour() - 6;
            timeIcon.sprite = timeIcons[0];
            timeSlider.value = silderValue / 12;
        }
        else {
            if (timeManager.GetWorldHour() <= 6) {
                silderValue = timeManager.GetWorldHour() + 6;
            }
            else {
                silderValue = timeManager.GetWorldHour() - 18;
            }
            timeIcon.sprite = timeIcons[1];
            timeSlider.value = silderValue / 12;
        }
        
        // ∏ﬁº“µÂ ∫Ø∞Êøπ¡§
    }


}

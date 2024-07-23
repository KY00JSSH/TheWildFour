using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagUI : MonoBehaviour
{
    private PlayerAbility playerAbility;
    [SerializeField] private GameObject statusTexts;
    private Text Attack;
    private Text Cold;
    private Text Defence;
    private Text Speed;


    private void Awake() {
        playerAbility = FindObjectOfType<PlayerAbility>();

        Attack = statusTexts.transform.GetChild(0).GetComponent<Text>();
        Cold = statusTexts.transform.GetChild(1).GetComponent<Text>();
        Defence = statusTexts.transform.GetChild(2).GetComponent<Text>();
        Speed = statusTexts.transform.GetChild(3).GetComponent<Text>();
    }

    private void OnEnable() {
        // 활성화 될때만 계산하게 할 것
        playerStatusMatching();
    }
    private void playerStatusMatching() {
        Attack.text = string.Format("{0}", (int)playerAbility.GetTotalPlayerAttack());
        Cold.text = string.Format("{0}", (int)playerAbility.GetTotalPlayerColdResistance());
        Defence.text = string.Format("{0}", (int)playerAbility.GetTotalPlayerDefense());
        Speed.text = string.Format("{0}", (int)playerAbility.GetTotalPlayerSpeed());
    }
}

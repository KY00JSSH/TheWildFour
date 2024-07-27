using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSave : MonoBehaviour
{
    [SerializeField] private GameObject TitleUI;

    public void OnEnterSaveSelect() {
        TitleUI.SetActive(false);
        gameObject.SetActive(true);
    }

    public void OnExitSaveSelect() {
        TitleUI.SetActive(true);
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectSave : MonoBehaviour
{
    [SerializeField] private GameObject TitleUI;
    [SerializeField] private GameObject SelectSaveUI;
    [SerializeField] private GameObject NewGameUI;
    [SerializeField] private GameObject LoadGameUI;

    public void OnEnterSaveSelect(Button callButton) {
        SelectSaveUI.SetActive(true);
        callButton.transform.parent.gameObject.SetActive(false);
    }

    public void OnExitSaveSelect(Button callButton) {
        TitleUI.SetActive(true);
        callButton.transform.parent.gameObject.SetActive(false);
    }

    public void OnEnterNewGame(Button callButton) {
        NewGameUI.SetActive(true);
        callButton.transform.parent.gameObject.SetActive(false);
        Save.Instance.InitSaveFile();
    }
    public void OnEnterLoadGamd(Button callButton) {
        LoadGameUI.SetActive(true); 
        callButton.transform.parent.gameObject.SetActive(false);
    }

    public void OnExitNewGame(Button callButton) {
        OnEnterSaveSelect(callButton);
    }

    public void OnExitLoadGame(Button callButton) {
        OnEnterSaveSelect(callButton);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            foreach (var each in GetComponentsInChildren<Button>()) {
                if (each.name == "ButtonEsc") {
                    each.onClick.Invoke();
                    break;
                }
            }
        }
    }
}

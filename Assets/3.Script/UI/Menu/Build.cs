using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Build : MonoBehaviour {
    /*
     건설에 해당하는 모든 스크립트에 들어가야하는 내용
    => build 버튼 클릭시 해당하는 내용
    1. 기존 null 스프라이트 저장
    2. bool 값 넣어주면 스프라이트 변경
     
    === 각종 prefab의 위치는 어떻게 찾아가는거지

     */

    public Sprite BuildAvailable;
    public Sprite BuildDisavailable;
    private Sprite previousSprite;
    private void Awake() {
        previousSprite = transform.GetComponent<Image>().sprite;
    }

    public virtual void BuildChecking(bool isValidBuild) {
        if (isValidBuild) transform.GetComponent<Image>().sprite = BuildAvailable;
        else transform.GetComponent<Image>().sprite = BuildAvailable;
    }

    private void OnDisable() {
        transform.GetComponent<Image>().sprite = previousSprite;
        transform.position = Vector3.zero;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Build : MonoBehaviour {
    /*
     �Ǽ��� �ش��ϴ� ��� ��ũ��Ʈ�� �����ϴ� ����
    => build ��ư Ŭ���� �ش��ϴ� ����
    1. ���� null ��������Ʈ ����
    2. bool �� �־��ָ� ��������Ʈ ����
     
    === ���� prefab�� ��ġ�� ��� ã�ư��°���

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

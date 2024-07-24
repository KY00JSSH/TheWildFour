using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivatedStatusControl : MonoBehaviour
{
    // ���� Ȱ��ȭ �Ǿ��ִ� �̹��� ����
    public GameObject[] CurrentActivateBoxs;
    // ���� Ȱ��ȭ ǥ���� ��������Ʈ
    public Sprite[] ActivatedStatusSprites;



    // return �� status 
    public Status AddActivatedStatus { get { return StatusControl.Instance.ActivatedStatus[StatusControl.Instance.ActivatedStatus.Count - 1].type; } }

    private List<GameObject> statusUIBoxs;

    private Vector2 defaultPos;

    private void Awake() {
        defaultPos = CurrentActivateBoxs[0].transform.position;
        CurrentActivateBoxs = new GameObject[5];
        foreach (GameObject item in CurrentActivateBoxs) {
            statusUIBoxs.Add(item);
        }
    }


}
/*
 1. StatusControl.Instance.ActivatedStatus�� ������ŭ box list�� ����
 2. �ڽ� ����Ʈ�� ���鼭 setactive�� false�� list���� ���� �������� ���� �� �ֳ�?
 3.  �ڽ� ����Ʈ�� �������� �ٸ��ٸ� ��ġ ����

=================
1. �ڸ�ƾ���� ���� ó��
2. ����Ʈ ���� �� ��ȸ�ؼ� �ѹ���
 */

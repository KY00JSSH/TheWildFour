using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivatedStatusControl : MonoBehaviour {
    // ���� Ȱ��ȭ �Ǿ��ִ� �̹��� ����
    public GameObject[] CurrentActivateBoxs;
    // ���� Ȱ��ȭ ǥ���� ��������Ʈ
    public Sprite[] ActivatedStatusSprites;

    // return �� status 
    public Status AddActivatedStatus { get; private set; }

    private bool isPosChangeComplete = false;

    private void Update() {
        if (StatusControl.Instance.ActivatedStatus != null) {
            // ���� ����Ʈ�� null�� �ƴ� ��� Ȱ��ȭ ������Ʈ ���� Ȯ��
            if (StatusControl.Instance.ActivatedStatus.Count > FindActiveBox()) {
                Debug.Log("Ȱ��ȭ ���� : " + StatusControl.Instance.ActivatedStatus.Count);
                BoxAddActivatedStatus();
                Debug.Log("�ڽ� Ȱ��ȭ status : " + AddActivatedStatus);
                Debug.Log("�ڽ� Ȱ��ȭ ���� : " + FindActiveBox());

            }

        }
        if (StatusControl.Instance.ActivatedStatus.Count <= FindActiveBox()) BoxAddActivePositionChange();

    }

    // Ȱ��ȭ �Ǿ��ִ� �ڽ� ������Ʈ ���� Ȯ��
    private int FindActiveBox() {
        int count = 0;
        for (int i = 0; i < CurrentActivateBoxs.Length; i++) {
            if (CurrentActivateBoxs[i].activeSelf) count++;
        }
        return count;
    }


    // Ȱ��ȭ ���¸�ŭ �迭 Ȱ��ȭ �ؾ��� => ���� �Ѱ������
    private void BoxAddActivatedStatus() {
        for (int i = 0; i < CurrentActivateBoxs.Length; i++) {
            if (!CurrentActivateBoxs[i].activeSelf) {
                AddActivatedStatus = StatusControl.Instance.ActivatedStatus[StatusControl.Instance.ActivatedStatus.Count - 1].type;
                CurrentActivateBoxs[i].SetActive(true);
                int positionDelta = StatusControl.Instance.ActivatedStatus.Count - 1;
                CurrentActivateBoxs[i].transform.position = new Vector2(CurrentActivateBoxs[i].transform.position.x + 65 * positionDelta, CurrentActivateBoxs[i].transform.position.y);
                return;
            }
        }
    }

    // Ȱ��ȭ�� ���� �ڽ��� ã�Ƽ� �׵ڿ� �ִ� �ڽ����� ��ġ�� �Űܾ���
    private void BoxAddActivePositionChange() {

        Debug.Log(StatusControl.Instance.ActivatedStatus.Count);
        Debug.Log("/ " + FindActiveBox());
        Debug.Log("BoxAddActivePositionChange !!!:");
        for (int i = 0; i < CurrentActivateBoxs.Length - 1; i++) {
            if (!CurrentActivateBoxs[i].activeSelf) {
                Debug.Log("CurrentActivateBoxs[i] :" + CurrentActivateBoxs[i].name);
                for (int j = i + 1; j < CurrentActivateBoxs.Length; j++) {
                    if (CurrentActivateBoxs[j].activeSelf) {
                        Debug.Log("CurrentActivateBoxs[j] :" + CurrentActivateBoxs[j].name);
                        CurrentActivateBoxs[j].transform.position = new Vector2(CurrentActivateBoxs[j].transform.position.x - 65, CurrentActivateBoxs[j].transform.position.y);
                    }
                }

            }
        }
    }

    // Ȱ��ȭ �� �ڽ� ������Ȯ���ؼ� ����ġ ã���� �ߴ� �޼ҵ�


}
/*
 1. StatusControl.Instance.ActivatedStatus�� ������ŭ �迭 Ȱ��ȭ
    - �ΰ��� �ѹ��� Ȱ��ȭ�� �Ǹ� ��� ��
 2. �ڽ� ����Ʈ�� ���鼭 setactive�� false 
 3.  �ڽ� ����Ʈ�� �������� �ٸ��ٸ� ��ġ ����

=================
1. �ڸ�ƾ���� ���� ó��
2. ����Ʈ ���� �� ��ȸ�ؼ� �ѹ���
 */

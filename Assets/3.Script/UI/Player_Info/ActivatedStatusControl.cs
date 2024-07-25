using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivatedStatusControl : MonoBehaviour {
    // ������ų ������
    public GameObject CurrentActivateBoxPrf;
    // ���� ����
    [SerializeField] private int boxNum = 5;
    // �ڽ� ����Ʈ
    private List<GameObject> CurrentActivateBoxs;
    // ���� Ȱ��ȭ ǥ���� ��������Ʈ
    public Sprite[] ActivatedStatusSprites;

    // return �� status 
    public Status AddActivatedStatus { get; private set; }

    // ���� ������ŭ�� default ��ġ ������
    private Vector2[] defaultPositions;

    private void Awake() {
        defaultPositions = new Vector2[boxNum];
        /* ���������� �ؼ� ��ġ�� �Ҿ������ ���
        if(boxNum != 0) {
            CurrentActivateBoxPrf.transform.position = new Vector2(-880, -380);
        }
        */
        CurrentActivateBoxs = new List<GameObject>();
        BoxActivatedStatusInit();
        BoxsDefaultPositionSave();
    }


    private void Update() {
        if (StatusControl.Instance.ActivatedStatus != null) {
            // ���� ����Ʈ�� null�� �ƴ� ��� Ȱ��ȭ ������Ʈ ���� Ȯ��
            int activeBoxCnt = FindActiveBox();
            if (StatusControl.Instance.ActivatedStatus.Count > activeBoxCnt) {
                BoxAddActivatedStatus(StatusControl.Instance.ActivatedStatus.Count - activeBoxCnt);
            }
            // ��Ȱ��ȭ Ȯ��
           if(BoxDelActivatedStatus()) BoxPositionSetting();
        }
    }

    // Ȱ��ȭ �Ǿ��ִ� �ڽ� ������Ʈ ���� Ȯ��
    private int FindActiveBox() {
        int count = 0;
        if (CurrentActivateBoxs.Count == 0) return count;

        for (int i = 0; i < CurrentActivateBoxs.Count; i++) {
            if (CurrentActivateBoxs[i].activeSelf) count++;
        }
        return count;
    }

    // ���ϴ� ������ŭ prefab ����Ʈ ����
    private void BoxActivatedStatusInit() {
        for (int i = 0; i < boxNum; i++) {
            // ������ ����
            GameObject gameObject = Instantiate(CurrentActivateBoxPrf, transform);
            gameObject.name = CurrentActivateBoxPrf.name;
            gameObject.transform.position = CurrentActivateBoxPrf.transform.position;
            CurrentActivateBoxs.Add(gameObject);
            CurrentActivateBoxs[i].SetActive(false);
        }
    }

    // ��ġ ���� ����
    private void BoxsDefaultPositionSave() {
        for (int i = 0; i < boxNum; i++) {
            defaultPositions[i] = new Vector2(CurrentActivateBoxPrf.transform.position.x + 65 * i, CurrentActivateBoxPrf.transform.position.y);
        }
    }


    // �޾ƿ� ���� ����Ʈ�� Ȱ��ȭ�Ǿ��ִ� prefab ����Ʈ ���̸�ŭ Ȱ��ȭ ����
    private void BoxAddActivatedStatus(int activeBoxCnt) {
        for (int i = 0; i < activeBoxCnt; i++) {
            for (int j = 0; j < CurrentActivateBoxs.Count; j++) {
                if (!CurrentActivateBoxs[j].activeSelf) {
                    //TODO: ���� �ѱ��
                    AddActivatedStatus = StatusControl.Instance.ActivatedStatus[StatusControl.Instance.ActivatedStatus.Count - (activeBoxCnt - i)].type;
                    CurrentActivateBoxs[j].SetActive(true);

                    // ���� ��ġ ��� _ ���� ���� ����Ʈ�� ����
                    int positionDelta = StatusControl.Instance.ActivatedStatus.Count - (activeBoxCnt - i);
                    CurrentActivateBoxs[j].transform.position = defaultPositions[positionDelta];
                    break;
                }
            }

        }
    }

    // ������ box����Ʈ �� ��Ȱ��ȭ �� ����Ʈ�� �ִٸ� ���� �� ��ġ �������
    private bool BoxDelActivatedStatus() {
        int cnt = 0;
        for (int i = 0; i < CurrentActivateBoxs.Count; i++) {

            if (!CurrentActivateBoxs[i].activeSelf) {
                // �� ��° ��Ҹ� �ӽ� ������ �����մϴ�.
                GameObject temp = CurrentActivateBoxs[i];

                // �� ��° ��Ҹ� ����Ʈ���� �����մϴ�.
                CurrentActivateBoxs.RemoveAt(i);

                // ������ ��Ҹ� ����Ʈ�� �������� �߰��մϴ�.
                CurrentActivateBoxs.Add(temp);
                cnt++;
            }
        }

        if (cnt > 0) return true;
        else return false;
    }

    // ������ box����Ʈ �� ����Ʈ�� �ε��� ������ �ȸ´� ��ġ�� �ִٸ� �����ؾ���
    private void BoxPositionSetting() {
        for (int i = 0; i < CurrentActivateBoxs.Count; i++) {
            if (CurrentActivateBoxs[i].transform.position.x != defaultPositions[i].x) {
                // �ڸ�ƾ���� ������ �°��� ��
                StartCoroutine(BoxPositionSetting_Co(i));
            }
        }
    }

    private IEnumerator BoxPositionSetting_Co(int i) {
        while (CurrentActivateBoxs[i].transform.position.x != defaultPositions[i].x) {
            Vector3 currentPosition = CurrentActivateBoxs[i].transform.position;
            float step = Time.deltaTime; // �ε巴�� �̵��ϴ� ����

            // ���� �ڽ��� �⺻ ��ġ ������ �����ʿ� �ִٸ�
            if (currentPosition.x > defaultPositions[i].x) {
                currentPosition.x = Mathf.Max(currentPosition.x - step, defaultPositions[i].x);
            }
            else {// ���� �ڽ��� �⺻ ��ġ ������ ���ʿ� �ִٸ�
                currentPosition.x = defaultPositions[i].x;
            }
            CurrentActivateBoxs[i].transform.position = currentPosition;
            yield return null;
        }
    }


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

using UnityEngine;
using UnityEngine.Events;

public class PlayerDead : MonoBehaviour {
    public static PlayerDead Instance = null;
    private void Awake() {
        if (Instance = null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public UnityEvent onDead;
    // �÷��̾ �׾��� �� ȣ��Ǿ�� �� �޼��带 ����ϴ� Action

    public void playerDie() { onDead.Invoke(); }
    // ���������� �׾����� �˷��� �� �� ȣ���ؾ� �� �޼���
}

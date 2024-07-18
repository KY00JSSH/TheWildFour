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
    // 플레이어가 죽었을 때 호출되어야 할 메서드를 등록하는 Action

    public void playerDie() { onDead.Invoke(); }
    // 실질적으로 죽었음을 알려야 할 때 호출해야 할 메서드
}

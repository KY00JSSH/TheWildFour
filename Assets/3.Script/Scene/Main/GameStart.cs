using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    public void PlayerSelect() {
        // 로딩 씬을 불러옴 - 로딩씬에서 비동기로 게임씬 부름
        SceneManager.LoadScene("PlayerSelect");
    }

    public void GameLoad() {
        SceneManager.LoadScene("Load");
    }
}

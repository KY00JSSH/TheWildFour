using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    public void StartGame() {
        // �ε� ���� �ҷ��� - �ε������� �񵿱�� ���Ӿ� �θ�
        SceneManager.LoadScene("Load");
    }
}

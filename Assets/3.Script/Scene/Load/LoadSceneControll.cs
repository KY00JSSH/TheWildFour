using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneControll : MonoBehaviour {
    // Start 메서드에서 비동기 로드 시작

    // start button
    private void Start() {
        AudioManager.instance.PlaySFX(AudioManager.Sfx.loading);
        StartCoroutine(LoadGameSceneAsync());
    }

    IEnumerator LoadGameSceneAsync() {
        // 비동기적으로 GameScene 로드 시작
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Game");

        // 로드가 완료될 때까지 대기
        while (!asyncLoad.isDone) {
            yield return null;
        }
    }

}


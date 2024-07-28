using UnityEngine;
using UnityEngine.UI;

public class SelectMode : MonoBehaviour {
    [SerializeField] private Sprite[] NormalModeSprite;
    [SerializeField] private Sprite[] ExtremeModeSprite;

    [SerializeField] private Image NormalModeImage;
    [SerializeField] private Image ExtremeModeImage;

    public void SetGameMode(bool gameMode) {
        if (gameMode) { // extreme
            NormalModeImage.sprite = NormalModeSprite[0];
            ExtremeModeImage.sprite = ExtremeModeSprite[1];
            Save.Instance.saveData.isExtreme = true;
        }
        else {          // noremal
            NormalModeImage.sprite = NormalModeSprite[1];
            ExtremeModeImage.sprite = ExtremeModeSprite[0];
            Save.Instance.saveData.isExtreme = false;
        }
    }
}
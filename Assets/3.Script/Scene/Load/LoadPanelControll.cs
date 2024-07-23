using UnityEngine;
using UnityEngine.UI;

public class LoadPanelControll : MonoBehaviour
{
    [SerializeField]
    private Sprite[] images;
    [SerializeField]
    private Image panelImage;


    void Start() {
        // 이미지 배열이 비어있지 않다면
        if (images.Length > 0) {
            int randomIndex = Random.Range(0, images.Length);
            panelImage.sprite = images[randomIndex];
        }
    }
}

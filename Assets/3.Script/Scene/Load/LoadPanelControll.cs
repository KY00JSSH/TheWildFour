using UnityEngine;
using UnityEngine.UI;

public class LoadPanelControll : MonoBehaviour
{
    [SerializeField]
    private Sprite[] images;
    [SerializeField]
    private Image panelImage;


    void Start() {
        // �̹��� �迭�� ������� �ʴٸ�
        if (images.Length > 0) {
            int randomIndex = Random.Range(0, images.Length);
            panelImage.sprite = images[randomIndex];
        }
    }
}

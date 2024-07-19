using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap_CompassRotation : MonoBehaviour
{
    public Transform miniMapCamera;

    public Text north;
    public Text east;
    public Text west;
    public Text south;

    private Quaternion initialRotation;

    private void Start()
    {
        initialRotation = Quaternion.identity;
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(0, 0, miniMapCamera.eulerAngles.y);

        north.rectTransform.rotation = initialRotation;
        east.rectTransform.rotation = initialRotation;
        west.rectTransform.rotation = initialRotation;
        south.rectTransform.rotation = initialRotation;
    }
}

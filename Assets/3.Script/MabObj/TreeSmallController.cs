using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSmallController : MonoBehaviour
{
    private int objectNumber;
    private Vector3 position;
    private bool enable;
    private int berrCount;
    private int type;

    public void InitializeObjData(SmallTreeData data)
    {
        objectNumber = data.objectNumber;
        position = new Vector3(data.position.x, data.position.y, data.position.z);
        enable = data.enable;
        berrCount = data.berrCount;
        type = data.type;

        transform.position = position;
        gameObject.SetActive(enable);
    }
}

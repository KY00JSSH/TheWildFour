using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    protected int objectNumber;
    protected Vector3 position;
    protected bool enable = true;
    protected float health;
    protected int type;

    public void InitializeObjData(ObjectData data)
    {
        objectNumber = data.objectNumber;
        position = new Vector3(data.position.x, data.position.y, data.position.z);
        enable = data.enable;
        health = data.health;
        type = data.type;

        transform.position = position;
        gameObject.SetActive(enable);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockController : MonoBehaviour
{
    private int objectNumber;
    private Vector3 position;
    private bool enable;
    private float health;
    private int type;

    [SerializeField]
    private GameObject thisRockObject;
    [SerializeField]
    private GameObject brokenObject;
    private BoxCollider coll;

    public bool isBroken = false;

    private void Start()
    {
        coll = GetComponent<BoxCollider>();
        brokenObject.SetActive(false);
    }

    public void InitializeObjData(RockData data)
    {
        objectNumber = data.objectNumber;
        position = new Vector3(data.position.x, data.position.y, data.position.z);
        enable = data.enable;
        health = data.health;
        type = data.type;

        transform.position = position;
        gameObject.SetActive(enable);
    }

    private void Update()
    {
        if (isBroken)
        {
            getDamage(20.5f);
            isBroken = false;
        }
    }

    public void getDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            enable = false;
            changeObjectModel();
        }
    }

    private void changeObjectModel()
    {
        if (type == 1)
        {
            brokenObject.transform.position = new Vector3(brokenObject.transform.position.x, 0.3f, brokenObject.transform.position.z);
        }
        brokenObject.SetActive(true);
        thisRockObject.SetActive(false);
        coll.enabled = false;
    }
}

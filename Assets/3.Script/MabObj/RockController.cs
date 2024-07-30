using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockController : MonoBehaviour {
    private int objectNumber;
    private Vector3 position;
    private bool enable;
    private float health;
    private int type;

    [SerializeField] private GameObject thisRockObject;     //원래 바위
    [SerializeField] private GameObject brokenObject;      //부서지고 바위
    [SerializeField] private GameObject dropRockPrf;        //때리면 드랍할 바위 prefab
    private BoxCollider coll;
    private RockSpawner rockSpawner;

    public bool isBroken = false;

    private void Start() {
        coll = GetComponent<BoxCollider>();
        brokenObject.SetActive(false);
    }

    public void InitializeObjData(RockData data) {
        objectNumber = data.objectNumber;
        position = new Vector3(data.position.x, data.position.y, data.position.z);
        enable = data.enable;
        health = data.health;
        type = data.type;

        transform.position = position;
        gameObject.SetActive(enable);
    }

    private void Update() {
        if (isBroken) {
            getDamage(20.5f);
            isBroken = false;
        }
    }

    public void getDamage(float damage) {
        health -= damage;
        if (health <= 0) {
            health = 0;
            enable = false;
            rockSpawner.UpdateRockData(objectNumber, enable, health, type == 1 ? true : false)
            changeObjectModel();
        }
    }

    private void changeObjectModel() {
        if (type == 1) {
            brokenObject.transform.position = new Vector3(brokenObject.transform.position.x, 0.3f, brokenObject.transform.position.z);
        }
        brokenObject.SetActive(true);
        thisRockObject.SetActive(false);
        coll.enabled = false;
    }

    public void dropRockItem(float gatherPoint) {
        InvenController invenController = FindObjectOfType<InvenController>();
        int checkNum = invenController.canAddThisBox(2);
        if (checkNum != 99) {
            CountableItem invenItem = invenController.Inventory[checkNum].GetComponent<CountableItem>();
            invenItem.addCurrStack((int)gatherPoint * 2);
            invenController.updateInvenInvoke();
        }
        else {
            int existBox = invenController.isExistEmptyBox();
            Vector3 itemDropPosition = new Vector3(gameObject.transform.position.x - 0.1f, gameObject.transform.position.y, gameObject.transform.position.z - 0.1f);
            GameObject itemObject = Instantiate(dropRockPrf, itemDropPosition, Quaternion.identity, rockSpawner.transform);
            itemObject.GetComponent<CountableItem>().setCurrStack((int)gatherPoint * 2);
            invenController.updateInvenInvoke();
            if (existBox != 99) {
                invenController.addIndexItem(existBox, itemObject);
                itemObject.SetActive(false);
            }
            else {
                itemObject.SetActive(true);
            }
        }
    }
}

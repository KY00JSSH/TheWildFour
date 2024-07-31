using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSmallController : MonoBehaviour {
    private int objectNumber;
    private Vector3 position;
    private bool enable;
    private int berrCount;
    private int type;

    public FoodItemData berrieItem;

    public void InitializeObjData(SmallTreeData data) {
        objectNumber = data.objectNumber;
        position = new Vector3(data.position.x, data.position.y, data.position.z);
        enable = data.enable;
        berrCount = data.berrCount;
        type = data.type;

        transform.position = position;
        gameObject.SetActive(enable);
    }

    private void Start() {
        if (berrCount > 0) {
            InstantBerries(berrCount);
        }
    }

    private void InstantBerries(int _berriesCnt) {
        for (int i = 0; i < _berriesCnt; i++) {
            if (i == 0) {
                GameObject dropItem = Instantiate(berrieItem.DropItemPrefab,
                    new Vector3(transform.position.x - 0.2f, transform.position.y, transform.position.z - 0.2f)
                    , Quaternion.identity);
                dropItem.transform.SetParent(gameObject.transform);
                dropItem.GetComponent<Rigidbody>().useGravity = false;
                dropItem.GetComponent<Rigidbody>().isKinematic = true;
            }
            else {
                GameObject dropItem = Instantiate(berrieItem.DropItemPrefab,
                   new Vector3(transform.position.x + 0.2f, transform.position.y, transform.position.z + 0.2f)
                   , Quaternion.identity);
                dropItem.transform.SetParent(gameObject.transform);
                dropItem.GetComponent<Rigidbody>().useGravity = false; 
                dropItem.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }
}

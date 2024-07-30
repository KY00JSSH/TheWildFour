using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBigController : MonoBehaviour {
    private int objectNumber;
    private Vector3 position;
    private bool enable = true;
    private float health;
    private int type;

    [SerializeField] private GameObject dropTreePrf;

    [SerializeField] private bool testBroke = false;

    private bool isFalling = false;
    private TreeSpawner treeSpawner;

    public void InitializeObjData(BigTreeData data) {
        objectNumber = data.objectNumber;
        position = new Vector3(data.position.x, data.position.y, data.position.z);
        enable = data.enable;
        health = data.health;
        type = data.type;

        transform.position = position;
        gameObject.SetActive(enable);
    }

    private void Awake() {
        treeSpawner = FindObjectOfType<TreeSpawner>();
    }

    public void getDamage(float damage) {
        health -= damage;
        if (health <= 0) {
            health = 0;
            enable = false;
        }
       // treeSpawner.UpdateTreeData(objectNumber, enable, health);
    }

    private void Update() {
        if (testBroke) {
            getDamage(10.2f);
            testBroke = false;
        }
    }

    private void FixedUpdate() {
        if (!enable && !isFalling) {
            disableTree();
        }
    }

    private void disableTree() {
        StartCoroutine(treeDisableCo());
    }

    private IEnumerator treeDisableCo() {
        //나무 쓰러지고 일정시간 유지후 destroy
        isFalling = true;

        Quaternion targetRotation = Quaternion.Euler(90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        float fallDuration = 2.0f;
        float elapsed = 0.0f;

        while (elapsed < fallDuration) {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, elapsed / fallDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
        float waitTime = 1.0f;
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }

    public void dropTreeItem(float gatherPoint) {
        InvenController invenController = FindObjectOfType<InvenController>();
        int checkNum = invenController.canAddThisBox(1);
        if (checkNum != 99) {
                CountableItem invenItem = invenController.Inventory[checkNum].GetComponent<CountableItem>();
                invenItem.addCurrStack((int)gatherPoint * 2);
                invenController.updateInvenInvoke();
        }
        else {
            int existBox = invenController.isExistEmptyBox();
                Vector3 itemDropPosition = new Vector3(gameObject.transform.position.x - 0.1f, gameObject.transform.position.y, gameObject.transform.position.z - 0.1f);
                GameObject itemObject = Instantiate(dropTreePrf, itemDropPosition, Quaternion.identity, treeSpawner.transform);
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

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
            AudioManager.instance.PlaySFX(AudioManager.Sfx.TreeDeath);
        }
        else {
            AudioManager.instance.PlaySFX(AudioManager.Sfx.TreePain);
            StartCoroutine(ShakeTree());
        }

        treeSpawner.UpdateTreeData(objectNumber, enable, health);
    }

    private IEnumerator ShakeTree() {
        float shakeSpeed = 25f;
        float direction = (Random.Range(0f, 1f) * 2 - 1); // Random.Range 수정

        for (int i = 0; i < 3; i++) {
            float targetRotationX = direction * Random.Range(1f, 3f);
            float targetRotationZ = direction * Random.Range(1f, 2f);

            Quaternion targetRotation = Quaternion.Euler(targetRotationX, 0, targetRotationZ);

            while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f) {
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * shakeSpeed);
                yield return null;
            }

            direction *= -1; // 반대 방향으로 흔들기 위해 방향 전환
        }

        // 원래 위치로 되돌리기
        Quaternion originalRotation = Quaternion.Euler(0, 0, 0);
        while (Quaternion.Angle(transform.rotation, originalRotation) > 0.1f) {
            transform.rotation = Quaternion.Lerp(transform.rotation, originalRotation, Time.deltaTime * shakeSpeed);
            yield return null;
        }
    }

    private void FixedUpdate() {
        if (!enable && !isFalling) {
            disableTree();
        }
    }

    private void disableTree() {
        gameObject.GetComponent<Collider>().enabled = false;
        dropDestroyTree();
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
        AudioManager.instance.PlaySFX(AudioManager.Sfx.TreeFall);

        transform.rotation = targetRotation;
        float waitTime = 1.0f;
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }

    private void dropDestroyTree() {
        int randomCount = Random.Range(1, 4);
        for (int i = 0; i < randomCount; i++) {
            Vector3 itemDropPosition = new Vector3(gameObject.transform.position.x - 0.1f, gameObject.transform.position.y, gameObject.transform.position.z - 0.1f);
            GameObject itemObject = Instantiate(dropTreePrf, itemDropPosition, Quaternion.identity, treeSpawner.transform);
            int randomStack = Random.Range(1, 9);
            itemObject.GetComponent<CountableItem>().setCurrStack(randomStack);
            ItemManager.Register(itemObject, Location.Normal);
        }
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
                ItemManager.Register(itemObject, Location.Normal);
            }
        }
    }
}

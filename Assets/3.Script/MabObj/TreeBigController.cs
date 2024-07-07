using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBigController : TreeController
{
    [SerializeField]
    private bool testBroke = false;

    private bool isFalling = false;
    private TreeSpawner treeSpawner;

    private void Awake()
    {
        treeSpawner = FindObjectOfType<TreeSpawner>();
    }


    public void getDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            enable = false;
        }
        treeSpawner.UpdateTreeData(objectNumber, enable, health);
    }

    private void Update()
    {
        if (testBroke)
        {
            getDamage(10.2f);
            testBroke = false;
        }
    }

    private void FixedUpdate()
    {
        if (!enable && !isFalling)
        {
            disableTree();
        }
    }

    private void disableTree()
    {
        StartCoroutine(treeDisableCo());
    }


    private IEnumerator treeDisableCo() //나무 쓰러지고 일정시간 유지후 destroy
    {
        isFalling = true;

        Quaternion targetRotation = Quaternion.Euler(90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        float fallDuration = 2.0f;
        float elapsed = 0.0f;

        while (elapsed < fallDuration)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, elapsed / fallDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
        float waitTime = 1.0f;
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }
}

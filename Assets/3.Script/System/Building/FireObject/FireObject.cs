using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireObject : MonoBehaviour, IFireLight {
    protected float totalTime, currentTime, tickTime;
    protected float HeatRange = 5f;
    protected bool isBurn = false;

    public bool IsBurn { get { return isBurn; } }

    protected bool isBuilding = true;
    private bool isBake = false;

    public bool IsBake { get { return isBake; } }

    private Light fireLight;
    protected ParticleSystem fireEffect;
    private InvenController invenCont;

    public float GetTotalTime() { return totalTime; }
    public float GetCurrentTime() { return currentTime; }

    public void LightUp(float intensity) {
        if (!isBuilding)
            fireLight.intensity = intensity;
    }

    public void LightOff() {
        fireLight.intensity = 0;
    }

    public virtual void AddWood() {
        invenCont = FindObjectOfType<InvenController>();
        if (invenCont.isInItem(1)) {
            invenCont.removeItemCount(1, 1);
            invenCont.updateInvenInvoke();
            IncreaseTime(10f);
            if (currentTime > 0 && !isBurn) {
                AudioManager.instance.PlaySFX(AudioManager.Sfx.CampfireKindle);
                StartCoroutine(Burn());
            }
        }
    }

    private IEnumerator Burn() {
        isBurn = true;
        fireEffect?.Play();
        while (currentTime > 0) {
            currentTime -= tickTime;
            yield return new WaitForSeconds(1f);
        }
        currentTime = 0;
        LightOff();
        isBurn = false;
        fireEffect?.Stop();
    }

    protected void IncreaseTime(float time) {
        currentTime += time;
        if (currentTime > totalTime) currentTime = totalTime;
    }

    protected virtual void OnCreated() {
        isBuilding = false;
        fireLight = GetComponentInChildren<Light>();
    }

    protected virtual void Update() {
        if (currentTime > 0) {
            Collider[] colliders = Physics.OverlapSphere(transform.position, HeatRange);
            foreach (Collider collider in colliders) {
                if (collider.CompareTag("Player")) {
                    PlayerStatus playerStatus = collider.GetComponentInParent<PlayerStatus>();
                    StatusControl.Instance.GiveStatus(Status.Heat, playerStatus);
                    break;
                }
            }
        }
    }

    public void BakeItem(int index, bool isFurnace) {

        isBake = true;

        InvenController inven = FindObjectOfType<InvenController>();
        GameObject bakeItem = Instantiate(inven.getIndexItem(index));
        CountableItem countItem = bakeItem.GetComponent<CountableItem>();

        if (isFurnace) {
            if (bakeItem.GetComponent<FoodItem>()!=null) {
                bakeItem.GetComponent<FoodItem>().setVisible();
                countItem.setCurrStack(1);
                inven.useItem(index);
            }
            else {
                if(countItem.itemData.Key == 2){
                    if(countItem.CurrStackCount >= 30) {
                        bakeItem.SetActive(true);
                        countItem.setCurrStack(30);
                        inven.removeItemCount(2, 30);
                    }
                }
            }
        }
        Rigidbody bakeRigidbody = bakeItem.GetComponent<Rigidbody>();
        bakeRigidbody.useGravity = false;
        Vector3 targetPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.7f, gameObject.transform.position.z);

        StartCoroutine(BakingCo(bakeItem, targetPosition, 3f, bakeItem));
    }

    private IEnumerator BakingCo(GameObject item, Vector3 targetPosition, float floatDuration, GameObject bakeItem) {
        float elapsedTime = 0f;
        while (elapsedTime < floatDuration) {
            if (currentTime <= 0) {
                bakeItem.GetComponent<Rigidbody>().useGravity = true;
                break;
            }
            item.transform.position = targetPosition;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //아이템 변경
        if(bakeItem.GetComponent<FoodItem>() != null) {
            if(bakeItem.GetComponent<FoodItem>()?.BakeItemPrf != null) {
                GameObject newBakeItem = Instantiate(bakeItem.GetComponent<FoodItem>()?.BakeItemPrf);
                newBakeItem.transform.position = targetPosition;
                ItemManager.Register(newBakeItem, Location.Normal);
                Destroy(bakeItem);
            }
            else {
                GameObject newBakeItem = Instantiate(invenCont.bakeIronPrf);
                newBakeItem.transform.position = targetPosition;
                newBakeItem.GetComponent<CountableItem>().setCurrStack(1);
                ItemManager.Register(newBakeItem, Location.Normal);
                Destroy(bakeItem);
            }
        }
    }
}

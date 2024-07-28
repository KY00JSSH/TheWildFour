using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenDrop : MonoBehaviour {
    private InvenController invenController;
    private GameObject player;

    private void Awake() {
        invenController = FindObjectOfType<InvenController>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    //아이템 한번 드랍
    public void dropItem(int selectBoxKey) {
        List<GameObject> inven = invenController.Inventory;
        if (inven[selectBoxKey] != null) {
            if (selectBoxKey >= 0 && selectBoxKey < inven.Count) {
                GameObject itemComponent = inven[selectBoxKey];
                Vector3 itemDropPosition = new Vector3(player.transform.position.x - 0.1f, player.transform.position.y + 1.5f, player.transform.position.z - 0.1f);

                if (itemComponent.GetComponent<CountableItem>() != null) {
                    //카운팅 되는 아이템일때
                    CountableItem countItem = itemComponent.GetComponent<CountableItem>();
                    if (invenController.checkItemType(selectBoxKey) == 1) {
                        //돌, 나무일때
                        if (countItem.CurrStackCount > 8) {
                            //인벤에 있는 돌, 나무가 8개보다 많을때 item 복제해서 떨굴 개수만큼 넣어주고 필드에 떨굼
                            GameObject dropItem = Instantiate(itemComponent, itemDropPosition, Quaternion.identity);
                            dropItem.GetComponent<CountableItem>().setCurrStack(8);
                            dropItem.SetActive(true);
                        }
                        else if (countItem.CurrStackCount <= 8) {
                            //인벤에 있는 돌, 나무가 8개 이하 이면 그 개수 그대로 현재 아이템 position만 변경하고 필드에 떨굼
                            itemComponent.transform.position = itemDropPosition;
                            itemComponent.SetActive(true);
                        }
                    }
                    else {
                        if (countItem.CurrStackCount > 1) {
                            GameObject dropItem = Instantiate(itemComponent, itemDropPosition, Quaternion.identity);
                            dropItem.GetComponent<CountableItem>().setCurrStack(1);
                            if (itemComponent.GetComponent<FoodItem>() != null) {
                                dropItem.GetComponent<FoodItem>().setVisible();
                                itemComponent.GetComponent<Rigidbody>().useGravity = true;
                                itemComponent.GetComponent<Rigidbody>().isKinematic = false;
                            }
                            else {
                                dropItem.SetActive(true);
                            }
                        }
                        else {
                            if (itemComponent.GetComponent<FoodItem>() != null) {
                                itemComponent.transform.position = itemDropPosition;
                                itemComponent.GetComponent<FoodItem>().setVisible();
                                itemComponent.GetComponent<Rigidbody>().useGravity = true;
                                itemComponent.GetComponent<Rigidbody>().isKinematic = false;
                            }
                            else {
                                itemComponent.transform.position = itemDropPosition;
                                itemComponent.SetActive(true);
                            }
                        }
                    }
                }
                else {
                    //카운팅용 아이템 아니면 그냥 포지션 변경해서 active → true
                    itemComponent.transform.position = itemDropPosition;
                    itemComponent.SetActive(true);
                }
                invenController.dropItem(selectBoxKey);
                invenController.invenFullFlagReset();
            }
            else {
                Debug.Log("Invalid selectBoxKey");
            }
        }
    }

    //인벤박스 아이템 뭉텅이 드랍
    public void dropItemAll(int selectBoxKey) {

        List<GameObject> inven = invenController.Inventory;

        if (inven[selectBoxKey] != null) {
            if (selectBoxKey >= 0 && selectBoxKey < inven.Count) {
                GameObject itemComponent = inven[selectBoxKey];
                Vector3 itemDropPosition = new Vector3(player.transform.position.x - 0.1f, player.transform.position.y + 1.5f, player.transform.position.z - 0.1f);

                itemComponent.transform.position = itemDropPosition;
                if (itemComponent.GetComponent<FoodItem>() != null) {
                    itemComponent.GetComponent<FoodItem>().setVisible();
                    itemComponent.GetComponent<Rigidbody>().useGravity = true;
                    itemComponent.GetComponent<Rigidbody>().isKinematic = false;
                }
                else {
                    itemComponent.SetActive(true);
                }

                invenController.removeItem(selectBoxKey);
            }
            else {
                Debug.Log("Invalid selectBoxKey");
            }
        }
    }

    //플레이어 죽을때 인벤 전부 drop
    public void dropAllSlotItems() {
        List<GameObject> inven = invenController.Inventory;

        for (int i = 0; i < inven.Count; i++) {

            if (inven[i] != null) {
                GameObject itemComponent = inven[i];
                Vector3 itemDropPosition = new Vector3(player.transform.position.x - 0.1f, player.transform.position.y + 1.5f, player.transform.position.z - 0.1f);

                itemComponent.transform.position = itemDropPosition;
                if (itemComponent.GetComponent<FoodItem>() != null) {
                    itemComponent.GetComponent<FoodItem>().setVisible();

                }
                else {
                    itemComponent.SetActive(true);
                }

                invenController.removeItem(i);
            }
        }
        invenController.invenFullFlagReset();
    }
}

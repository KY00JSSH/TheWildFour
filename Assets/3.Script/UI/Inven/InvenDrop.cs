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

    //������ �ѹ� ���
    public void dropItem(int selectBoxKey) {
        List<GameObject> inven = invenController.Inventory;
        if (inven[selectBoxKey] != null) {
            if (selectBoxKey >= 0 && selectBoxKey < inven.Count) {
                GameObject itemComponent = inven[selectBoxKey];
                Vector3 itemDropPosition = new Vector3(player.transform.position.x - 0.1f, player.transform.position.y + 1.5f, player.transform.position.z - 0.1f);

                if (itemComponent.GetComponent<CountableItem>() != null) {
                    //ī���� �Ǵ� �������϶�
                    CountableItem countItem = itemComponent.GetComponent<CountableItem>();
                    if (invenController.checkItemType(selectBoxKey) == 1) {
                        //��, �����϶�
                        if (countItem.CurrStackCount > 8) {
                            //�κ��� �ִ� ��, ������ 8������ ������ item �����ؼ� ���� ������ŭ �־��ְ� �ʵ忡 ����
                            GameObject dropItem = Instantiate(itemComponent, itemDropPosition, Quaternion.identity);
                            dropItem.GetComponent<CountableItem>().setCurrStack(8);
                            //������ �κ� ������ invencontroller���� ����
                            dropItem.SetActive(true);
                        }
                        else if (countItem.CurrStackCount <= 8) {
                            //�κ��� �ִ� ��, ������ 8�� ���� �̸� �� ���� �״�� ���� ������ position�� �����ϰ� �ʵ忡 ����
                            itemComponent.transform.position = itemDropPosition;
                            itemComponent.SetActive(true);
                        }
                    }
                }
                else {
                    //ī���ÿ� ������ �ƴϸ� �׳� ������ �����ؼ� active �� true
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

    //�κ��ڽ� ������ ������ ���
    public void dropItemAll(int selectBoxKey) {

        List<GameObject> inven = invenController.Inventory;

        if (inven[selectBoxKey] != null) {
            if (selectBoxKey >= 0 && selectBoxKey < inven.Count) {
                GameObject itemComponent = inven[selectBoxKey];
                Vector3 itemDropPosition = new Vector3(player.transform.position.x - 0.1f, player.transform.position.y + 1.5f, player.transform.position.z - 0.1f);

                itemComponent.transform.position = itemDropPosition;
                itemComponent.SetActive(true);

                invenController.removeItem(selectBoxKey);
                invenController.invenFullFlagReset();
            }
            else {
                Debug.Log("Invalid selectBoxKey");
            }
        }
    }

    //�÷��̾� ������ �κ� ���� drop
    public void dropAllSlotItems() {
        List<GameObject> inven = invenController.Inventory;

        for (int i = 0; i < inven.Count; i++) {

            if (inven[i] != null) {
                GameObject itemComponent = inven[i];
                Vector3 itemDropPosition = new Vector3(player.transform.position.x - 0.1f, player.transform.position.y + 1.5f, player.transform.position.z - 0.1f);

                itemComponent.transform.position = itemDropPosition;
                itemComponent.SetActive(true);

                invenController.removeItem(i);
            }
        }
        invenController.invenFullFlagReset();
    }
}

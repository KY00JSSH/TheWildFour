using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonInven : MonoBehaviour {
    protected List<GameObject> inventory = new List<GameObject>();
    public List<GameObject> Inventory { get { return inventory; } }

    public GameObject itemObject;

    public delegate void OnInvenChanged(List<GameObject> inventory);
    public event OnInvenChanged InvenChanged;

    private bool isInvenFull = false;               //�κ��丮 ��ü ���ְ� �������� �߰� ���� ����
    public bool IsInvenFull { get { return isInvenFull; } }

    public void updateInvenInvoke() {
        InvenChanged?.Invoke(inventory);
    }

    public void addInvenBox() {
        inventory.Add(null);
    }

    public void invenFullReset() {
        isInvenFull = false;
    }

    //�κ��� �߰� ���� flag reset
    public void invenFullFlagReset() {
        isInvenFull = false;
    }

    //������ ������ �κ����� ����
    public void removeItem(int index) {
        inventory[index] = null;
        invenFullFlagReset();
        updateInvenInvoke();
    }

    //if �ش� �������� inven�� �ְ�,(�ش� box item count < itemMaxStackCount)
    // �ش� ĭ�� ������ �߰�
    //  else if(!full)  ���ڽ��� ������ add
    //else isInvenFull = true
    public void ItemAdd() {
        int checkNum = canAddThisBox(itemObject.GetComponent<Item>().Key);

        if (checkNum != 99) {
            //�ش�ĭ�� ������ �߰�
            if (itemObject.GetComponent<CountableItem>() != null && inventory[checkNum] != null) {
                CountableItem invenItem = inventory[checkNum].GetComponent<CountableItem>();
                invenItem.addCurrStack(itemObject.GetComponent<CountableItem>().CurrStackCount);
            }
        }
        else {
            int existBox = isExistEmptyBox();
            if (existBox != 99) {
                //null�� ����� inventory�� �߰�
                inventory[existBox] = itemObject;
            }
            else {
                isInvenFull = true;
            }
        }
        updateInvenInvoke();

        for(int i = 0; i < inventory.Count; i++) {
            Debug.Log($" inven { i }  : {inventory[i]}");
        }
    }

    //������ �߰� ���ɿ���
    public bool canItemAdd() {
        Item item = itemObject.GetComponent<Item>();
        //�������� ���ļ� ���� �� �ִ��� Ȯ��
        int checkNum = canAddThisBox(item.Key);
        if (checkNum != 99) {
            return true;
        }
        else {
            int existBox = isExistEmptyBox();
            if (existBox != 99) {
                return true;
            }
            else {
                Debug.Log("���� box�� �߰� ����");
                return false;
            }
        }
    }

    //��ü �ڽ��� �ش� item�� �ְ�, �ش� ĭ�� �߰� ������ �� �ش� ĭ num�� return, ������ 99 return
    protected int canAddThisBox(int itemKey) {
        for (int i = 0; i < inventory.Count; i++) {
            if (inventory[i] != null) {
                if (inventory[i].GetComponent<Item>().Key == itemKey) {
                    if (inventory[i].GetComponent<CountableItem>() != null) {
                        CountableItem countItem = inventory[i].GetComponent<CountableItem>();
                        if (countItem?.CurrStackCount < countItem?.MaxStackCount) {
                            return i;
                        }
                    }
                }
            }
        }
        return 99;
    }

    //�� inven box�� �ִ��� ����
    public int isExistEmptyBox() {
        for (int i = 0; i < inventory.Count; i++) {
            if (inventory[i] == null) {
                //������ ���������� null�� �ʱ�ȭ �� inventory�϶��� �ش� index return
                return i;
            }
        }
        return 99;  //�ƿ� �� �ڽ��� �����Ҷ�
    }

    //������ 1�� ���
    public void useItem(int index) {
        if (index >= 0 && index < inventory.Count && inventory[index] != null) {
            if (inventory[index].GetComponent<CountableItem>() != null) {
                if (inventory[index].GetComponent<CountableItem>().CurrStackCount == 1) {
                    removeItem(index);
                }
                else {
                    inventory[index].GetComponent<CountableItem>().useCurrStack(1);
                }
            }
            else {
                removeItem(index);
            }
        }
        updateInvenInvoke();
    }

    //�ش� �ε����� ������ Ÿ�� üũ
    //type 1: ��,���� , 2: ī��Ʈ �Ǵ� item, 3: ī��Ʈ ���� ������, 0: ������ ����
    public int checkItemType(int index) {
        if (inventory[index] != null) {
            CountableItem countItem = inventory[index].GetComponent<CountableItem>();
            if (countItem != null) {
                if (countItem.itemData.Key == 1 || countItem.itemData.Key == 2) {
                    return 1;
                }
                else {
                    return 2;
                }
            }
            else {
                return 3;
            }
        }
        else {
            return 0;   //������ ������
        }
    }

    //������ ����� ������ ����
    public void dropItem(int index) {
        int itemType = checkItemType(index);
        if (itemType > 0 && itemType < 3) {
            CountableItem countItem = inventory[index].GetComponent<CountableItem>();
            if (itemType == 1) {
                if (countItem.CurrStackCount > 8) {
                    countItem.useCurrStack(8);
                }
                else if (countItem.CurrStackCount == 8) {
                    countItem.useCurrStack(8);
                    removeItem(index);
                }
                else {
                    countItem.useCurrStack(countItem.CurrStackCount);
                    removeItem(index);
                }
                updateInvenInvoke();
            }
            else if (itemType == 2) {
                countItem.useCurrStack(1);
                if (countItem.CurrStackCount == 0) {
                    removeItem(index);
                }
                updateInvenInvoke();
            }
        }
        else if (itemType == 3) {
            removeItem(index);
            updateInvenInvoke();
        }
    }

    //�κ� �����۳��� ����Ī
    public void changeInvenIndex(int currentIndex, int changeIndex) {
        if (currentIndex != changeIndex && changeIndex != 99) {
            if (inventory[changeIndex] != null) {
                GameObject changeItem = inventory[changeIndex];
                inventory[changeIndex] = inventory[currentIndex];
                inventory[currentIndex] = changeItem;
                updateInvenInvoke();
            }
            else {
                inventory[changeIndex] = inventory[currentIndex];
                inventory[currentIndex] = null;
                invenFullFlagReset();
                updateInvenInvoke();
            }
        }
    }

    //Ư�� �ε����� ������ �߰�
    public void addIndexItem(int index, GameObject newItem) {
        inventory[index] = newItem;
        updateInvenInvoke();
    }
}

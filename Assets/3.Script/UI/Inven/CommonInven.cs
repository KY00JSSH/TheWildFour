using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonInven : MonoBehaviour {
    protected List<GameObject> inventory = new List<GameObject>();
    public List<GameObject> Inventory { get { return inventory; } }

    public GameObject itemObject;

    public delegate void OnInvenChanged(List<GameObject> inventory);
    public event OnInvenChanged InvenChanged;

    private bool isInvenFull = false;               //인벤토리 전체 차있고 기존에도 추가 못함 여부
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

    //인벤에 추가 못함 flag reset
    public void invenFullFlagReset() {
        isInvenFull = false;
    }

    //아이템 뭉텅이 인벤에서 삭제
    public void removeItem(int index) {
        inventory[index] = null;
        invenFullFlagReset();
        updateInvenInvoke();
    }

    //특정 key인 아이템 count만큼 삭제
    public void removeItemCount(int key, int count) {
        int leftCount = count;
        for (int i = 0; i < inventory.Count; i++) {
            if (inventory[i] != null) {
                CountableItem countItem = inventory[i].GetComponent<CountableItem>();
                if (countItem.itemData.Key == key) {
                    if (countItem.CurrStackCount > leftCount) {
                        countItem.useCurrStack(leftCount);
                        return;
                    }
                    else if (countItem.CurrStackCount == leftCount) {
                        inventory[i] = null;
                        return;
                    }
                    else {
                        leftCount -= countItem.CurrStackCount;
                        inventory[i] = null;
                    }
                }
            }
        }
    }

    //if 해당 아이템이 inven에 있고,(해당 box item count < itemMaxStackCount)
    // 해당 칸에 아이템 추가
    //  else if(!full)  새박스에 아이템 add
    //else isInvenFull = true
    public void ItemAdd() {
        //item이 인벤토리 내에 있고, 있는 박스 안에 추가 가능하면 해당 inven index return, 없으면 99를 return
        int checkNum = canAddThisBox(itemObject.GetComponent<Item>().Key);

        if (checkNum != 99) {
            //해당칸에 아이템 추가
            if (itemObject.GetComponent<CountableItem>() != null && inventory[checkNum] != null) {
                CountableItem invenItem = inventory[checkNum].GetComponent<CountableItem>();
                invenItem.addCurrStack(itemObject.GetComponent<CountableItem>().CurrStackCount);
            }
        }
        else {
            //빈 박스 있는지 체크. 제일 작은 index부터 검사해서 있으면 해당 index return, 없으면 99 return
            int existBox = isExistEmptyBox();
            if (existBox != 99) {
                //null로 비워둔 inventory에 추가
                inventory[existBox] = itemObject;
            }
            else {
                isInvenFull = true;
            }
        }
        updateInvenInvoke();
    }

    //아이템 추가 가능여부
    public bool canItemAdd() {
        int existBox = isExistEmptyBox();
        if (existBox != 99) {
            return true;
        }
        else {
            //Debug.Log("기존 box에 추가 못함");
            return false;
        }
    }

    //전체 박스에 해당 item이 있고, 해당 칸에 추가 가능할 때 해당 칸 num을 return, 없을때 99 return
    public int canAddThisBox(int itemKey) {
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

    //빈 inven box가 있는지 여부
    public int isExistEmptyBox() {
        for (int i = 0; i < inventory.Count; i++) {
            if (inventory[i] == null) {
                //기존에 생성했지만 null로 초기화 한 inventory일때는 해당 index return
                return i;
            }
        }
        return 99;  //아예 빈 박스를 사용못할때
    }

    //특정 인덱스의 아이템 1개 사용
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

    //해당 인덱스의 아이템 타입 체크
    //type 1: 돌,나무 , 2: 카운트 되는 item, 3: 카운트 없는 아이템, 0: 아이템 없음
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
            return 0;   //아이템 없을때
        }
    }

    //아이템 드랍시 아이템 삭제
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

    //인벤 아이템끼리 스위칭
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

    //특정 인덱스에 아이템 추가
    public void addIndexItem(int index, GameObject newItem) {
        inventory[index] = newItem;
        updateInvenInvoke();
    }

    //특정 인덱스의 오브젝트 return
    public GameObject getIndexItem(int index) {
        if (inventory[index] != null) {
            return inventory[index];
        }
        else {
            return null;
        }
    }

    //key로 아이템 있는지 확인
    public bool isInItem(int key) {
        for(int i = 0; i < inventory.Count; i++) {
            if(inventory[i] != null) {
                if(inventory[i].GetComponent<Item>().itemData.Key == key) {
                    return true;
                }
            }
        }
        return false;
    }
}

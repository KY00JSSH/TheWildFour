using UnityEngine;

public class MenuWeapon : MonoBehaviour {

    [SerializeField]
    private GameObject firstSlot;
    [SerializeField]
    private GameObject secondSlot;

    private RectTransform firstBoxTransf;
    public RectTransform WeapFirstBoxPos { get { return firstBoxTransf; } }
    private RectTransform secondBoxTransf;
    public RectTransform WeapSecondBoxPos { get { return secondBoxTransf; } }

    private WeaponSlotControll firstCont;
    private WeaponSlotControll secondCont;

    private InvenController invenCont;

    private int currSelectSlot = 1; //���� ���õ� ���â ���� �⺻�� 1

    // PlayerWeaponEquip ���� �޾ư��� ���� ������Ƽ�Դϴ�.
    public int CurrentSelectSlot { get { return currSelectSlot; } }

    private void Update() {
        if (PlayerStatus.isDead) return;

        if (Input.GetKeyDown(KeyCode.X)) {
            if(currSelectSlot == 1) {
                setCurrSelectSlot(2);
            }
            else {
                setCurrSelectSlot(1);
            }
        }
    }

    private void Start() {
        firstBoxTransf = firstSlot.GetComponent<RectTransform>();
        secondBoxTransf = secondSlot.GetComponent<RectTransform>();
        firstCont = firstSlot.GetComponent<WeaponSlotControll>();
        secondCont = secondSlot.GetComponent<WeaponSlotControll>();
        invenCont = FindObjectOfType<InvenController>();
    }

    public void setCurrSelectSlot(int slotNum) {
        currSelectSlot = slotNum;
        if(slotNum ==1) {
            firstCont.enableCursor();
            secondCont.disableCursor();
        }
        else {
            firstCont.disableCursor();
            secondCont.enableCursor();
        }
    }

    public GameObject addItemBox(int index, GameObject item) {
        if (index == 1) {
            //1�� ���Կ� ������ �߰�
            GameObject reItem = getcurrentItem(index);
            firstCont.setWeaponSlot(item);
            return reItem;
        }
        else {
            //2�� ���Կ� ������ �߰�
            GameObject reItem = getcurrentItem(index);
            secondCont.setWeaponSlot(item);
            return reItem;
        }
    }

    public void removeItem(int index) {
        if (index == 1) {
            //1�� ���Կ� ������ ����
            firstCont.setWeaponSlot(null);
        }
        else {
            //2�� ���Կ� ������ ����
            secondCont.setWeaponSlot(null);
        }
    }

    public GameObject getcurrentItem(int index) {
        //������ �ʵ��� item return
        if (index == 1) {
            GameObject item = firstCont.returnItem();
            return item;
        }
        else {
            GameObject item = secondCont.returnItem();
            return item;
        }
    }

    public void switchingSlot(int index) {
        GameObject firstWeapon = firstCont?.CurrentItem ;
        GameObject secondWeapon = secondCont?.CurrentItem ;

        if (firstWeapon && secondWeapon) {
            //�Ѵ� ������ ������ ���� ����Ī
            firstCont.setWeaponSlot(secondWeapon);
            secondCont.setWeaponSlot(firstWeapon);
        }
        else {
            if (index == 1) {
                //������ �������� 1�� ���� -> 2�� �������� �巡��
                firstCont.setWeaponSlot(null);
                secondCont.setWeaponSlot(firstWeapon);
            }
            else {
                //������ �������� 2�� ���� -> 1�� �������� �巡��
                firstCont.setWeaponSlot(secondWeapon);
                secondCont.setWeaponSlot(null);
            }
        }
    }

    public void addToInventory(int index, int target) {
        if (invenCont?.Inventory[target] !=null) {
            //�ش�ĭ�� ���� ������ ������ ����Ī
            if(invenCont?.Inventory[target].GetComponent<WeaponItem>() != null) {
                if (index == 1) {
                    invenCont.addIndexItem(target, firstCont.CurrentItem);
                    firstCont.setWeaponSlot(invenCont?.Inventory[target]);
                }
                else {
                    invenCont.addIndexItem(target, secondCont.CurrentItem);
                    secondCont.setWeaponSlot(invenCont?.Inventory[target]);
                }
            }
            else {
                return;
            }
        }
        else {
            //�ش�ĭ�� ������ �߰�
            if (index == 1) {
                invenCont.addIndexItem(target, firstCont.CurrentItem);
                firstCont.setWeaponSlot(null);
            }
            else {
                invenCont.addIndexItem( target, secondCont.CurrentItem);
                secondCont.setWeaponSlot(null);
            }
        }
    }

    public void addSlotFromInvenWeapon(int target, GameObject item) {
        if (currSelectSlot == 1) {
            GameObject firstWeapon = firstCont?.CurrentItem;
            if (firstWeapon) {
                firstCont.setWeaponSlot(item);
                invenCont.addIndexItem( target , firstWeapon);
            }
            else {
                firstCont.setWeaponSlot(item);
                invenCont.removeItem(target);
            }
        }
        else {
            GameObject secondWeapon = secondCont?.CurrentItem;
            if (secondWeapon) {
                secondCont.setWeaponSlot(item);
                invenCont.addIndexItem(target, secondWeapon);
            }
            else {
                secondCont.setWeaponSlot(item);
                invenCont.removeItem(target);
            }
        }
    }
}


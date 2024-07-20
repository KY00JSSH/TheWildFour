using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum ItemDurability {
    high,
    mid,
    low,
    zero
}

public class Tooltip_Inven : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    /*
     inven box�� ����
    // weap �� ��� tooltip�� ������ + �����̴� �ؾ��� + ���ݷµ� ����
     */
    private InventoryBox inventoryBox;
    [SerializeField] private GameObject Tooltip_inven;
    private Vector2 tooltipPos;
    private Text textTitle;
    private Text textMain;
    private Image itemImg;
    // ����� ��� tooltip�� �����̵�
    private Slider weapSlider;

    private Slider invenBoxSlider;
    // ���� �����̴� ��Ÿ��
    private float currentTime;

    private bool isItemUse = false;

    public ItemDurability ItemDurability { get; private set; }
    private Image durability_Food;
    private Image durability_Weap;

    private void Awake() {
        inventoryBox = GetComponent<InventoryBox>();
        if (Tooltip_inven == null) transform.GetChild(2);
        textTitle = Tooltip_inven.transform.GetChild(1).GetChild(0).GetComponent<Text>();
        textMain = Tooltip_inven.transform.GetChild(1).GetChild(1).GetComponent<Text>();
        itemImg = Tooltip_inven.transform.GetChild(2).GetComponent<Image>();
        invenBoxSlider = transform.GetChild(3).GetComponent<Slider>();

        durability_Food = Tooltip_inven.transform.GetChild(3).GetComponent<Image>();
        durability_Weap = Tooltip_inven.transform.GetChild(4).GetComponent<Image>();
        weapSlider = itemImg.transform.GetChild(0).GetComponent<Slider>();

        tooltipPos = new Vector2(550, 150);
    }
    private void Update() {
        if (inventoryBox.isItemIn) {

            // �������� ������ ���
            if (inventoryBox.CurrentItem is FoodItem || inventoryBox.CurrentItem is EquipItem) {

                if (inventoryBox.CurrentItem is FoodItem) {

                    if (durability_Weap.gameObject.activeSelf) {
                        durability_Weap.gameObject.SetActive(false);
                        invenBoxSlider.gameObject.SetActive(false);
                    }

                    currentTime += Time.deltaTime;
                    InvenItemDurabilityText_Food();
                }
                else if (inventoryBox.CurrentItem is WeaponItem weaponItem) {
                    if (invenBoxSlider.gameObject.activeSelf) {
                        invenBoxSlider.gameObject.SetActive(false);
                        durability_Food.gameObject.SetActive(false);
                    }

                    weapSlider.gameObject.SetActive(true);
                    InvenItemDurabilityText_Weap(weaponItem);
                }

                float value = InvenBoxItemSlider();
                ItemDurability = InvenItemDurabilityCheck(value);
                InvenItemDurabilityShow();
            }

        }
        else {
            // �������� ����߰ų� ������ ���
            invenBoxSlider.transform.Find("Background").GetComponent<Image>().color = Color.white;
            currentTime = 0;
            invenBoxSlider.value = 1;
            invenBoxSlider.gameObject.SetActive(false);
            weapSlider.gameObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (eventData.pointerEnter == gameObject) {
            Debug.Log("=======���콺 �Էµ�==========");
            if (inventoryBox.CurrentItem?.itemData != null) {
                Debug.Log("=================");
                Tooltip_inven.SetActive(true);
                InvenBoxItemInfo();
            }
            else {
                Debug.Log("�κ��丮�� null��");
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (Tooltip_inven.activeSelf) {
            Tooltip_inven.SetActive(false);
        }
    }

    private void InvenBoxItemInfo() {
        Tooltip_inven.transform.position = tooltipPos;
        textTitle.text = inventoryBox.CurrentItem.itemData.name;
        textMain.text = inventoryBox.CurrentItem.itemData.Description;
        itemImg.sprite = inventoryBox.CurrentItem.itemData.Icon;
    }

    private float InvenBoxItemSlider() {
        if (inventoryBox.CurrentItem is FoodItem foodItem) {
            invenBoxSlider.gameObject.SetActive(true);
            durability_Food.gameObject.SetActive(true);
            invenBoxSlider.value = (foodItem.foodItemData.TotalDecayTime - currentTime) / foodItem.foodItemData.TotalDecayTime;
        }
        else if (inventoryBox.CurrentItem is WeaponItem weaponItem) {
            durability_Weap.gameObject.SetActive(true);
            invenBoxSlider.gameObject.SetActive(true);
            invenBoxSlider.value = weaponItem.equipItemData.CurrDurability / weaponItem.equipItemData.TotalDurability;
        }
        return invenBoxSlider.value;
    }

    public ItemDurability InvenItemDurabilityCheck(float slidervalue) {
        if (slidervalue >= 0.6f) return ItemDurability.high;
        else if (slidervalue >= 0.35f) return ItemDurability.mid;
        else if (slidervalue > 0f) return ItemDurability.low;
        else return ItemDurability.zero;
    }

    private void InvenItemDurabilityShow() {
        Image fillImage = invenBoxSlider.fillRect.GetComponent<Image>();
        if (ItemDurability == ItemDurability.high) {
            fillImage.color = Color.green;
        }
        else if (ItemDurability == ItemDurability.mid) {
            fillImage.color = Color.yellow;
        }
        else if (ItemDurability == ItemDurability.low) {
            fillImage.color = Color.red;
        }
        else if (ItemDurability == ItemDurability.zero) {
            invenBoxSlider.transform.Find("Background").GetComponent<Image>().color = Color.gray;
        }
    }
    private void InvenItemDurabilityText_Food() {
        Text text = durability_Food.GetComponentInChildren<Text>();
        if (ItemDurability == ItemDurability.high) {
            durability_Food.color = Color.green;
            text.text = "�ż���";
        }
        else if (ItemDurability == ItemDurability.mid) {
            durability_Food.color = Color.yellow;
            text.text = "����";
        }
        else if (ItemDurability == ItemDurability.low) {
            durability_Food.color = Color.red;
            text.text = "����";
        }
        else if (ItemDurability == ItemDurability.zero) {
            durability_Food.color = Color.gray;
            text.text = "������";
        }
    }
    private void InvenItemDurabilityText_Weap(WeaponItem weaponItem) {
        Text text = durability_Weap.GetComponentInChildren<Text>();
        text.text = string.Format("{0} - {1}", weaponItem.WeaponItemData.MinPowerPoint, weaponItem.WeaponItemData.MaxPowerPoint);

        Image fillImage = weapSlider.fillRect.GetComponent<Image>();
        Text slidertext = weapSlider.transform.GetChild(2).GetComponent<Text>();
        slidertext.text = string.Format("{0} - {1}", weaponItem.WeaponItemData.CurrDurability, weaponItem.WeaponItemData.TotalDurability);
        if (ItemDurability == ItemDurability.high) {
            fillImage.color = Color.green;
            slidertext.color = Color.green;
        }
        else if (ItemDurability == ItemDurability.mid) {
            fillImage.color = Color.yellow;
            slidertext.color = Color.yellow;
        }
        else if (ItemDurability == ItemDurability.low) {
            fillImage.color = Color.red;
            slidertext.color = Color.red;
        }
        else if (ItemDurability == ItemDurability.zero) {
            weapSlider.transform.Find("Background").GetComponent<Image>().color = Color.gray;
            slidertext.transform.Find("Background").GetComponent<Image>().color = Color.gray;
        }
    }

}

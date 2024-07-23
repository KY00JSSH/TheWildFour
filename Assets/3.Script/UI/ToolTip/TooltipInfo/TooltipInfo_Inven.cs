using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipInfo_Inven : MonoBehaviour
{
    [SerializeField] protected GameObject Tooltip_inven;
    protected Vector2 tooltipPos;
    protected Text textTitle;
    protected Text textMain;
    protected Image itemImg;

    // ����� ��� tooltip�� �����̵�
    protected Slider weapSlider;
    protected Slider invenBoxSlider;

    protected Image durability_Food;
    protected Image durability_Weap;

    // �������� ���� ������ ������
    protected Item _item;

    protected virtual void Awake() {

        // tooltip
        if (Tooltip_inven == null) transform.GetChild(2);
        // ������ �̸�, ����
        textTitle = Tooltip_inven.transform.GetChild(1).GetChild(0).GetComponent<Text>();
        textMain = Tooltip_inven.transform.GetChild(1).GetChild(1).GetComponent<Text>();
        // ������ �̹���, ����� �����̴�
        itemImg = Tooltip_inven.transform.GetChild(2).GetComponent<Image>();
        weapSlider = itemImg.transform.GetChild(0).GetComponent<Slider>();

        // �κ��丮 �����̴�
        invenBoxSlider = transform.Find("Slider").GetComponent<Slider>();

        durability_Food = Tooltip_inven.transform.GetChild(3).GetComponent<Image>();
        durability_Weap = Tooltip_inven.transform.GetChild(4).GetComponent<Image>();

        tooltipPos = new Vector2(550, 170);
    }

    protected virtual void WeapItemOff() {
        invenBoxSlider.value = 1;
        invenBoxSlider.gameObject.SetActive(false);
        durability_Weap.gameObject.SetActive(false);
        weapSlider.gameObject.SetActive(false);
    }
    protected virtual void FoodItemOff() {
        invenBoxSlider.value = 1;

        invenBoxSlider.gameObject.SetActive(false);
        durability_Food.gameObject.SetActive(false);
    }

    protected virtual void InvenBoxItemInfo() {
        Tooltip_inven.transform.position = tooltipPos;
        textTitle.text = _item.itemData.name;
        textMain.text = _item.itemData.Description;
        itemImg.sprite = _item.itemData.Icon;
    }

    protected virtual void InvenItemText_Weap(WeaponItem currentWeap) {
        durability_Weap.gameObject.SetActive(true);
        invenBoxSlider.gameObject.SetActive(true);
        invenBoxSlider.value = currentWeap.CurrDurability / currentWeap.equipItemData.TotalDurability;


        float slidervalue = invenBoxSlider.value;
        Text text = durability_Weap.GetComponentInChildren<Text>();
        text.text = string.Format("{0} - {1}", currentWeap.WeaponItemData.MinPowerPoint, currentWeap.WeaponItemData.MaxPowerPoint);

        Image fillImage = weapSlider.fillRect.GetComponent<Image>();
        Text slidertext = weapSlider.transform.GetChild(2).GetComponent<Text>();
        slidertext.text = string.Format("{0} - {1}", currentWeap.CurrDurability, currentWeap.WeaponItemData.TotalDurability);


        Image fillImage_box = invenBoxSlider.fillRect.GetComponent<Image>();

        if (slidervalue >= 0.6f) {
            fillImage.color = Color.green;
            slidertext.color = Color.green;
            fillImage_box.color = Color.green;
        }
        else if (slidervalue >= 0.35f) {
            fillImage.color = Color.yellow;
            slidertext.color = Color.yellow;
            fillImage_box.color = Color.yellow;
        }
        else if (slidervalue > 0f) {
            fillImage.color = Color.red;
            slidertext.color = Color.red;
            fillImage_box.color = Color.red;
        }
        else {
            weapSlider.transform.Find("Background").GetComponent<Image>().color = Color.gray;
            //slidertext.transform.Find("Background").GetComponent<Image>().color = Color.gray;
            //fillImage_box.transform.Find("Background").GetComponent<Image>().color = Color.gray;
        }
    }

    protected virtual void InvenItemText_Food(FoodItem currentFood) {
        durability_Food.gameObject.SetActive(true);
        invenBoxSlider.gameObject.SetActive(true);
        invenBoxSlider.value = currentFood.CurrDecayTime / currentFood.foodItemData.TotalDecayTime;

        // �ż��� ǥ�� �ؽ�Ʈ 
        Text text = durability_Food.GetComponentInChildren<Text>();

        Image fillImage_box = invenBoxSlider.fillRect.GetComponent<Image>();
        switch (currentFood.Status) {
            case ItemStatus.Fresh:
                durability_Food.color = Color.green;
                fillImage_box.color = Color.green;
                text.text = "�ż���";
                break;
            case ItemStatus.Spoiled:
                durability_Food.color = Color.yellow;
                fillImage_box.color = Color.yellow;
                text.text = "����";
                break;
            case ItemStatus.Rotten:
                durability_Food.color = Color.gray;
                fillImage_box.color = Color.red;
                text.text = "������";
                weapSlider.transform.Find("Background").GetComponent<Image>().color = Color.gray;
                fillImage_box.transform.Find("Background").GetComponent<Image>().color = Color.gray;
                break;
            default:
                break;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tooltip_InvenWeap : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    private WeaponSlotControll weaponSlotControll;
    [SerializeField] private GameObject Tooltip_inven;
    private Vector2 tooltipPos;
    private Text textTitle;
    private Text textMain;
    private Image itemImg;
    // 장비일 경우 tooltip의 슬라이드
    private Slider weapSlider;

    private Slider invenBoxSlider;
    private WeaponItemData currentWeap;

    public ItemDurability ItemDurability { get; private set; }
    private Image durability_Weap;

    private void Awake() {
        weaponSlotControll = GetComponent<WeaponSlotControll>();
        if (Tooltip_inven == null) transform.GetChild(2);
        textTitle = Tooltip_inven.transform.GetChild(1).GetChild(0).GetComponent<Text>();
        textMain = Tooltip_inven.transform.GetChild(1).GetChild(1).GetComponent<Text>();
        itemImg = Tooltip_inven.transform.GetChild(2).GetComponent<Image>();
        invenBoxSlider = transform.Find("Slider").GetComponent<Slider>();

        durability_Weap = Tooltip_inven.transform.GetChild(4).GetComponent<Image>();
        weapSlider = itemImg.transform.GetChild(0).GetComponent<Slider>();

        tooltipPos = new Vector2(550, 150);
    }

    private void Update() {
        // 아이템이 들어왔을 경우
        if(weaponSlotControll.CurrentItem?.itemData) {
            if (weaponSlotControll.CurrentItem?.itemData is WeaponItemData weapon) {
                currentWeap = weapon;
            }
        }
        if (currentWeap != null) {
            if (weaponSlotControll.CurrentItem?.itemData) {
                weapSlider.gameObject.SetActive(true);
                InvenItemDurabilityText_Weap(currentWeap);

                float value = InvenBoxItemSlider();
                ItemDurability = InvenItemDurabilityCheck(value);
                InvenItemDurabilityShow();
            }
            else {

                // 아이템을 사용했거나 버렸을 경우
                invenBoxSlider.transform.Find("Background").GetComponent<Image>().color = Color.white;
                invenBoxSlider.value = 1;
                invenBoxSlider.gameObject.SetActive(false);
                weapSlider.gameObject.SetActive(false);
            }
        }
      
        
        if (Input.GetKeyDown(KeyCode.Escape)) Tooltip_inven.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (eventData.pointerEnter == gameObject) {
            if (weaponSlotControll.CurrentItem?.itemData) {
                if(weaponSlotControll.CurrentItem.itemData is WeaponItemData weapon) {
                    currentWeap = weapon;
                    Tooltip_inven.SetActive(true);
                    InvenBoxItemInfo();
                }
            }
            else {
                Debug.Log("인벤토리가 null임");
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
        textTitle.text = weaponSlotControll.CurrentItem.itemData.name;
        textMain.text = weaponSlotControll.CurrentItem.itemData.Description;
        itemImg.sprite = weaponSlotControll.CurrentItem.itemData.Icon;
    }
    private float InvenBoxItemSlider() {
        if (weaponSlotControll.CurrentItem is WeaponItem weaponItem) {
            durability_Weap.gameObject.SetActive(true);
            invenBoxSlider.gameObject.SetActive(true);
            invenBoxSlider.value = weaponItem.equipItemData.CurrDurability / weaponItem.equipItemData.TotalDurability;
        }
        return invenBoxSlider.value;
    }

    private void InvenItemDurabilityText_Weap(WeaponItemData weaponItem) {
        Text text = durability_Weap.GetComponentInChildren<Text>();
        text.text = string.Format("{0} - {1}", weaponItem.MinPowerPoint, weaponItem.MaxPowerPoint);

        Image fillImage = weapSlider.fillRect.GetComponent<Image>();
        Text slidertext = weapSlider.transform.GetChild(2).GetComponent<Text>();
        slidertext.text = string.Format("{0} - {1}", weaponItem.CurrDurability, weaponItem.TotalDurability);
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
}

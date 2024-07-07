using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inven_Bottom_Box : MonoBehaviour
{

    /*
     Inven_Bottom_Controll ���� ���� ������ ����
     
     
     */

    // �κ� �ڽ� -> ��ư
    private Button Inven_Box;
    [SerializeField] private Text Inven_Text;
    [SerializeField] private Image Inven_Sprite;


    private int Item_count = 0;

    private int Click_count = 0;

    // �������� ����ϴ��� Ȯ��
    public bool isItemUse = false;
    // �������� ���ִ��� Ȯ��
    public bool isItemIn = false;
    // �������� 80�� á�ٸ�
    public bool isInvenBoxAvailable = true;

    // �κ� ���� ������ 
    public GameObject Inven_Item {get; private set; }

    private void Awake()
    {
        Inven_Box = transform.GetComponent<Button>();
        Inven_Text = transform.GetChild(0).GetComponent<Text>();
        Inven_Text.text = Item_count.ToString();

        //TODO: ������ ��������Ʈ�� ���� �޾ƿö� Ȯ��
        Inven_Sprite = transform.GetChild(1).GetComponent<Image>();
        Inven_Sprite.enabled = false;
    }


    /*
    private void Update()
    {
        if (isItemIn)
        {
            Inven_Box.onClick.AddListener(ItemUse_Button);
            ItemUse();
        }
    }
    */

    //  �������� �߰��� ��� ������ ����
    public void ItemIn(GameObject item)
    {
        if (Inven_Item != null)
        {
            Item_count++;
            ItemCountCheck();
        }
        else
        {
            Inven_Item = item;
            isItemIn = true;
            Item_count++;
            ItemCountCheck();
        }
        Debug.Log("������ �̸� Ȯ��" + item.name);
        Debug.Log("������ count Ȯ��" + Item_count);
        Inven_Text.text = Item_count.ToString();
        Inven_Sprite.enabled = true;
    }

    // ������ ���
    private void ItemUse()
    {
        if (Input.GetKey(KeyCode.F)&& isItemIn)
        {
            isItemUse = true;

            //TODO: ������ ����ؾ��մϴ�
            if (Item_count<=0)
            {
                // �ʱ�ȭ
                Init_InvenBox();
            }
        }
        isItemUse = false;
    }

    private void ItemUse_Button()
    {
        Click_count++;
        if (Click_count >= 2 && isItemIn)
        {
            Click_count = 0;
            isItemUse = true;
            //TODO: ������ ����ؾ��մϴ�
            Item_count--;
            if (Item_count <= 0)
            {
                // �ʱ�ȭ
                Init_InvenBox();
            }
        }
        isItemUse = false;
    }

    private void ItemCountCheck()
    {
        if (Inven_Item.tag == "Item_Food" && Item_count >= 80)
        {
            isInvenBoxAvailable = false;
        }
        else if (Inven_Item.tag == "Item_Weapon" && Item_count >= 1)
        {
            isInvenBoxAvailable = false;
        }
        else if (Inven_Item.tag == "Item_Ingre" && Item_count >= 80)
        {
            isInvenBoxAvailable = false;
        }
        else if (Inven_Item.tag == "Item_Etc" && Item_count >= 1)
        {
            isInvenBoxAvailable = false;
        }
    }


    // �ʱ�ȭ Ȥ�� ������
    private void Init_InvenBox()
    {
        Item_count = 0;
        isItemUse = false;
        isItemIn = false;
        isInvenBoxAvailable = true;
        //itemImgOj = transform.GetComponent<Image>().sprite;
        Inven_Sprite.enabled = false;
    }

}

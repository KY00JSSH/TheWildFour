using UnityEngine;

public class Item : MonoBehaviour {
    public ItemData itemData;

    public int Key => itemData.Key;
}

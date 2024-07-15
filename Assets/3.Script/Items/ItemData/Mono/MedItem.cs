using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedItem : CountableItem {
    public MedicItemData medicItemData;

    public float HealTime => medicItemData.HealTime;
}

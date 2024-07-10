using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CountableItem : Item {

    public CountableItemData CountableData { get; private set; }

    public int CurrCount { get; protected set; }
    public int MaxCount => CountableData.MaxCount;
    public bool IsMax => CurrCount >= CountableData.MaxCount;
    public bool IsEmpty => CurrCount <= 0;

    public CountableItem(CountableItemData data, int count = 1) : base(data) {
        CountableData = data;
        SetCount(count);
    }

    public void SetCount(int count) {   //���� ���� max ���Ϸ� ����
        CurrCount = Mathf.Clamp(count, 0, MaxCount);
    }

    public int AddCountAndGetExcess(int count) {    //maxCount �̻��̸� �ٸ� ������ �߰�
        int nextCount = CurrCount + count;
        SetCount(nextCount);

        return (nextCount > MaxCount) ? (nextCount - MaxCount) : 0;
    }

    public CountableItem SeperateAndClone(int count) {
        // ������ �Ѱ� ������ ���, ���� �Ұ�
        if (CurrCount <= 1) return null;

        if (count > CurrCount - 1)
            count = CurrCount - 1;

        CurrCount -= count;
        return Clone(count);
    }

    protected abstract CountableItem Clone(int count);
}
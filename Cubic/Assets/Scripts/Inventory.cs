using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Singleton<Inventory> {

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public List<Item> items = new List<Item>();
    public int space = 6;

    public List<GameObject> blocksPrefabs;

    public int cursorSlot = 0;

    public bool processingItem = false;

    public bool sameItem = false;
    public int itemInsertIndex;

    public int currentSlotStorage = 0;
    public int[] amountItemSlots;

    private void Awake()
    {
        
    }

    public bool Add(Item item)
    {
        processingItem = true;
        if (items.Count >= space)
            return false;

        if(items.Count > 0)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].name == item.name)
                {
                    sameItem = true;
                    itemInsertIndex = i;
                    Debug.Log("Added one of the same " + item.name);
                }
                else
                {
                    items.Add(item);
                }
            }
        }
        else
        {
            items.Add(item);
        }


        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
       
        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    public void MoveCursorToSlot(int slot)
    {
        if (HasItemOnSlot(slot))
        {
            cursorSlot = slot;
            InventoryUI.Instance.UpdateUI();
        }
    }

    public void KeepCursorOnItemSlot()
    {
        while (!HasItemOnSlot(cursorSlot))
        {
            MoveCursorToSlot(cursorSlot - 1);
        }
    }

    bool HasItemOnSlot(int slot)
    {
        return (items.Count >= slot) ? true : false;
    }

    public GameObject PickBlock()
    {
        if(items[cursorSlot - 1] != null)
        {
            switch (items[cursorSlot - 1].name)
            {
                case "DirtItem(Clone)":
                    return ReturnBlock(0);
                default:
                    return new GameObject();
            }
        }
        return null;
    }

    GameObject ReturnBlock(int index)
    {
        if(items[cursorSlot - 1].amountItem > 1)
        {
            items[cursorSlot - 1].amountItem--;
        }
        else
        {
            items.RemoveAt(cursorSlot - 1);
        }

        KeepCursorOnItemSlot();
        return blocksPrefabs[index];
    }
}

using UnityEngine;

public class InventoryUI : Singleton<InventoryUI> {

    public Transform itemsParent;

    Inventory inventory;

    InventorySlot[] slots;

	// Use this for initialization
	void Start () {
        inventory = Inventory.Instance;
        inventory.onItemChangedCallback += UpdateUI;

        slots = GetComponentsInChildren<InventorySlot>();
	}

    public void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if(i < inventory.items.Count)
            {
                if (inventory.sameItem)
                {
                    if(i == inventory.itemInsertIndex)
                    {
                        slots[i].AddItem(inventory.items[i], true);
                    }
                    else
                    {
                        slots[i].AddItem(inventory.items[i]);
                    }
                }
                else
                {
                    slots[i].AddItem(inventory.items[i]);
                }
            }
            else
            {
                slots[i].ClearSlot();
            }

            if(slots[i].name == inventory.cursorSlot.ToString())
            {
                slots[i].PlaceCursorOn();
            }
            else
            {
                slots[i].RemoveCursor();
            }
        }
    }
}

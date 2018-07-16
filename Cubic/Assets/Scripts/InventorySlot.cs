using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {

    Item item;
    public Image icon;
    public Button removeButton;
    private Image removeButtonImage;
    public Image cursorImage;
    public Text amountItemText;
    public bool isCursorOn = false;

    private void Start()
    {
        removeButtonImage = removeButton.GetComponent<Image>();
    }

    public void AddItem(Item newItem, bool isSame = false)
    {
        if (!isSame)
        {
            item = newItem;
            icon.sprite = item.icon;
            icon.enabled = true;
            amountItemText.enabled = true;
            item.amountItem = 1;
            amountItemText.text = item.amountItem.ToString();
            removeButton.interactable = true;
            removeButtonImage.enabled = true;
        }
        else
        {
            item.amountItem++;
            amountItemText.text = item.amountItem.ToString();
        }
    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
        amountItemText.enabled = false;
        removeButton.interactable = false;
        removeButtonImage.enabled = false;
    }

    public void OnRemoveButton()
    {
        Inventory.Instance.Remove(item);
    }

    public void PlaceCursorOn()
    {
        isCursorOn = true;
        cursorImage.enabled = true;
    }

    public void RemoveCursor()
    {
        isCursorOn = false;
        cursorImage.enabled = false;
    }
}

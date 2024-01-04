using UnityEngine;
using UnityEngine.UI;

public class InventoryItemController : MonoBehaviour
{
    Item item;
    public Button RemoveButton;

    public void RemoveItem()
    {
        InventoryManager.Instance.Remove(item);

        Destroy(gameObject);
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
    }

    public void UseItem()
    {
        switch (item.itemType)
        {
            case Item.ItemType.Heart:
                Player.Instance.Cure(item.value);
                break;
            case Item.ItemType.HealthPot:
                Player.Instance.IncreaseMaxHealth(item.value);
                break;
            case Item.ItemType.StrengthPot:
                Player.Instance.IncreaseDamage(item.value);
                break;
        }

        RemoveItem();
    }

}
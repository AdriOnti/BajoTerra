using UnityEngine;
using UnityEngine.UI;

public class InventoryItemController : MonoBehaviour
{
    Item item;
    public Button RemoveButton;
    //public static InventoryItemController instance;

    // Funcion que borra un item del inventario
    public void RemoveItem()
    {
        InventoryManager.Instance.Remove(item);
        Destroy(gameObject);
    }

    // A�ade un nuevo item al inventario
    public void AddItem(Item newItem) { item = newItem; }

    // Funcion para usar un item. Una vez que se usa, se elimina del inventario
    public void UseItem()
    {
        switch (item.itemType)
        {
            case Item.ItemType.Heart:
                Player.Instance.IncreaseMaxHealth(item.value);
                break;
            case Item.ItemType.HealthPot:
                Player.Instance.Cure(item.value);
                break;
            case Item.ItemType.StrengthPot:
                Player.Instance.IncreaseDamage(item.value);
                break;
            case Item.ItemType.SpeedPot:
                Player.Instance.IncreaseSpeed(item.value);
                break;
        }
        RemoveItem();
    }

}
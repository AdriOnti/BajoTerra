using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;

    public void Pickup()
    {
        InventoryManager.Instance.Add(item);
        if(item.itemType != Item.ItemType.Key) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player") Pickup();
    }
}
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;

    void Pickup()
    {
        InventoryManager.Instance.Add(item);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Pickup();
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Inventory")]
    public List<Item> Items = new List<Item>();
    public Transform ItemContent;
    public GameObject InventoryItem;
    public Toggle EnableRemove;
    public InventoryItemController[] InventoryItems;

    private void Awake() { Instance = this; }

    public void Add(Item item) { Items.Add(item); }

    public void Remove(Item item) { Items.Remove(item); }

    public void ListItems()
    {
        // Clean content before open
        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in Items)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemName = obj.transform.Find("ItemName").GetComponent<Text>();
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            var rmBtn = obj.transform.Find("RmBtn").GetComponent<Button>();

            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;

            if (EnableRemove.isOn)  rmBtn.gameObject.SetActive(true);
        }
        SetInventoryItems();
    }

    // Funcion para que se puedan borrar los items sin usarlos
    public void EnableItemsRemove()
    {
        // Si el toggle esta en ON, el boton para quitar items aparece. Si esta en OFF, estara desactivado el boton
        if (EnableRemove.isOn)
        {
            foreach (Transform item in ItemContent) item.Find("RmBtn").gameObject.SetActive(true);
        }
        else
        {
            foreach (Transform item in ItemContent) item.Find("RmBtn").gameObject.SetActive(false);
        }
    }

    // Pone los items que estan como hijos del contenido del canvas
    public void SetInventoryItems()
    {
        // Limpia completamente el arreglo InventoryItems
        Array.Clear(InventoryItems, 0, InventoryItems.Length);

        // Obtiene los nuevos elementos hijos de ItemContent
        InventoryItems = ItemContent.GetComponentsInChildren<InventoryItemController>();

        for (int i = 0; i < Items.Count; i++)
        {
            if (i < InventoryItems.Length && InventoryItems[i] != null)
            {
                InventoryItems[i].AddItem(Items[i]);
            }
            else
            {
                Debug.LogWarning($"InventoryItemController missing at index {i}");
            }
        }
    }
}
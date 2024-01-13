using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Inventory")]
    [SerializeField] public List<Item> Items = new List<Item>();
    public Transform ItemContent;
    public GameObject InventoryItem;
    public Toggle EnableRemove;
    public InventoryItemController[] InventoryItems;
    public GameObject Inventory;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void Add(Item item) 
    {
        Item newItem = Instantiate(item);
        Items.Add(newItem); 
    }

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
        Debug.Log(ItemContent.childCount);
        InventoryItems = ItemContent.GetComponentsInChildren<InventoryItemController>();
        Debug.Log(ItemContent.childCount);

        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i] != null) InventoryItems[i].AddItem(Items[i]);
        }
    }

    public void ClearInventoryItems()
    {
        InventoryItems = new InventoryItemController[0];
        Inventory.gameObject.SetActive(false);
    }
}
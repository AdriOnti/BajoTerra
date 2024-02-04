using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    //public GameObject KeyCont;
    //public Text KeyTXT;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            List<Item> Items = InventoryManager.Instance.Items;
            foreach(Item item in Items)
            {
                if (item.itemType == Item.ItemType.Key)
                {
                    GameManager.Instance.ToggleDialog(1);
                }
                //else
                //{
                //    Debug.Log("NO KEY");
                //    KeyTXT.text = "NOT WORTHY";
                //}
            }
        }
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if(collision.tag == "Player")
    //    {
    //        KeyTXT.text = "";
    //        KeyCont.SetActive(false);
    //    }
    //}
}

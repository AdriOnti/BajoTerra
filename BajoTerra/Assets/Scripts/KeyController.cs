using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyController : MonoBehaviour
{
    public GameObject KeyCont;
    public Text KeyTXT;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            List<Item> Items = InventoryManager.Instance.Items;
            foreach(Item item in Items)
            {
                if (item.itemType == Item.ItemType.Key)
                {
                    KeyCont.SetActive(true);
                    Debug.Log("YOU WIN");
                    KeyTXT.text = "YOU WIN";
                }
                else
                {
                    Debug.Log("NO KEY");
                    KeyTXT.text = "NOT WORTHY";
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            //KeyTXT.text = "";
            KeyCont.SetActive(false);
        }
    }
}

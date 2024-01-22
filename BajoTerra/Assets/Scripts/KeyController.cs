using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("1");
        if(collision.tag == "Player")
        {
            //Debug.Log("2");
            List<Item> Items = InventoryManager.Instance.Items;
            foreach(Item item in Items)
            {
                //Debug.Log("3");
                if (item.itemType == Item.ItemType.Key)
                {
                    //Debug.Log("4");
                    Debug.Log("YOU WIN");
                }
                else
                {
                    Debug.Log("NO KEY");
                }
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

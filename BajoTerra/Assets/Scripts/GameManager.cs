using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    int trapType;
    public static GameManager instance;

    // Start is called before the first frame update
    void Start()
    {
        trapType = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetInventory()
    {
        GameObject inventory = GameObject.Find("Inventory");
        Debug.Log(inventory);
        return inventory;
    }
}

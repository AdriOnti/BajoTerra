using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public List<GameObject> rooms;
    private string path; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void SpawnRoom()
    {
        System.Random rnd = new System.Random();
        int rndRoom = rnd.Next(1,7);
        switch (path)
        {
            case "N":
                //random de 
                break;
            case "S":
                break;
            case "E":
                break;
            case "O":
                break;
        }
    }
}

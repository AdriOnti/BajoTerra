using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    protected Transform roomBoard;
    protected Transform roomParent;
    protected List<Transform> rooms = new List<Transform>();
    protected string path;

    // Start is called before the first frame update
    private void Start()
    {
        switch (tag)
        {
            case "NDoor":
                path = "N";
                break;
            case "SDoor":
                path = "S";
                break;
            case "EDoor":
                path = "E";
                break;
            case "ODoor":
                path = "O";
                break;
        }
    }

    void Awake()
    {
        roomBoard = GameObject.FindGameObjectWithTag("RoomManager").transform;
        roomParent = GameObject.FindGameObjectWithTag("Rooms").transform;
        foreach (Transform child in roomParent)
        {
            foreach (Transform child2 in child)
            {
                rooms.Add(child2);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnRoom()
    {
        switch (path)
        {
            case "N":
                break;
            case "S":
                break;
            case "E":
                break;
            case "O":
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SetChildrenPositions();
    }

    void SetChildrenPositions()
    {
        int i = 0;
        foreach (Transform child in roomBoard)
        {
            foreach (Transform child2 in child)
            {
                rooms[i].SetParent(child2);
                rooms[i].position = child2.position;
                i++;
            }
        }
    }
}

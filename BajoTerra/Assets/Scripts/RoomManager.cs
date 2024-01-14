using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    protected Transform roomBoard;
    protected Transform roomParent;
    protected List<Transform> boardSpaces = new List<Transform>();
    protected List<Transform> rooms = new List<Transform>();
    protected string path;
    protected Transform player;

    // Start is called before the first frame update
    private void Start()
    {

    }

    void Awake()
    {
        roomBoard = GameObject.FindGameObjectWithTag("RoomManager").transform;
        roomParent = GameObject.FindGameObjectWithTag("Rooms").transform;

        foreach (Transform child in roomBoard)
        {
            boardSpaces.Add(child);
        }

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player = collision.gameObject.transform;
        }

        switch (tag)
        {
            case "N":
                path = "S";
                player.position = new Vector3(player.position.x, player.position.y + 4.5f, player.position.z);
                break;
            case "S":
                path = "N";
                player.position = new Vector3(player.position.x, player.position.y - 4.5f, player.position.z);
                break;
            case "E":
                path = "O";
                player.position = new Vector3(player.position.x + 3f, player.position.y, player.position.z);
                break;
            case "O":
                path = "E";
                player.position = new Vector3(player.position.x - 3f, player.position.y, player.position.z);
                break;
        }

        SetRoom();
    }

    void SetRoom()
    {
        Debug.Log(path);

        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].name.Contains(path))
            {
                Debug.Log("a");
                rooms[i].SetParent(boardSpaces[3]);
                rooms[i].position = boardSpaces[3].position;
                break;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Paths : MonoBehaviour
{
    protected bool usedDoor;
    protected Transform player;
    protected string path;
    protected string parent;
    public bool startPath;

    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(startPath);
        if (!startPath) parent = transform.parent.gameObject.transform.parent.gameObject.name;
        else parent = "6_3";

        if (collision.gameObject.tag == "Player")
        {
            player = collision.gameObject.transform;

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

            Debug.Log (parent);
            Debug.Log (path);

            if (!usedDoor) RoomManager.Instance.SetRoom(parent, path);
            usedDoor = true;
        }
    }
}

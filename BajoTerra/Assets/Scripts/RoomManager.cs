using System;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    protected Transform roomBoard;
    protected Transform roomParent;
    protected List<Transform> boardSpaces = new List<Transform>();
    protected List<Transform> rooms = new List<Transform>();
    public string[,] roomBoard2 = new string[6,9];
    protected Dictionary<boardRooms, Transform> roomsDictionary = new();
    public static RoomManager Instance;

    public enum boardRooms
    {
        R0_0, R0_1, R0_2, R0_3, R0_4, R0_5, R0_6, R0_7, R0_8, R0_9,
        R1_0, R1_1, R1_2, R1_3, R1_4, R1_5, R1_6, R1_7, R1_8, R1_9,
        R2_0, R2_1, R2_2, R2_3, R2_4, R2_5, R2_6, R2_7, R2_8, R2_9,
        R3_0, R3_1, R3_2, R3_3, R3_4, R3_5, R3_6, R3_7, R3_8, R3_9,
        R4_0, R4_1, R4_2, R4_3, R4_4, R4_5, R4_6, R4_7, R4_8, R4_9,
        R5_0, R5_1, R5_2, R5_3, R5_4, R5_5, R5_6, R5_7, R5_8, R5_9
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    void Awake()
    {
        for (int i = 0; i < roomBoard2.GetLength(0); i++)
        {
            for (int j = 0; j < roomBoard2.GetLength(1); j++)
            {
                roomBoard2[i, j] = "0";
            }
        }

        roomBoard = GameObject.FindGameObjectWithTag("RoomManager").transform;
        roomParent = GameObject.FindGameObjectWithTag("Rooms").transform;

        foreach (Transform child in roomBoard)
        {
            boardSpaces.Add(child);
            bool sucess;
            if (sucess = Enum.TryParse(child.gameObject.name, out boardRooms value))
            {
                roomsDictionary.Add(value, child);
                Debug.Log(sucess);
            }
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

    public void SetRoom(string path)
    {
        Debug.Log(path);

        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].name.Contains(path))
            {
                rooms[i].SetParent(boardSpaces[3]);
                rooms[i].position = boardSpaces[3].position;
                break;
            }
        }
    }

    string ShowRoomboard()
    {
        string matrix = "";
        for (int i = 0; i < roomBoard2.GetLength(0); i++)
        {
            for (int j = 0; j < roomBoard2.GetLength(1); j++)
            {
                matrix += roomBoard2[i, j] + ",";
            }
            matrix += "\n";
        }
        return matrix;
    }
}

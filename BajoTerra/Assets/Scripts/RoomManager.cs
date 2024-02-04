using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    protected Transform roomBoard;
    protected Transform roomParent;
    protected List<Transform> boardSpaces = new List<Transform>();
    protected List<Transform> rooms = new List<Transform>();
    public string[,] roomBoard2 = new string[6,9];
    public static RoomManager Instance;
    Transform desiredKey;

    // Start is called before the first frame update
    private void Start()
    {

    }

    void Awake()
    {
        Instance = this;

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

    public void SetRoom(string position, string direction)
    {
        if (CheckRoom(position, direction)) return;

        List<Transform> setRooms = new List<Transform>();
        int random;

        /*int[] boardPositions = new int[2];
        string northLimit;
        string eastLimit;
        string westLimit;
        string southLimit;

        boardPositions[0] = int.Parse(position.Substring(0, 1));
        boardPositions[1] = Convert.ToInt32(position.Substring(2, 1));

        switch (direction)
        {
            case "N":
                northLimit = CheckRoom((boardPositions[0] + 1) + "_" + (boardPositions[1]), direction);
                eastLimit = CheckRoom((boardPositions[0] + 1) + "_" + (boardPositions[1]), direction);
                westLimit = CheckRoom((boardPositions[0] + 1) + "_" + (boardPositions[1]), direction);
                break;
            case "S":
                southLimit = CheckRoom((boardPositions[0] + 1) + "_" + (boardPositions[1]), direction);
                eastLimit = CheckRoom((boardPositions[0] + 1) + "_" + (boardPositions[1]), direction);
                westLimit = CheckRoom((boardPositions[0] + 1) + "_" + (boardPositions[1]), direction);
                break;
            case "E":
                northLimit = CheckRoom((boardPositions[0] + 1) + "_" + (boardPositions[1]), direction);
                southLimit = CheckRoom((boardPositions[0] + 1) + "_" + (boardPositions[1]), direction);
                eastLimit = CheckRoom((boardPositions[0] + 1) + "_" + (boardPositions[1]), direction);
                break;
            case "O":
                northLimit = CheckRoom((boardPositions[0] + 1) + "_" + (boardPositions[1]), direction);
                southLimit = CheckRoom((boardPositions[0] + 1) + "_" + (boardPositions[1]), direction);
                westLimit = CheckRoom((boardPositions[0] + 1) + "_" + (boardPositions[1]), direction);
                break;
        }*/

        SoundManager.Instance.PlayAudio(2);

        foreach (Transform room in rooms)
        {
            if (room.gameObject.name.Contains(direction))
            {
                setRooms.Add(room);
            }
        }
        random = UnityEngine.Random.Range(0, setRooms.Count);

        Debug.Log(position[0] + "_" + position[2]);


        setRooms[random].SetParent(desiredKey);
        setRooms[random].position = desiredKey.position;
        rooms.Remove(setRooms[random]);
        EnemiesManager.Instance.SetEnemie(setRooms[random],1);
        ItemManager.Instance.SetItem(setRooms[random]);
    }

    public bool CheckRoom(string position, string direction)
    {
        bool isRoom = false;
        int[] boardPositions = new int[2];

        boardPositions[0] = int.Parse(position.Substring(0, 1));
        boardPositions[1] = Convert.ToInt32(position.Substring(2, 1));

        switch (direction)
        {
            case "N":
                boardPositions[0]++;
                break;
            case "S":
                boardPositions[0]--;
                break;
            case "E":
                boardPositions[1]--;
                break;
            case "O":
                boardPositions[1]++;
                break;
        }

        foreach (Transform room in roomBoard)
        {
            if (room.gameObject.name == boardPositions[0] + "_" + boardPositions[1])
            {
                desiredKey = room;
                break;
            }
        }

        if (desiredKey.childCount > 0) isRoom = true;

        return isRoom;
    }

    //string ShowRoomboard()
    //{
    //    string matrix = "";
    //    for (int i = 0; i < roomBoard2.GetLength(0); i++)
    //    {
    //        for (int j = 0; j < roomBoard2.GetLength(1); j++)
    //        {
    //            matrix += roomBoard2[i, j] + ",";
    //        }
    //        matrix += "\n";
    //    }
    //    return matrix;
    //}
}

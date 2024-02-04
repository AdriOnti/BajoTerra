using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    Transform collectibles;
    int itemsCount;
    public static ItemManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        collectibles = GameObject.FindGameObjectWithTag("Collectibles").transform;
        itemsCount = collectibles.childCount;
    }

    public void SetItem(Transform room)
    {
        int rnd = UnityEngine.Random.Range(0, itemsCount);

        Transform item = collectibles.GetChild(rnd);
        item.gameObject.SetActive(true);
        item.position = new Vector2(room.position.x, room.position.y +8f);
        item.SetParent(room);
        itemsCount--;
    }
}

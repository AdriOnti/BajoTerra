using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<GameObject>();
    }

    void Update()
    {
        //transform.position = player.transform.position;
    }
}

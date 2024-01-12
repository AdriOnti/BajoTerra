using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    private GameObject player;

    void Start() { player = GameObject.Find("Mage"); }

    void Update() { transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 10f); }
}

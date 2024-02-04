using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    Transform invokersP; 
    Transform stalkersP; 
    Transform turretsP;

    public static EnemiesManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        invokersP = GameObject.FindGameObjectWithTag("Invokers").transform;
        stalkersP = GameObject.FindGameObjectWithTag("Stalkers").transform;
        turretsP = GameObject.FindGameObjectWithTag("Turrets").transform;
    }

    public void SetEnemie(Transform room, int numEnemies)
    {
        int rnd = UnityEngine.Random.Range(0, 6);

        for (int i = 0; i < numEnemies; i++)
        {
            if (rnd == 5)
            {
                foreach (Transform invoker in invokersP)
                {
                    invoker.gameObject.SetActive(true);
                    invoker.position = new Vector3(room.position.x, room.position.y + 9f, room.position.z);
                    invoker.SetParent(room);
                    break;
                }
            }
            if (rnd == 1 || rnd == 2)
            {
                foreach (Transform stalker in stalkersP)
                {
                    stalker.gameObject.SetActive(true);
                    stalker.position = new Vector3(room.position.x, room.position.y + 9f, room.position.z);
                    stalker.SetParent(room);
                    break;
                }
            }
            if (rnd == 3 || rnd == 4)
            {
                foreach (Transform turret in turretsP)
                {
                    turret.gameObject.SetActive(true);
                    turret.position = new Vector3(room.position.x, room.position.y + 9f, room.position.z);
                    turret.SetParent(room);
                    break;
                }
            }
        }
    }
}

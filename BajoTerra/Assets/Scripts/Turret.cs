using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Enemy
{
    public GameObject shotPrefab;
    public float timeBetweenShots = 2.0f;
    public int numberOfShots;
    private Transform[] shots;
    public float visionRange = 10f;

    private GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");

        // Aadimos el numero de disparos que puede hacer y que se vayan desactivando
        shots = new Transform[numberOfShots];
        for (int i = 0; i < numberOfShots; i++)
        {
            shots[i] = Instantiate(shotPrefab).transform;
            shots[i].gameObject.SetActive(false);
            shots[i].transform.SetParent(transform);
        }

        StartCoroutine(ActiveLauncher());
    }

    private void OnDrawGizmosSelected()
    {
        // Dibuja un gizmo en el editor para visualizar el rango de visión
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, visionRange);
    }

    private IEnumerator ActiveLauncher()
    {
        while (true)
        {
            animator.SetBool("isShooting", true);
            yield return new WaitForSeconds(timeBetweenShots);

            // Find a disable shot and active it
            for (int i = 0; i < numberOfShots; i++)
            {
                if (!shots[i].gameObject.activeSelf)
                {
                    shots[i].gameObject.SetActive(true);
                    shots[i].GetComponent<TurretBullet>().target = player.transform;
                    shots[i].GetComponent<TurretBullet>().Disparar();
                    break;
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Enemy
{
    [Header("Settings")]
    public float visionRange = 2f;
    public float rotateSpeed = 30f;
    public float shotSpeed = 25f;

    [Header("TurretBullets Setting")]
    public int projectileQuantity;
    public float bulletLifeTime;
    public float timeBetweenShots;

    // PRIVATE ATTRIBUTES
    //private GameObject player;
    private GameObject projectile;
    private List<Transform> pool;
    private HealthBar healthBar;

    void Start()
    {
        base.animator = GetComponent<Animator>();
        base.player = GameObject.FindGameObjectWithTag("Player");
        projectile = GameObject.Find("TurretBullet");

        healthBar = GetComponentInChildren<HealthBar>();

        if (healthBar != null) { healthBar.UpdateBar(currentHp, maxHp); }
        else { Debug.LogError("HealthBar component not found in children."); }

        InstantiatePoolItem();
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, player.transform.position) <= visionRange)
        {
            RotatePlayer();
            StartCoroutine(Shot());
        }
        else animator.SetBool("isShooting", false);

        healthBar.UpdateBar(currentHp, maxHp);
    }

    void InstantiatePoolItem()
    {
        pool = new List<Transform>();

        for (int i = 0; i < projectileQuantity; i++)
        {
            GameObject shot = Instantiate(projectile, transform.position, Quaternion.identity, transform);
            shot.SetActive(false);
            pool.Add(shot.transform);
        }
    }

    void RotatePlayer()
    {
        Vector2 direction = player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotate2Player = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotate2Player, Time.deltaTime * rotateSpeed);
    }

    IEnumerator Shot()
    {
        if (animator.GetBool("isShooting"))
        {
            yield break; // Evitar disparar múltiples balas simultáneamente
        }

        animator.SetBool("isShooting", true);

        foreach (Transform shotTransform in pool)
        {
            if (!shotTransform.gameObject.activeSelf)
            {
                shotTransform.position = transform.position;
                shotTransform.rotation = transform.rotation;
                shotTransform.gameObject.SetActive(true);

                Vector2 direction = player.transform.position - transform.position;

                Rigidbody2D rbShot = shotTransform.GetComponent<Rigidbody2D>();
                rbShot.velocity = direction.normalized * shotSpeed;

                StartCoroutine(DesactivarBala(shotTransform.gameObject, bulletLifeTime));

                // Esperar antes de disparar la siguiente bala
                yield return new WaitForSeconds(timeBetweenShots); // Ajusta según tus necesidades
            }
        }

        // Esperar un tiempo después de disparar todas las balas y luego desactivar la animación
        yield return new WaitForSeconds(bulletLifeTime); // Puedes ajustar esto según tus necesidades
        animator.SetBool("isShooting", false);
    }



    IEnumerator DesactivarBala(GameObject shot, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        shot.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, visionRange);
    }
}

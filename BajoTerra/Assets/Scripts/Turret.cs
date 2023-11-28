using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public float visionRange = 5f;
    public float rotateSpeed = 3f;
    public float shotSpeed = 50f;
    public Transform player;
    public GameObject projectile;
    public int projectileQuantity;

    private List<Transform> pool;

    void Start()
    {
        // Inicializar el pool de balas
        InstantiatePoolItem();
    }

    void Update()
    {
        // Verificar si el jugador está dentro del rango de visión
        if (Vector2.Distance(transform.position, player.position) <= visionRange)
        {
            // Girar hacia el jugador
            RotatePlayer();

            // Disparar al jugador
            StartCoroutine(Shot());
        }
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
        Vector2 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotate2Player = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotate2Player, Time.deltaTime * rotateSpeed);
    }

    IEnumerator Shot()
    {
        // Buscar una bala inactiva en el pool
        Transform shotTransform = pool.Find(b => !b.gameObject.activeSelf);

        // Si encontramos una bala inactiva, la activamos y la disparamos
        if (shotTransform != null)
        {
            shotTransform.position = transform.position;
            shotTransform.rotation = transform.rotation;
            shotTransform.gameObject.SetActive(true);

            // Obtener la dirección hacia el jugador
            Vector2 direction = player.position - transform.position;

            yield return new WaitForSeconds(direction.magnitude);

            // Configurar la velocidad de la bala
            Rigidbody2D rbShot = shotTransform.GetComponent<Rigidbody2D>();
            rbShot.velocity = direction.normalized * shotSpeed;

            // Desactivar la bala después de un tiempo (ajusta según tus necesidades)
            StartCoroutine(DesactivarBala(shotTransform.gameObject, 0.5f));
        }
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

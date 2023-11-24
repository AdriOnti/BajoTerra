using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    public Transform target;
    public float speed = 15f;
    private bool moving = false;
    public float minDistance2Target = 0.1f;

    private void Update()
    {
        if (moving) { MoveToTarget(); }
    }

    public void Disparar()
    {
        transform.position = transform.parent.position;
        moving = true;
    }

    private void MoveToTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        float distance = Vector2.Distance(transform.position, target.position);

        if (distance < minDistance2Target)
        {
            moving = false;
            BackToPool();
        }
    }

    private void BackToPool() { transform.gameObject.SetActive(false); }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            moving = false;
            BackToPool();
        }
    }

}

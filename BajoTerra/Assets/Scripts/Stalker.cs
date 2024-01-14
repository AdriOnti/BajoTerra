using System.Collections;
using UnityEngine;

public class Stalker : Enemy
{
    private float distance;
    private SpriteRenderer sr;
    private HealthBar healthBar;

    [Header("Stalker Settings")]
    public float minDistance;
    public bool iAmSummon;
    public float explosionForce;
    [SerializeField] private GameObject explosionEffect;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        base.animator = GetComponent<Animator>();
        base.player = GameObject.FindGameObjectWithTag("Player");

        healthBar = GetComponentInChildren<HealthBar>();

        if (healthBar != null) { healthBar.UpdateBar(currentHp, maxHp); }
        else { Debug.LogError("HealthBar component not found in children."); }
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x < transform.position.x) sr.flipX = true;
        else sr.flipX = false;

        distance = Vector3.Distance(transform.position, player.transform.position);
        Vector3 direction = player.transform.position - transform.position;
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

        if (speed > 0) animator.SetBool("stalkerRun", true);
        else animator.SetBool("stalkerRun", false);

        if(distance < minDistance && !iAmSummon) Explosion(); 
        else
        {
            if (distance < 3) animator.SetBool("stalkerAttack", true);
            else animator.SetBool("stalkerAttack", false);
        }

        healthBar.UpdateBar(currentHp, maxHp);
        if(currentHp <= 0)
        {
            speed = 0;
            DetectDead("StalkerDeath");
            StartCoroutine(StopAnimation());
        }
    }

    private IEnumerator StopAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.enabled = false;
        yield return new WaitForSeconds(0.01f);
        animator.enabled = true;

        this.gameObject.SetActive(false);
        if(iAmSummon) this.gameObject.GetComponentInParent<Invoker>().summonsActive--;
    }

    public void Explosion()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, minDistance);

        foreach (Collider2D collision in objects)
        { 
            Rigidbody2D rb2D = collision.GetComponent<Rigidbody2D>();
            if(rb2D != null)
            {
                Vector2 direction = collision.transform.position - transform.position;
                float distance = 1 + direction.magnitude;
                float finalForce = explosionForce / distance;
                rb2D.AddForce(direction * finalForce);
                explosionEffect.SetActive(true);
                StartCoroutine(DestroyExplosion());
                StartCoroutine(StopAnimation());
            }
        }
    }

    private IEnumerator DestroyExplosion()
    {
        yield return new WaitForSeconds(2f);
        explosionEffect.SetActive(false);
    }

}

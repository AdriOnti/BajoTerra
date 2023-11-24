using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalker : Enemy
{
    public GameObject player;
    private float distance;
    private SpriteRenderer sr;
    public bool iAmSummon;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        base.animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x < transform.position.x) sr.flipX = true;
        else sr.flipX = false;

        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

        if (speed > 0) animator.SetBool("stalkerRun", true);
        else animator.SetBool("stalkerRun", false);

        if (distance < 3) animator.SetBool("stalkerAttack", true);
        else animator.SetBool("stalkerAttack", false);

        if(hp <= 0)
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

}

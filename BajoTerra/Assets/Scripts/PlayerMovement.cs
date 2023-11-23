using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public int hp;

    private Rigidbody2D rb;
    private Animator animator;

    // Input Variable
    private Vector2 movement;
    private Vector2 attack;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void OnMovement(InputValue value)
    {
        movement = value.Get<Vector2>();
    }

    private void OnAttack(InputValue value)
    {
        attack = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        PlayerAttack();
    }

    private void MovePlayer()
    {
        if (!animator.GetBool("isAttacking")) { rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime); }

        if (movement.x != 0 || movement.y != 0)
        {
            animator.SetFloat("posX", movement.x);
            animator.SetFloat("posY", movement.y);

            animator.SetBool("isWalking", true);
        }
        else { animator.SetBool("isWalking", false); }
        if(hp <= 0) { StartCoroutine(DeadPlayer()); }
    }

    private void PlayerAttack()
    {
        if (attack.x != 0 || attack.y != 0)
        {
            animator.SetFloat("posX", attack.x);
            animator.SetFloat("posY", attack.y);

            animator.SetBool("isAttacking", true);
        }
        else { animator.SetBool("isAttacking", false); }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(HurtPlayer());
        }
    }

    private IEnumerator HurtPlayer()
    {
        if (hp != 0)
        {
            hp--;
            animator.SetBool("isHurt", true);
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            animator.SetBool("isHurt", false);
        }
    }

    private IEnumerator DeadPlayer()
    {
        animator.SetBool("isDead", true);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.enabled = false;
        yield return new WaitForSeconds(0.01f);
        animator.enabled = true;

        this.gameObject.SetActive(false);
    }
}

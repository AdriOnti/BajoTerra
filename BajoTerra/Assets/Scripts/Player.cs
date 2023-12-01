using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : Character
{
    private Rigidbody2D rb;
    private PlayerInputs inputs;

    // Input Variable
    private Vector2 movement;
    private Vector2 attack;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        base.animator = GetComponent<Animator>();
        GameObject.Find("hpText").GetComponent<Text>().text = Convert.ToString(hp);
        inputs = new PlayerInputs();

        //inputs.InGame.Movement.performed += MovePlayer;
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

    public void MovePlayer(/*InputAction.CallbackContext ctx*/)
    {
        //movement = ctx.ReadValue<Vector2>();

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
            animator.SetBool("isWalking", false);
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
            GameObject.Find("hpText").GetComponent<Text>().text = Convert.ToString(hp);
            animator.SetBool("isHurt", true);
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            animator.SetBool("isHurt", false);
        }
    }

    private IEnumerator DeadPlayer()
    {
        animator.SetBool("isDead", true);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        this.gameObject.SetActive(false);
    }
}

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class Player : Character
{
    private Rigidbody2D rb;
    private PlayerInputs inputs;
    public static Player Instance;
    public GameObject melee;

    [Header("Player Stadistics")]
    public Text hpText;
    public Text attackText;


    // Input Variable
    private Vector2 movement;
    private Vector2 attack;

    private void OnEnable() { inputs.Enable(); }
    private void OnDisable() { inputs.Disable(); }

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else if (Instance != this)  { Destroy(gameObject); }

        rb = GetComponent<Rigidbody2D>();
        base.animator = GetComponent<Animator>();
        inputs = new PlayerInputs();

        inputs.InGame.Movement.performed += ReadMove;
        inputs.InGame.Movement.canceled += ReadMove;
        inputs.InGame.Attack.performed += ReadAttack;
        inputs.InGame.Attack.canceled += ReadAttack;

        melee.SetActive(false);
    }

    private void ReadMove(InputAction.CallbackContext ctx)
    {
        movement = ctx.ReadValue<Vector2>();
    }

    private void ReadAttack(InputAction.CallbackContext ctx)
    {
        attack = ctx.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        PlayerMove();
        PlayerAttack();
        ResetUI();
    }

    public void PlayerMove()
    {
        if (!animator.GetBool("isAttacking") && movement.sqrMagnitude > 0)
        {
            rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime);
        }

        if (movement.x != 0 || movement.y != 0)
        {
            animator.SetFloat("posX", movement.x);
            animator.SetFloat("posY", movement.y);

            animator.SetBool("isWalking", true);
        }
        else { animator.SetBool("isWalking", false); }
        if(currentHp <= 0) { StartCoroutine(DeadPlayer()); }
    }

    public void PlayerAttack()
    {
        if (attack.x != 0 || attack.y != 0)
        {
            animator.SetBool("isWalking", false);
            animator.SetFloat("posX", attack.x);
            animator.SetFloat("posY", attack.y);

            animator.SetBool("isAttacking", true);

            melee.SetActive(true);
        }
        else { animator.SetBool("isAttacking", false); melee.SetActive(false); }
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
        if (currentHp != 0)
        {
            currentHp--;
            animator.SetBool("isHurt", true);
            yield return new WaitForSeconds(0.1f);
            animator.SetBool("isHurt", false);
        }
    }

    private IEnumerator DeadPlayer()
    {
        animator.SetBool("isDead", true);
        yield return new WaitForSeconds(2.0f);

        this.gameObject.SetActive(false);
    }

    public void IncreaseMaxHealth(int value) { maxHp += value; }

    public void Cure(int value)
    {
        if(currentHp < maxHp) { currentHp += value; }
        if(currentHp >= maxHp) { currentHp = maxHp; }
    }

    public void IncreaseDamage(int value) {  damage += value; }

    public void ResetUI()
    {
        hpText.text = Convert.ToString($"{currentHp}/{maxHp}");
        attackText.text = Convert.ToString($"{damage}");
    }
}

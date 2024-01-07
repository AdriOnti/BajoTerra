using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class Player : Character
{
    private Rigidbody2D rb;
    private PlayerInputs inputs;
    private List<Transform> pool;
    public static Player Instance;
    private enum Weapon
    {
        Melee,
        Shotgun,
        FlameThrower
    }

    private Weapon actualWeapon;

    [Header("Weapons")]
    public GameObject melee;
    public GameObject bullet;

    [Header("GUI")]
    public Text hpText;
    public Text attackText;
    public Text speedText;
    public Text weaponText;

    [Header("Player Stadistics")]
    public int projectileQuantity;
    public float shotSpeed;
    public float timeBetweenShots;
    public float bulletLifeTime;

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
        actualWeapon = Weapon.Melee;

        InstantiatePoolItem();
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
        DecideWeapon();
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
            if (actualWeapon == Weapon.Melee) { melee.SetActive(true); }
            if (actualWeapon == Weapon.Shotgun) { StartCoroutine(Shot()); }
        }
        else
        { 
            animator.SetBool("isAttacking", false); 
            if(actualWeapon == Weapon.Melee) { melee.SetActive(false); }
        }

        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(HurtPlayer(collision.gameObject));
        }
    }

    private IEnumerator HurtPlayer(GameObject enemy)
    {
        if (currentHp != 0)
        {
            Debug.Log(enemy.GetComponent<Enemy>().damage);
            currentHp -= enemy.GetComponent<Enemy>().damage;
            animator.SetBool("isHurt", true);
            //rb.simulated = false;
            yield return new WaitForSeconds(0.1f);
            //rb.simulated = true;
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
    public void IncreaseSpeed(int value) { speed += value; }

    public void ResetUI()
    {
        hpText.text = Convert.ToString($"{currentHp}/{maxHp}");
        attackText.text = Convert.ToString($"{damage}");
        speedText.text = Convert.ToString($"{speed}");
        weaponText.text = $"Weapon: {actualWeapon}";
    }

    public void DecideWeapon()
    {
        if (Input.GetKey(KeyCode.Alpha1)) { actualWeapon = Weapon.Melee; }
        if (Input.GetKey(KeyCode.Alpha2)) { actualWeapon = Weapon.Shotgun; }
        if (Input.GetKey(KeyCode.Alpha3)) { actualWeapon = Weapon.FlameThrower; }
    }

    private IEnumerator Shot()
    {
        foreach (Transform shotTransform in pool)
        {
            if (!shotTransform.gameObject.activeSelf)
            {
                shotTransform.position = transform.position;
                shotTransform.rotation = transform.rotation;
                shotTransform.gameObject.SetActive(true);

                Vector2 direction = new Vector2(attack.x, attack.y);

                Rigidbody2D rbShot = shotTransform.GetComponent<Rigidbody2D>();
                rbShot.velocity = direction.normalized * shotSpeed;

                StartCoroutine(DesactivarBala(shotTransform.gameObject, bulletLifeTime));

                // Esperar antes de disparar la siguiente bala
                yield return new WaitForSeconds(timeBetweenShots);
            }
        }

        // Esperar un tiempo después de disparar todas las balas y luego desactivar la animación
        yield return new WaitForSeconds(bulletLifeTime);
    }

    IEnumerator DesactivarBala(GameObject shot, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        shot.SetActive(false);
    }

    void InstantiatePoolItem()
    {
        pool = new List<Transform>();

        for (int i = 0; i < projectileQuantity; i++)
        {
            GameObject shot = Instantiate(bullet, transform.position, Quaternion.identity, transform);
            shot.SetActive(false);
            pool.Add(shot.transform);
        }
    }
}

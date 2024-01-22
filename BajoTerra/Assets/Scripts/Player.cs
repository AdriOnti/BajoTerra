using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : Character
{
    // Private stats
    private Rigidbody2D rb;
    private PlayerInputs inputs;
    private List<Transform> pool;
    public static Player Instance;
    public enum Weapon
    {
        Melee,
        Shotgun,
        FlameThrower
    }
    public Weapon actualWeapon;

    [Header("Weapons")]
    public GameObject melee;
    public GameObject bullet;
    public ParticleSystem flameThrower;
    public float CurrentFlameThrowerCooldown;
    public float FlameThrowerCooldown;

    [Header("GUI")]
    private Text hpText;
    private Text attackText;
    private Text speedText;
    private Text weaponText;

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
        // Singleton
        if (Instance == null) { Instance = this; }
        else if (Instance != this)  { Destroy(gameObject); }

        // Obtain rigidbody, animator & PlayerInputs
        rb = GetComponent<Rigidbody2D>();
        base.animator = GetComponent<Animator>();
        inputs = new PlayerInputs();

        // Performed and canceled actions
        inputs.InGame.Movement.performed += ReadMove;
        inputs.InGame.Movement.canceled += ReadMove;
        inputs.InGame.Attack.performed += ReadAttack;
        inputs.InGame.Attack.canceled += ReadAttack;

        // Disable Melee & Instantiate bullet pool
        melee.SetActive(false);
        flameThrower.gameObject.SetActive(false);
        InstantiatePoolItem();

        if(currentHp > maxHp) currentHp = maxHp;

        
    }

    public void GetValues()
    {
        hpText = GameManager.Instance.PlayerStat("health");
        attackText = GameManager.Instance.PlayerStat("attack");
        speedText = GameManager.Instance.PlayerStat("speed");
        weaponText = GameManager.Instance.PlayerStat("weapon");

        ResetUI();   
    }

    private void ReadMove(InputAction.CallbackContext ctx) { movement = ctx.ReadValue<Vector2>(); }

    private void ReadAttack(InputAction.CallbackContext ctx) { attack = ctx.ReadValue<Vector2>(); }

    private void FixedUpdate()
    {
        // Without these two calls, the player will have to hit it several times to be able to move
        PlayerMove();
        PlayerAttack();

        // Change the actual weapon
        DecideWeapon();

        // Reset the HUD
        ResetUI();

        // Get the pause input
        PauseGame();

        if (CurrentFlameThrowerCooldown > 0) CurrentFlameThrowerCooldown -= 0.5f;
    }

    public void PlayerMove()
    {
        // If is not attacking and de movement is greater than zero, we can move the rigidbody of the player
        if (!animator.GetBool("isAttacking") && movement.sqrMagnitude > 0) { rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime); }


        // Say to the animator which animation must use
        if (movement.x != 0 || movement.y != 0)
        {
            animator.SetFloat("posX", movement.x);
            animator.SetFloat("posY", movement.y);

            animator.SetBool("isWalking", true);
        }
        else { animator.SetBool("isWalking", false); }

        // If current hp is equal or less than zero, the player dies
        if(currentHp <= 0) { StartCoroutine(DeadPlayer()); }
    }

    public void PlayerAttack()
    {
        // Say to the animator which animation must use
        if (attack.x != 0 || attack.y != 0)
        {
            animator.SetBool("isWalking", false);
            animator.SetFloat("posX", attack.x);
            animator.SetFloat("posY", attack.y);

            animator.SetBool("isAttacking", true);

            // Depedends of the actual weapon. One weapon need a code, and other need another code...
            if (actualWeapon == Weapon.Melee) { melee.SetActive(true); }
            if (actualWeapon == Weapon.Shotgun) { StartCoroutine(Shot()); }
            if (actualWeapon == Weapon.FlameThrower && CurrentFlameThrowerCooldown == 0) StartCoroutine(LethalFire());
        }
        else
        { 
            animator.SetBool("isAttacking", false); 
            if(actualWeapon == Weapon.Melee) { melee.SetActive(false); }
        }

        
    }

    /// <summary>
    /// Hace falta decirlo, si choca contra un enemigo se activa la corrutina de daño al jugador
    /// </summary>
    /// <param name="collision">Contra lo que ha chocado</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) { StartCoroutine(HurtPlayer(collision.gameObject)); }
    }

    /// <summary>
    /// Corrutina para hacer daño al jugador
    /// </summary>
    /// <param name="enemy">Enemigo contra el que ha chocado</param>
    /// <returns>Es para poder usar el bendito WaitForSeconds ES UNA CORRUTINA</returns>
    public IEnumerator HurtPlayer(GameObject enemy)
    {
        if (currentHp != 0)
        {
            currentHp -= enemy.GetComponent<Enemy>().damage;
            animator.SetBool("isHurt", true);
            yield return new WaitForSeconds(0.1f);
            animator.SetBool("isHurt", false);
        }
    }

    /// <summary>
    /// Corrutina que activa la animacion de muerte para luego destruir al jugador
    /// </summary>
    /// <returns></returns>
    private IEnumerator DeadPlayer()
    {
        animator.SetBool("isDead", true);
        yield return new WaitForSeconds(2.0f);
        this.gameObject.SetActive(false);
        GameManager.Instance.GameOver();
    }

    // PLAYER USE ITEM

    /// <summary>
    /// Increase the maxHP
    /// </summary>
    /// <param name="value">The value of the item</param>
    public void IncreaseMaxHealth(int value) { maxHp += value; }

    /// <summary>
    /// Increase currentHP as long as it does not exceed maxHP
    /// </summary>
    /// <param name="value">The value of the item</param>
    public void Cure(int value)
    {
        if(currentHp < maxHp) { currentHp += value; }
        if(currentHp >= maxHp) { currentHp = maxHp; }
    }

    /// <summary>
    /// Increase damage
    /// </summary>
    /// <param name="value">The value of the item</param>
    public void IncreaseDamage(int value) {  damage += value; }

    /// <summary>
    /// Increase the speed variable
    /// </summary>
    /// <param name="value">The value of the item</param>
    public void IncreaseSpeed(int value) { speed += value; }

    // RESET THE HUD
    public void ResetUI()
    {
        hpText.text = Convert.ToString($"{currentHp}/{maxHp}");
        attackText.text = Convert.ToString($"{damage}");
        speedText.text = Convert.ToString($"{speed}");
        weaponText.text = $"Weapon: {actualWeapon}";
        if (actualWeapon == Weapon.FlameThrower && CurrentFlameThrowerCooldown == 0.0f) weaponText.color = Color.green;
        else if(actualWeapon == Weapon.FlameThrower && CurrentFlameThrowerCooldown > 0.0f)  weaponText.color = Color.red;
    }

    // DECIDE THE ACTUAL WEAPON
    public void DecideWeapon()
    {
        // Se trato de hacer con Input System, pero no se logro.
        if (Input.GetKey(KeyCode.Alpha1)) { actualWeapon = Weapon.Melee; }                      // Arma Melee
        if (Input.GetKey(KeyCode.Alpha2)) { actualWeapon = Weapon.Shotgun; }                    // Arma Shotgun
        if (Input.GetKey(KeyCode.Alpha3)) { actualWeapon = Weapon.FlameThrower; }               // Arma FlameThrower
        if (Input.GetKey(KeyCode.Alpha4)) { maxHp = 999; currentHp = 999; damage = 20; }        // Modo Semi-Dios
        if (Input.GetKey(KeyCode.Alpha5)) { currentHp = 0; }                                    // Vas a morir
    }

    /// <summary>
    /// Corrutina para el disparo del personaje
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Corrutina para desactivar la bala
    /// </summary>
    /// <param name="shot">La bala</param>
    /// <param name="waitTime">El tiempo que debe de esperar</param>
    /// <returns></returns>
    IEnumerator DesactivarBala(GameObject shot, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        shot.SetActive(false);
    }


    /// <summary>
    /// La tipica pool para disparos. Dudo que se necesite explicar algo más
    /// </summary>
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

    /// <summary>
    /// Pausar el juego. Ya sabemos que no usa el Input System, pero no encontrabamos otra forma, perdon
    /// </summary>
    void PauseGame()
    {
        if(Input.GetKey(KeyCode.Escape)) { GameManager.Instance.PauseGame(); }
    }

    /// <summary>
    /// Corrutina que activa el lanzallamas y su cooldown
    /// </summary>
    /// <returns></returns>
    IEnumerator LethalFire()
    {
        flameThrower.gameObject.SetActive(true);
        flameThrower.Play();
        yield return new WaitForSeconds(5.0f);
        flameThrower.gameObject.SetActive(false);
        CurrentFlameThrowerCooldown = FlameThrowerCooldown;
    }
}

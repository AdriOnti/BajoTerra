using System.Collections;
using UnityEngine;

public class Invoker : Enemy
{
    [Header("Summoner Setting")]
    public GameObject summon;
    public float timeBetweenSummon = 2.5f;
    public int maxSummons = 8;
    public int summonsActive;

    private GameObject[] summons;
    public bool isSummoning;
    private HealthBar healthBar;

    [Header("Summon Setting")]
    [SerializeField] private int currentHPSummon;
    [SerializeField] private int maxHPSummon;
    [SerializeField] private float speedSummon;
    [SerializeField] private float minDistanceSummon;
    [SerializeField] private float explosionForceSummon;


    void Start()
    {
        base.animator = GetComponent<Animator>();
        base.animator.Play("InvokerIdle");
        InstantiateSummons();

        healthBar = GetComponentInChildren<HealthBar>();

        if (healthBar != null) { healthBar.UpdateBar(currentHp, maxHp); }
        else { Debug.LogError("HealthBar component not found in children."); }
    }

    private void Update()
    {
        healthBar.UpdateBar(currentHp, maxHp);
        if (currentHp <= 0)
        {
            speed = 0;
            DetectDead("InvokerDeath");
        }

        if (!isSummoning)
        {
            StartCoroutine(StopAnimation());
            StartCoroutine(ActiveSummons());
        }

        if(summonsActive == maxSummons)
        {
            StartCoroutine(StopAnimation());
            animator.SetBool("isSummoning", false);
        }

    }

    void InstantiateSummons()
    {
        summons = new GameObject[maxSummons];

        for (int i = 0; i < maxSummons; i++)
        {
            GameObject nuevoStalker = Instantiate(summon, transform.position, Quaternion.identity);
            nuevoStalker.transform.SetParent(transform);
            nuevoStalker.SetActive(false);
            summons[i] = nuevoStalker;
        }
    }

    IEnumerator ActiveSummons()
    {
        isSummoning = true;

        for (int i = 0; i < maxSummons; i++)
        {
            yield return new WaitForSeconds(timeBetweenSummon);

            if (!summons[i].activeSelf)
            {
                animator.SetBool("isSummoning", true);
                summons[i].transform.position = transform.position;
                summons[i].SetActive(true);

                summons[i].GetComponent<Stalker>().speed = speedSummon;
                summons[i].GetComponent<Stalker>().currentHp = currentHPSummon;
                summons[i].GetComponent<Stalker>().maxHp = maxHPSummon;
                summons[i].GetComponent<Stalker>().minDistance = minDistanceSummon;
                summons[i].GetComponent<Stalker>().explosionForce = explosionForceSummon;
                summons[i].GetComponent<Stalker>().iAmSummon = true;
                summonsActive++;
            }
        }

        isSummoning = false;
    }

    private IEnumerator StopAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.enabled = false;
        yield return new WaitForSeconds(0.01f);
        animator.enabled = true;
    }
}

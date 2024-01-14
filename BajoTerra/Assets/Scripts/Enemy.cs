using UnityEngine;

public class Enemy : Character
{
    protected GameObject player;

    private void Start()
    {
        base.animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            currentHp -= Player.Instance.damage;
            if (currentHp <= 0) DetectDead("Dead");
        }
    }

    public override void DetectDead(string animationName) { base.DetectDead(animationName); }
}

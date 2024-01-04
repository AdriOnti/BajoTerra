using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [Header("Drop Item")]
    public GameObject dropItem;

    protected GameObject player;

    private void Start()
    {
        base.animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if(dropItem != null)
        {
            dropItem.transform.position = transform.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "PlayerAttack")
        {
            currentHp -= damage;
        }
    }

    public override void DetectDead(string animationName)
    {
        base.DetectDead(animationName);
        DropItem();
    }

    private void DropItem()
    {
        if(dropItem != null) 
        {
            this.gameObject.SetActive(false);
            dropItem.SetActive(true);
        }
    }
}

using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Character Attributes")]
    public int hp;
    public float speed;
    public int damage;
    protected Animator animator;

    public virtual void DetectDead(string animationName)
    {
        animator.Play(animationName);
    }
}

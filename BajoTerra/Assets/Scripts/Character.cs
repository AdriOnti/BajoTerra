using UnityEngine;

public class Character : MonoBehaviour
{
    public int hp;
    public float speed;
    protected Animator animator;

    public virtual void DetectDead(string animationName)
    {
        animator.Play(animationName);
    }
}

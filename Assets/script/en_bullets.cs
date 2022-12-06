using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_bullets : MonoBehaviour
{
    public int dmg;

    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "border")
        {
            animator.SetTrigger("is_hit");
            Invoke("destroy_deley", 0.1f);
        }
        if (collision.gameObject.tag == "Player")
        {
            animator.SetTrigger("is_hit");
            Invoke("destroy_deley", 0.1f);

        }
        if (collision.gameObject.tag == "bottom")
        {
            animator.SetTrigger("is_hit");
            Invoke("destroy_deley", 0.1f);

        }
        if (collision.gameObject.tag == "barricade")
        {
            animator.SetTrigger("is_hit");
            Invoke("destroy_deley", 0.1f);

        }
    }
    void destroy_deley()
    {
        gameObject.SetActive(false);
    }
}

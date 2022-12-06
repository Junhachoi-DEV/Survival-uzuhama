using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{
    float h;
    public bool z_down;

    //public Transform weapon_pos;
    SpriteRenderer sprite;
    Animator animator;
    player pl;
    void Awake()
    {
        pl = FindObjectOfType<player>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        if (pl.is_shop)
        {
            return;
        }
        h = Input.GetAxisRaw("Horizontal");
        z_down = Input.GetButtonDown("Fire1");

        if (Input.GetButton("Horizontal"))
        {
            sprite.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }

        if (z_down)
        {
            animator.SetBool("is_shot", true);
            Invoke("shot_delay", 0.3f);
        }

    }
    void shot_delay()
    {
        animator.SetBool("is_shot", false);
    }
}

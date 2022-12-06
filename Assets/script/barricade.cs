using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barricade : MonoBehaviour
{
    public float barri_health;
    int hit_sound_deley;

    player player;
    Animator animator;
    void Start()
    {
        player = FindObjectOfType<player>();
        animator = GetComponent<Animator>();
        hit_sound_deley = 0;
    }

    void Update()
    {
        if (barri_health <= 4)
        {
            animator.SetTrigger("is_hited1");
            if (hit_sound_deley == 0)
            {
                player.play_sounds("audio_barricade_hit");
                hit_sound_deley += 1;
            }
        }
        if (barri_health <= 2)
        {
            animator.SetTrigger("is_hited2");
            if (hit_sound_deley == 1)
            {
                player.play_sounds("audio_barricade_hit");
                hit_sound_deley += 1;
            }
        }
        if (barri_health <= 0)
        {
            player.cur_barricade -= 1;
            if (hit_sound_deley == 2)
            {
                player.play_sounds("audio_barricade_hit");
                hit_sound_deley += 1;
            }
            gameObject.SetActive(false);
            animator.SetTrigger("is_new");
            barri_health = 5;
            hit_sound_deley = 0;

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "boss_en_bullet")
        {
            barri_health -= 1f;
        }
        if (collision.gameObject.tag == "enemy")
        {
            barri_health -= 0.3f;
        }
        if (collision.gameObject.tag == "big_enemy")
        {
            barri_health -= 2f;
        }
        if (collision.gameObject.tag == "enemy_bullet")
        {
            barri_health -= 0.9f;
        }
    }
}

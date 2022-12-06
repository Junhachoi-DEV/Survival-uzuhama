using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class enemy2 : MonoBehaviour
{
    public int cur_en_health;
    public float en_speed;

    public float max_en_shot_deley;
    public float cur_en_shot_deley;
    public float cur_en_move_deley;
    public float max_en_move_deley;


    public bool is_check;
    public bool is_chase = true;
    public bool is_dead;
    public bool is_attack;
    public bool do_attack = false;

    public Transform target;
    public GameObject players;
    public GameObject item_coin;
    public GameObject die;


    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator animator;
    obj_manager obj_m;
    manager manage;
    player player;
    new AudioSource audio;
    void Awake()
    {
        audio = GetComponent<AudioSource>();
        player = FindObjectOfType<player>();
        manage = FindObjectOfType<manager>();
        obj_m = FindObjectOfType<obj_manager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        chase();
        targetting();
    }

    void chase()
    {
        if (do_attack)
        {
            return;
        }
        cur_en_move_deley += Time.deltaTime;
        if (is_chase && !is_dead && players.activeSelf && cur_en_move_deley >max_en_move_deley && !is_attack)
        {
            animator.SetBool("is_run", true);
            if (transform.position.x < target.transform.position.x)
            {
                is_check = true;
            }
            else
            {
                is_check = false;
            }
            Vector2 cur_pos = transform.position;
            Vector2 next_pos = new Vector2(is_check ? 1 : -1, 0) * en_speed * Time.deltaTime;
            spriteRenderer.flipX = !is_check;
            transform.position = cur_pos + next_pos;

        }
        else
        {
            animator.SetBool("is_run", false);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "player_bullet" && !is_dead)
        {
            bullets bullet = collision.GetComponent<bullets>();
            cur_en_health -= bullet.dmg;

            StartCoroutine(on_damage());
        }
        

    }
    void targetting()
    {
        if (!is_dead)
        {
            Debug.DrawRay(rigid.position, Vector2.right * (is_check ? 1.5f : -1.5f), new Color(0, 1, 0));
            RaycastHit2D ray_2d = Physics2D.Raycast(rigid.position, Vector2.right * (is_check ? 1 : -1), 1.5f, LayerMask.GetMask("player", "playerdead", "barricade"));
            if (ray_2d && players.activeSelf)
            {
                do_attack = true;
            }
            if (do_attack)
            {
                cur_en_move_deley = 0;
                animator.SetBool("is_attack", true);
                attack();
                
            }
            else
            {
                is_chase = true;
                cur_en_shot_deley = 0;
                animator.SetBool("is_attack", false);

            }
        }
    }
    void attack()
    {
        is_chase = false;
        is_attack = true;
        cur_en_shot_deley += Time.deltaTime;
        if (cur_en_shot_deley > max_en_shot_deley && is_attack)
        {
            if (is_check)
            {
                StartCoroutine(attacking());
                do_attack = false;
            }
            else
            {
                StartCoroutine(attacking());
                do_attack = false;
            }
            cur_en_shot_deley = 0;
        }
        is_attack = false;
    }
    /*void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(is_check ? pos_r.position : pos_l.position, box_size);
    }*/
    public void boss_die()
    {
        animator.SetTrigger("is_die");
        spriteRenderer.color = Color.gray;
        gameObject.layer = 11;
        is_chase = false;
        is_dead = true;
        Invoke("destroy_this", 2f);
    }
    IEnumerator attacking()
    {
        gameObject.tag = "enemy_melee";
        gameObject.layer = 0;
        audio.Play();
        rigid.AddForce(Vector2.right * (is_check ? 4f : -4f), ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.1f);
        gameObject.tag = "enemy";
        gameObject.layer = 9;
    }
    IEnumerator on_damage()
    {
        spriteRenderer.color = Color.green;
        yield return new WaitForSeconds(0.1f);

        if (cur_en_health > 0)
        {
            //player.play_sounds("audio_hit_en");
            spriteRenderer.color = Color.white;
        }
        else
        {
            gameObject.layer = 11;
            is_chase = false;
            is_dead = true;
            manage.cur_en_cnt += 1;
            GameObject ins_coin = obj_m.Make_obj("item_coin");
            ins_coin.transform.position = transform.position;
            destroy_this();
            GameObject ins_die = obj_m.Make_obj("enemy_22_die");
            ins_die.transform.position = transform.position;
            player.play_sounds("audio_die_en1_2");

        }
    }
    void destroy_this()
    {
        gameObject.SetActive(false);
        spriteRenderer.color = Color.white;

        is_dead = false;
        is_chase = true;
        gameObject.layer = 9;
        cur_en_health = 4;
    }
}

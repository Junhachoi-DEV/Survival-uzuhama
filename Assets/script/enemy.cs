using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class enemy : MonoBehaviour
{
    public int cur_en_health;
    public float en_speed;

    public float max_en_shot_deley;
    public float cur_en_shot_deley;
    public float cur_en_move_deley;
    public float max_en_move_deley;

    public bool is_check;
    public bool is_chase =true;
    public bool is_dead;
    public bool is_attack;


    public Transform target;
    public GameObject en_bullets;
    public GameObject players;
    public GameObject item_coin;
    public GameObject die;


    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator animator;
    obj_manager obj_m;
    manager manage;
    new AudioSource audio;
    player player;
    private void Start()
    {
        obj_m = FindObjectOfType<obj_manager>();

    }
    void Awake()
    {
        player = FindObjectOfType<player>();
        audio = GetComponent<AudioSource>();
        manage = FindObjectOfType<manager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        chase();
        targetting();
    }

    void chase()
    {
        cur_en_move_deley += Time.deltaTime;
        if (is_chase && !is_dead && players.activeSelf && cur_en_move_deley > max_en_move_deley)
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
        if(collision.tag == "player_bullet" && !is_dead)
        {
            bullets bullet = collision.GetComponent<bullets>();
            cur_en_health -= bullet.dmg;

            StartCoroutine(on_damage());
        }
    }
    public void boss_die()
    {
        animator.SetTrigger("is_die");
        spriteRenderer.color = Color.gray;
        gameObject.layer = 11;
        is_chase = false;
        is_dead = true;
        Invoke("destroy_this", 2f);
    }
    void targetting()
    {
        if (!is_dead)
        {
            cur_en_shot_deley += Time.deltaTime;
            Debug.DrawRay(rigid.position, Vector2.right * (is_check ? 3 : -3) , new Color(0, 1, 0));
            RaycastHit2D ray_2d = Physics2D.Raycast(rigid.position, Vector2.right * (is_check ? 3 : -3), 3, LayerMask.GetMask("player", "playerdead", "barricade"));
            if (ray_2d && players.activeSelf)
            {
                cur_en_move_deley = 0;
                attack();

            }
            else
            {
                is_chase = true;
                animator.SetBool("is_attack", false);

            }
        }
    }
    void attack()
    {
        is_chase = false;
        is_attack = true;
        if (cur_en_shot_deley > max_en_shot_deley && is_attack)
        {
            animator.SetBool("is_attack", true);
            GameObject ins_en_bullet = obj_m.Make_obj("enemy_bullets");
            ins_en_bullet.transform.position = transform.position;
            audio.Play();
            Rigidbody2D rigid_en_bullet = ins_en_bullet.GetComponent<Rigidbody2D>();
            
            rigid.AddForce(Vector2.up * 1.5f, ForceMode2D.Impulse);

            if (is_check)
            {
                //오른쪽 발사
                rigid_en_bullet.velocity = (transform.right * 3) + (transform.up * 7);
                cur_en_shot_deley = 0;
            }
            else
            {
                //왼쪽 발사
                rigid_en_bullet.velocity = (transform.right * -3) + (transform.up * 7);
                cur_en_shot_deley = 0;
            }
        }

    }
    IEnumerator on_damage()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        if(cur_en_health > 0)
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
            GameObject ins_die = obj_m.Make_obj("enemy_11_die");
            ins_die.transform.position = transform.position;
            player.play_sounds("audio_die_en1_2");
            if(manage.stage == 5)
            {
                int ran_ran = Random.Range(0, 19);
                if (ran_ran > 17)
                {
                    GameObject ins_mushinegun = obj_m.Make_obj("item_mushiengun");
                    ins_mushinegun.transform.position = transform.position;
                }
            }
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

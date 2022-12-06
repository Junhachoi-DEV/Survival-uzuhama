using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss : MonoBehaviour
{
    public int cur_en_health;
    public float en_speed;

    public float max_en_shot_deley;
    public float cur_en_shot_deley;
    public float cur_en_move_deley;
    public float max_en_move_deley;

    public float cur_attack1_time;
    public float max_attack1_time;

    public float cur_en_spwan_cooltime;
    public float max_en_spwan_cooltime;
    public float cur_en_spwan2_cooltime;
    public float max_en_spwan2_cooltime;


    public bool is_last_stage;

    public bool is_check;
    public bool is_chase = true;
    public bool is_dead;
    public bool is_attack;

    int escape_cnt;
    

    public Transform target;
    public GameObject players;
    public GameObject[] hit_effects;

    public bool left_or_right;
    public Transform left_attack_spot;
    public bool left_attack2_spot_appear;

    public Transform right_attack_spot;
    public bool right_attack2_spot_appear;

    public bool is_boss_disshow;
    public bool do_attack1;
    public Transform attack1_spot;
    public Transform show_spot;

    public AudioClip audio_fire1;
    public AudioClip audio_fire2;
    public AudioClip audio_escape;
    public AudioClip audio_boss_die;


    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator animator;
    obj_manager obj_m;
    new AudioSource audio;
    manager manager;
    private void Start()
    {
        obj_m = FindObjectOfType<obj_manager>();

    }
    void Awake()
    {
        manager = FindObjectOfType<manager>();
        audio = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        escape_cnt = 0;
        
    }
    void Update()
    {
        if (is_last_stage)
        {
            cur_en_spwan_cooltime += Time.deltaTime;
            cur_en_spwan2_cooltime += Time.deltaTime;
            cur_en_shot_deley += Time.deltaTime;
            attack1();
            effect();
            enemy_spwan();
        }
        
    }
    void play_sounds(string action)
    {
        switch (action)
        {
            case "audio_fire1":
                audio.clip = audio_fire1;
                break;
            case "audio_fire2":
                audio.clip = audio_fire2;
                break;
            case "audio_escape":
                audio.clip = audio_escape;
                break;
            case "audio_boss_die":
                audio.clip = audio_boss_die;
                break;

        }
        audio.Play();
    }
    void show_down()
    {
        if (transform.position.y > show_spot.position.y)
        {
            Vector2 cur_pos = transform.position;
            Vector2 next_pos = new Vector2(0, -1) * en_speed * Time.deltaTime;
            transform.position = cur_pos + next_pos;
        }
    }
    void disshow()
    {
        if (is_dead)
        {
            return;
        }
        if (!(cur_attack1_time <= max_attack1_time || is_dead))
        {
            
            if(transform.position.x > show_spot.position.x)
            {
                is_boss_disshow = true;
                animator.SetBool("is_move_right", true);
                animator.SetBool("is_move_left", false);
            }
            else
            {
                is_boss_disshow = false;
                animator.SetBool("is_move_right", false);
                animator.SetBool("is_move_left", true);
            }
            Vector2 cur_pos = transform.position;
            Vector2 next_pos = new Vector2(is_boss_disshow ? 1 : -1, 0) * en_speed * 5 * Time.deltaTime;
            if(escape_cnt == 0)
            {
                play_sounds("audio_escape");
                escape_cnt += 1;
            }
            transform.position = cur_pos + next_pos;
            Invoke("boss_stop", 2f);
            Invoke("escape_deley", 3f);
        }
    }
    void escape_deley()
    {
        escape_cnt = 0;
    }
    void boss_stop()
    {
        transform.position = attack1_spot.position;
        cur_attack1_time = 0;
    }
    void chase()
    {
        cur_en_move_deley += Time.deltaTime;
        
        if (is_chase && !is_dead && players.activeSelf && cur_en_move_deley > max_en_move_deley && !is_attack)
        {
            //animator.SetBool("is_idle", true);
            if (transform.position.x < target.transform.position.x)
            {
                //¿À¸¥ÂÊ
                is_check = true;
                animator.SetBool("is_move_right", true);
                animator.SetBool("is_move_left", false);
            }
            else
            {
                //¿ÞÂÊ
                is_check = false;
                animator.SetBool("is_move_left", true);
                animator.SetBool("is_move_right", false);
            }
            Vector2 cur_pos = transform.position;
            Vector2 next_pos = new Vector2(is_check ? 1 : -1, 0) * en_speed * Time.deltaTime;
            //spriteRenderer.flipX = !is_check;
            transform.position = cur_pos + next_pos;

        }
        else
        {
            animator.SetBool("is_move_left", false);
            animator.SetBool("is_move_right", false);
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
            Debug.DrawRay(rigid.position, Vector2.down * (is_check ? 9f : 9f), new Color(0, 1, 0));
            RaycastHit2D ray_2d = Physics2D.Raycast(rigid.position, Vector2.down * (is_check ? 1 : -1), 9f, LayerMask.GetMask("player", "playerdead"));
            if (ray_2d)
            {
                cur_en_move_deley = 0;
                animator.SetBool("is_move_right", false);
                animator.SetBool("is_move_left", false);

                if(cur_en_health >= 300)
                {
                    attack();
                }
                else
                {
                    attack1_p2();
                }
            }
            else
            {
                is_chase = true;
                animator.SetBool("is_attack_idle", false);

            }
        }
    }
    void attack()
    {
        
        is_chase = false;
        is_attack = true;
        if (cur_en_shot_deley > max_en_shot_deley && is_attack)
        {
            if (is_check)
            {
                StartCoroutine(attacking());
            }
            else
            {
                StartCoroutine(attacking());
            }
            cur_en_shot_deley = 0;
        }
        is_attack = false;
    }
    void attack1_p2()
    {
        if(cur_en_health > 300)
        {
            return;
        }
        int ran_attack = Random.Range(0, 2);
        is_chase = false;
        is_attack = true;
        if (cur_en_shot_deley > max_en_shot_deley && is_attack)
        {
            if (is_check)
            {
                if(ran_attack == 0)
                {
                    StartCoroutine(attacking());
                }
                else
                {
                    StartCoroutine(attacking_2());
                }
            }
            else
            {
                if (ran_attack == 0)
                {
                    StartCoroutine(attacking());
                }
                else
                {
                    StartCoroutine(attacking_2());
                }
            }
            cur_en_shot_deley = 0;
        }
        is_attack = false;
    }
    
    void attack1()
    {
        
        cur_attack1_time += Time.deltaTime;
        if(cur_attack1_time < max_attack1_time)
        {
            do_attack1 = true;
            if (do_attack1)
            {
                show_down();
                chase();
                targetting();
            }
        }
        else
        {
            do_attack1 = false;
            disshow();
        }
        
    }
    
    void enemy_spwan()
    {
        if(cur_en_health <= 300 && cur_en_spwan_cooltime > max_en_spwan_cooltime)
        {
            obj_m.max_spwan_e1 += 10;
            cur_en_spwan_cooltime = 0;
        }
        if (cur_en_health <= 150 && cur_en_spwan2_cooltime > max_en_spwan2_cooltime)
        {
            obj_m.max_spwan_e2 += 5;
            obj_m.max_spwan_e3 += 2;
            cur_en_spwan2_cooltime = 0;
        }
    }
    
    void effect()
    {
        if(cur_en_health >= 300)
        {
            return;
        }
        if(cur_en_health < 300)
        {
            hit_effects[0].SetActive(true);
        }
        else if(cur_en_health < 200)
        {
            hit_effects[1].SetActive(true);
        }
        else if(cur_en_health < 100)
        {
            hit_effects[2].SetActive(true);
        }

    }
    IEnumerator attacking()
    {
        
        animator.SetBool("is_attack_idle", true);
        int round_num = 5;
        play_sounds("audio_fire1");
        for (int index =0; index < round_num; index++)
        {
            GameObject ins_en_bullet_r = obj_m.Make_obj("enemy_bullets2");
            ins_en_bullet_r.transform.position = transform.position + Vector3.right * 2f;
            ins_en_bullet_r.transform.rotation = Quaternion.identity;
            GameObject ins_en_bullet_l = obj_m.Make_obj("enemy_bullets2");
            ins_en_bullet_l.transform.position = transform.position + Vector3.right * -2f;
            ins_en_bullet_l.transform.rotation = Quaternion.identity;


            Rigidbody2D rigid_en_bullet_r = ins_en_bullet_r.GetComponent<Rigidbody2D>();
            Rigidbody2D rigid_en_bullet_l = ins_en_bullet_l.GetComponent<Rigidbody2D>();
            Vector2 dir_vec = new Vector2(Mathf.Cos(Mathf.PI * 2 * index / round_num), -1);

            rigid_en_bullet_r.AddForce(dir_vec.normalized * 5, ForceMode2D.Impulse);
            
            rigid_en_bullet_l.AddForce(dir_vec.normalized * 5, ForceMode2D.Impulse);
            
            Vector3 rot_vec = Vector3.forward * 180 * index / round_num + Vector3.forward * 90;
            rigid_en_bullet_r.transform.Rotate(rot_vec);
            rigid_en_bullet_l.transform.Rotate(rot_vec);
        }
        yield return new WaitForSeconds(0.1f);

    }
    IEnumerator attacking_2()
    {
        animator.SetBool("is_attack_idle", true);
        int round_num = 6;
        for (int index = 0; index < round_num; index++)
        {
            GameObject ins_en_bullet_r = obj_m.Make_obj("enemy_bullets2");
            ins_en_bullet_r.transform.position = transform.position + Vector3.right * 2f;
            ins_en_bullet_r.transform.rotation = Quaternion.identity;
            GameObject ins_en_bullet_l = obj_m.Make_obj("enemy_bullets2");
            ins_en_bullet_l.transform.position = transform.position + Vector3.right * -2f;
            ins_en_bullet_l.transform.rotation = Quaternion.identity;


            Rigidbody2D rigid_en_bullet_r = ins_en_bullet_r.GetComponent<Rigidbody2D>();
            Rigidbody2D rigid_en_bullet_l = ins_en_bullet_l.GetComponent<Rigidbody2D>();
            Vector2 dir_vec = new Vector2(Mathf.Cos(Mathf.PI * 2 * index / round_num), -1);

            rigid_en_bullet_r.AddForce(dir_vec.normalized * 5, ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.2f);
            play_sounds("audio_fire2");

            rigid_en_bullet_l.AddForce(dir_vec.normalized * 5, ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.2f);
            play_sounds("audio_fire2");
            Vector3 rot_vec = Vector3.forward * 180 * index / round_num + Vector3.forward * 90;
            rigid_en_bullet_r.transform.Rotate(rot_vec);
            rigid_en_bullet_l.transform.Rotate(rot_vec);
        }
        yield return new WaitForSeconds(0.1f);
        
    }

    IEnumerator on_damage()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        if (cur_en_health > 0)
        {
            spriteRenderer.color = Color.white;
        }
        else
        {
            spriteRenderer.color = Color.gray;
            gameObject.layer = 11;
            is_chase = false;
            is_dead = true;
            Invoke("destroy_this", 4.5f);
            hit_effects[3].SetActive(true);
            rigid.gravityScale = 1;
            Invoke("boss_die", 3f);
        }
    }
    void destroy_this()
    {
        Invoke("game_end", 3f);
        gameObject.SetActive(false);
        spriteRenderer.color = Color.white;
    }
    void boss_die()
    {
        play_sounds("audio_boss_die");
        spriteRenderer.color = Color.white;
        animator.SetTrigger("is_dead");
    }
    void game_end()
    {
        manager.game_end_scroll();
    }
}

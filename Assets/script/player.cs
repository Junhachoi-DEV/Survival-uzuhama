using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Pool;

public class player : MonoBehaviour
{
    public int life;
    public int coin;
    public float speed;
    public float jump_power;

    public float max_shot_delay;
    public float cur_shot_delay;
    public float cur2_shot_delay;
    public float max2_shot_delay;
    public float cur_granade_delay;
    public float max_granade_delay;

    public int jump_count = 1;
    //public float max_jump_delay;
    //public float cur_jump_delay;

    public float bullet_speed;
    //public int bullet_power;
    //public int max_bullet_power;
    public int cur_shot_ammo;
    public int ammo;
    public int has_granade;
    public int has_barricade;
    public int max_barricade;
    public int cur_barricade;
    public Transform bullet_pos;
    public GameObject bullet;
    public Transform restart_pos;
    public GameObject granade;
    
    public GameObject hand_gun;
    public GameObject mushin_gun;
    public GameObject player_restart_effect;

    public AudioClip audio_jump;
    public AudioClip audio_landing;
    public AudioClip audio_fire;
    public AudioClip audio_fire2;
    public AudioClip audio_change_weapon;
    public AudioClip audio_hit;
    public AudioClip audio_grenate;
    public AudioClip audio_coin;
    public AudioClip audio_items;
    public AudioClip audio_barricade;
    public AudioClip audio_barricade_hit;
    public AudioClip audio_store_in;
    public AudioClip audio_store_out;
    public AudioClip audio_buy;
    public AudioClip audio_not_buy;
    public AudioClip audio_store_heart;
    public AudioClip audio_start_button;
    public AudioClip audio_die_en1_2;
    public AudioClip audio_die_en3;
    public AudioClip audio_hit_en;
    public AudioClip audio_end_wave;
    

    float h;
    float V;
    public bool z_down;
    public bool c_down;
    public bool d_down;
    public bool is_trun;
    public bool is_jump;
    public bool is_dead;
    public bool is_swich_weapon;

    public bool has_item;
    public bool is_granade;

    public bool is_night;
    public bool is_night_ready;
    public bool is_shop;
    public bool is_shop_ready;

    SpriteRenderer sprite;
    Rigidbody2D rigid;
    Animator animator;
    obj_manager obj_m;
    store store;
    manager manager;
    new AudioSource audio;
    boss boss;
    private void Start()
    {
        obj_m = FindObjectOfType<obj_manager>();
    }
    public void Awake()
    {
        //pool = new ObjectPool<bullets>(creat_bullets, on_get_bullet, on_release_bullet, OnDestroy_bullets, maxSize:20);
        audio = GetComponent<AudioSource>();
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        store = FindObjectOfType<store>();
        manager = FindObjectOfType<manager>();
        boss = FindObjectOfType<boss>();
        jump_count = 0;
        player_restart_effect.SetActive(false);
    }

    public void Update()
    {
        get_input();
        if (is_dead)
        {
            return;
        }
        if (is_shop)
        {
            return;
        }
        jump();
        
        fire();
        fire2();
        swich_wea();
        fire_granade();
        use_barricade();
        interaction();
    }
    void FixedUpdate()
    {
        move();
    }
    public void get_input()
    {
        h = Input.GetAxisRaw("Horizontal");
        V = Input.GetAxisRaw("Vertical");
        z_down = Input.GetButtonDown("Fire1");
        c_down = Input.GetButtonDown("Fire2");
        d_down = Input.GetButtonDown("use_barricade");
    }
    public void move()
    {

        if (is_dead)
        {
            return;
        }
        Vector2 cur_pos = transform.position;
        Vector2 next_pos = new Vector2(h, 0) * speed * Time.deltaTime;

        transform.position = cur_pos + next_pos;

        //가로이동
        if (Input.GetButton("Horizontal"))
        {
            sprite.flipX = Input.GetAxisRaw("Horizontal") == -1;

            if (!sprite.flipX)
            {
                is_trun = false;
                hand_gun.transform.localPosition = new Vector3(0.7f, -0.15f, 0);
                mushin_gun.transform.localPosition = new Vector3(0.5f, -0.15f, 0);
            }
            else
            {
                is_trun = true;
                hand_gun.transform.localPosition = new Vector3(-0.7f, -0.15f, 0);
                mushin_gun.transform.localPosition = new Vector3(-0.5f, -0.15f, 0);

            }
        }
        // 애니메이션
        if (Input.GetButton("Vertical"))
        {
            if (V == 1) //위에 쏠떄
            {
                if (is_trun)
                {
                    hand_gun.transform.eulerAngles = new Vector3(0, 0, -90);
                    hand_gun.transform.localPosition = new Vector3(-0.3f, 0.3f, 0);

                    mushin_gun.transform.eulerAngles = new Vector3(0, 0, -90);
                    mushin_gun.transform.localPosition = new Vector3(-0.3f, 0, 0);

                }
                else
                {
                    hand_gun.transform.eulerAngles = new Vector3(0, 0, 90);
                    hand_gun.transform.localPosition = new Vector3(0.3f, 0.3f, 0);

                    mushin_gun.transform.eulerAngles = new Vector3(0, 0, 90);
                    mushin_gun.transform.localPosition = new Vector3(0.3f, 0, 0);

                }
                animator.SetBool("isupshot", true);
            }
            if (is_jump && V == -1) //점프해서 아래를 쏠때
            {
                if (is_trun)
                {
                    hand_gun.transform.eulerAngles = new Vector3(0, 0, 90);
                    hand_gun.transform.localPosition = new Vector3(0, -0.3f, 0);

                    mushin_gun.transform.eulerAngles = new Vector3(0, 0, 90);
                    mushin_gun.transform.localPosition = new Vector3(0.2f, 0, 0);

                }
                else
                {
                    hand_gun.transform.eulerAngles = new Vector3(0, 0, -90);
                    hand_gun.transform.localPosition = new Vector3(0, -0.3f, 0);

                    mushin_gun.transform.eulerAngles = new Vector3(0, 0, -90);
                    mushin_gun.transform.localPosition = new Vector3(-0.2f, 0, 0);

                }
                animator.SetBool("isdownshot", true);
            }
        }
        else // 기본 서있을때
        {
            hand_gun.transform.eulerAngles = new Vector3(0, 0, 0);
            mushin_gun.transform.eulerAngles = new Vector3(0, 0, 0);
            if (is_trun)
            {
                hand_gun.transform.localPosition = new Vector3(-0.7f, -0.15f, 0);
                mushin_gun.transform.localPosition = new Vector3(-0.5f, -0.15f, 0);
            }
            else
            {
                hand_gun.transform.localPosition = new Vector3(0.7f, -0.15f, 0);
                mushin_gun.transform.localPosition = new Vector3(0.5f, -0.15f, 0);
            }
            animator.SetBool("isupshot", false);
            animator.SetBool("isdownshot", false);
        }

        if (h == 0)
        {
            animator.SetBool("isrun", false);
        }
        else
        {
            animator.SetBool("isrun", true);
        }

        if (Input.GetButton("Horizontal") && V == 1)
        {
            animator.SetBool("isuprunshot", true);
            
        }
        else
        {
            animator.SetBool("isuprunshot", false);
        }
    }
    void jump()
    {

        //cur_jump_delay += Time.deltaTime;
        //Debug.DrawRay(rigid.position, Vector2.up * -1.5f, new Color(0, 1, 0));
        //RaycastHit2D ray_2d = Physics2D.Raycast(rigid.position, Vector2.down * 1.5f, 1, LayerMask.GetMask("bottom"));
        //x도 점프다.
        if (is_jump)
        {
            if(jump_count > 0)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    animator.SetBool("dojump", true);
                    play_sounds("audio_jump");
                    rigid.AddForce(Vector2.up * jump_power, ForceMode2D.Impulse);
                    jump_count--;
                }
            }
            
        }
    }

    
    void interaction()
    {
        if (is_night_ready)
        {
            if (Input.GetButtonDown("interaction"))
            {
                manager.stage += 1;
                play_sounds("audio_start_button");
                is_night = true;
            }
        }
        if (is_shop_ready)
        {
            if (Input.GetButtonDown("interaction"))
            {
                play_sounds("audio_store_in");
                is_shop = true;
            }
        }
    }
    
    void fire()
    {
        cur_shot_delay += Time.deltaTime;
        if (has_item)
        {
            return;
        }
        
        if (z_down && cur_shot_delay > max_shot_delay)
        {
            //var 는 변수 라느 뜻 (variable)
            var ins_bullets = obj_m.Make_obj("player_bullets");
            ins_bullets.transform.position = transform.position;
            Rigidbody2D rigid_bullet = ins_bullets.GetComponent<Rigidbody2D>();
            play_sounds("audio_fire");
            if (is_trun)
            {
                rigid_bullet.velocity = transform.right * -50;
                
            }
            else
            {
                rigid_bullet.velocity = transform.right * 50;
            }
            
            if (V == 1)
            {
                rigid_bullet.velocity = transform.up * 50;
                //rigid_bullet.AddForce(Vector2.up * 50,ForceMode2D.Impulse);
                
            }
            if (is_jump && V == -1)
            {
                rigid_bullet.velocity = transform.up * -50;
                //rigid_bullet.AddForce(Vector2.up * -50,ForceMode2D.Impulse);
                
            }
            cur_shot_delay = 0;
        }
    }
    void fire2()
    {
        cur2_shot_delay += Time.deltaTime;
        if (is_swich_weapon)
        {
            return;
        }
        if (ammo <=0)
        {
            has_item = false;
            hand_gun.SetActive(true);
            mushin_gun.SetActive(false);
            ammo = 0;
            return;
        }
        StartCoroutine(fire2_way());
    }
    IEnumerator fire2_way()
    {
        if (z_down && cur2_shot_delay > max2_shot_delay)
        {
            for(int index =0; index < cur_shot_ammo; index++)
            {
                yield return new WaitForSeconds(0.1f);
                var ins_bullets = obj_m.Make_obj("player_bullets");
                play_sounds("audio_fire2");
                ins_bullets.transform.position = transform.position;
                Rigidbody2D rigid_bullet = ins_bullets.GetComponent<Rigidbody2D>();


                if (is_trun)
                {
                    rigid_bullet.velocity = transform.right * -50;
                    rigid_bullet.AddForce(Vector2.up * Random.Range(-4, 4), ForceMode2D.Impulse);
                    //yield return new WaitForSeconds(0.01f);

                }
                else
                {
                    rigid_bullet.velocity = transform.right * 50;
                    rigid_bullet.AddForce(Vector2.up * Random.Range(-4, 4), ForceMode2D.Impulse);
                    //yield return new WaitForSeconds(0.01f);

                }

                if (V == 1)
                {
                    rigid_bullet.velocity = transform.up * 50;
                    rigid_bullet.AddForce(Vector2.right * Random.Range(-4, 4), ForceMode2D.Impulse);
                    //yield return new WaitForSeconds(0.01f);

                }
                if (is_jump && V == -1)
                {
                    rigid_bullet.velocity = transform.up * -50;
                    rigid_bullet.AddForce(Vector2.right * Random.Range(-4, 4), ForceMode2D.Impulse);
                    //yield return new WaitForSeconds(0.01f);
                }
            }
            cur2_shot_delay = 0;
            ammo -= cur_shot_ammo ;
        }

    }
    void swich_wea()
    {
        if (Input.GetButtonDown("swich_weapon"))
        {
            is_swich_weapon = !is_swich_weapon;
            if (is_swich_weapon)
            {
                if (ammo > 0)
                {
                    has_item = false;
                    play_sounds("audio_change_weapon");
                    hand_gun.SetActive(true);
                    mushin_gun.SetActive(false);
                }
            }
            else
            {
                if (ammo > 0)
                {
                    is_swich_weapon = false;
                    has_item = true;
                    play_sounds("audio_change_weapon");
                    hand_gun.SetActive(false);
                    mushin_gun.SetActive(true);
                }
            }
        }
    }
    
    void fire_granade()
    {
        cur_granade_delay += Time.deltaTime;
        if(!is_granade || has_granade <= 0)
        {
            is_granade = false;
            has_granade = 0;
            return;
        }
        if (c_down && cur_granade_delay > max_granade_delay)
        {
            GameObject ins_grandes = obj_m.Make_obj("grenate");
            ins_grandes.transform.position = transform.position;
            Rigidbody2D rigid_grandes = ins_grandes.GetComponent<Rigidbody2D>();
            play_sounds("audio_grenate");
            if (is_trun)
            {
                rigid_grandes.velocity = (transform.right * -7) + (transform.up * 7);
            }
            else
            {
                rigid_grandes.velocity = (transform.right * 7) + (transform.up * 7);
            }
            cur_granade_delay = 0;
            has_granade -= 1;
        }
    }
    void use_barricade()
    {
        if(has_barricade <= 0 || cur_barricade >= max_barricade)
        {
            return;
        }
        if (d_down)
        {
            GameObject ins_barricade = obj_m.Make_obj("barricade");
            ins_barricade.transform.position = transform.position;
            play_sounds("audio_barricade");
            has_barricade -= 1;
            cur_barricade += 1;
        }
    }

    public void play_sounds(string action)
    {
        switch (action)
        {
            case "audio_jump":
                audio.clip = audio_jump;
                break;
            case "audio_landing":
                audio.clip = audio_landing;
                break;
            case "audio_fire":
                audio.clip = audio_fire;
                break;
            case "audio_fire2":
                audio.clip = audio_fire2;
                break;
            case "audio_change_weapon":
                audio.clip = audio_change_weapon;
                break;
            case "audio_hit":
                audio.clip = audio_hit;
                break;
            case "audio_grenate":
                audio.clip = audio_grenate;
                break;
            case "audio_coin":
                audio.clip = audio_coin;
                break;
            case "audio_items":
                audio.clip = audio_items;
                break;
            case "audio_barricade":
                audio.clip = audio_barricade;
                break;
            case "audio_barricade_hit":
                audio.clip = audio_barricade_hit;
                break;
            case "audio_store_in":
                audio.clip = audio_store_in;
                break;
            case "audio_store_out":
                audio.clip = audio_store_out;
                break;
            case "audio_buy":
                audio.clip = audio_buy;
                break;
            case "audio_not_buy":
                audio.clip = audio_not_buy;
                break;
            case "audio_store_heart":
                audio.clip = audio_store_heart;
                break;
            case "audio_start_button":
                audio.clip = audio_start_button;
                break;
            case "audio_die_en1_2":
                audio.clip = audio_die_en1_2;
                break;
            case "audio_die_en3":
                audio.clip = audio_die_en3;
                break;
            case "audio_hit_en":
                audio.clip = audio_hit_en;
                break;
            case "audio_end_wave":
                audio.clip = audio_end_wave;
                break;
        }
        audio.Play();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "bottom")
        {
            is_jump = true;
            jump_count = 1;
            play_sounds("audio_landing");
            animator.SetBool("dojump", false);
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (boss.is_dead == false)
        {

            if (collision.gameObject.tag == "enemy_bullet")
            {
                is_dead = true;
                animator.SetTrigger("isdead");
                play_sounds("audio_hit");
                gameObject.layer = 12;
                Invoke("on_die", 0.5f);
            }
            if (collision.gameObject.tag == "boss_en_bullet")
            {
                is_dead = true;
                animator.SetTrigger("isdead");
                play_sounds("audio_hit");
                gameObject.layer = 12;
                Invoke("on_die", 0.5f);
            }
            if (collision.gameObject.tag == "enemy_melee")
            {
                is_dead = true;
                animator.SetTrigger("isdead");
                play_sounds("audio_hit");
                gameObject.layer = 12;
                Invoke("on_die", 0.5f);
            }
            if (collision.gameObject.tag == "item")
            {
                is_swich_weapon = false;
                has_item = true;
                ammo += 150;
                play_sounds("audio_items");
                hand_gun.SetActive(false);
                mushin_gun.SetActive(true);
                store.cur_item_mushiengun_in_map -= 1;
            }
            if (collision.gameObject.tag == "item_granade")
            {
                is_granade = true;
                play_sounds("audio_items");
                has_granade += 10;
                store.cur_item_grenate_in_map -= 1;
            }
            if (collision.gameObject.tag == "item_barricade")
            {
                has_barricade += 4;
                play_sounds("audio_items");
                store.cur_item_barricade_in_map -= 1;
            }
            if (collision.gameObject.tag == "item_coin")
            {
                play_sounds("audio_coin");
                coin += 120;
            }
            if (collision.gameObject.tag == "store")
            {
                is_shop_ready = true;
            }
            if (collision.gameObject.tag == "wave_start_botton")
            {
                is_night_ready = true;
            }
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "store")
        {
            is_shop_ready = false;
            if (is_shop)
            {
                store.exit_player();
            }
        }
        if (collision.gameObject.tag == "wave_start_botton")
        {
            is_night_ready = false;
        }

    }
    public void on_die()
    {
        if (life > 0)
        {
            gameObject.SetActive(false);
            is_dead = true;
            life -= 1;
            Invoke("on_restart", 1.5f);
        }
        else
        {
            is_dead = true;
            gameObject.SetActive(false);
        }
    }
    public void on_restart()
    {
        if(life == 0)
        {
            return;
        }
        player_restart_effect.SetActive(true);
        gameObject.SetActive(true);
        is_dead = false;
        sprite.color = new Color(1, 1, 1, 0.4f);
        transform.position = restart_pos.position;

        Invoke("on_restart_b", 2f);
    }
    public void on_restart_b()
    {
        sprite.color = new Color(1, 1, 1, 1);
        gameObject.layer = 10;
        player_restart_effect.SetActive(false);

    }
}

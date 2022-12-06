using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class manager : MonoBehaviour
{

    public SpriteRenderer[] backgrounds;
    public GameObject shop;
    public GameObject shop_fake;
    public GameObject shop_night;
    public GameObject start_botton;
    public GameObject start_botton_night;
    public GameObject streed_lemp;
    public GameObject streed_lemp_night;
    public GameObject start_button_effect;
    

    public float change_night_delay;
    public float change_morning_delay;

    public int stage;
    public int max_stage;
    public float play_time;
    public bool is_battle;
    public bool is_battle_over;
    public bool is_game_paused;


    public int[] stage_en_cnt;
    public bool[] stage_n_cut;
    public int cur_en_cnt;

    int morning_sounds_deley;
    int night_sounds_deley;

    public GameObject menu_panel;
    public GameObject ingame_panel;
    public GameObject esc_panel;
    public GameObject gameover_panel;

    public Text stage_text;
    public Text play_time_text;
    public Text all_en_text;

    public Text play_life_text;
    public Text play_coin_text;

    public Text play_ammo_text;
    public Text play_grenate_text;
    public Text play_barricade_text;

    public RectTransform boss_health_group;
    public RectTransform boss_health_bar;

    public AudioClip audio_morning_background;
    public AudioClip audio_night_background;
    public AudioClip audio_night2_background;
    public AudioClip audio_last_background;
    public AudioClip audio_boss_background;


    SpriteRenderer spriteRenderer;
    obj_manager obj_m;
    player player;
    boss boss;
    store store;
    new AudioSource audio;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        store = FindObjectOfType<store>();
        player = FindObjectOfType<player>();
        obj_m = FindObjectOfType<obj_manager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boss = FindObjectOfType<boss>();
        change_morning_delay = 0;
        change_night_delay = 0;
        morning_sounds_deley = 0;
        night_sounds_deley = 0;
    }
    public void game_start()
    {
        SceneManager.LoadScene(0);
    }
    public void game_over()
    {
        gameover_panel.SetActive(true);
        ingame_panel.SetActive(false);
    }
    public void option()
    {
        Debug.LogError("yet");
    }
    public void retry()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }
    public void resume()
    {
        esc_panel.SetActive(false);
        Time.timeScale = 1f;
        is_game_paused = false;
    }

    public void pause()
    {
        esc_panel.SetActive(true);
        Time.timeScale = 0f;
        is_game_paused = true;
    }
    public void play_sounds(string action)
    {
        switch (action)
        {
            case "audio_morning_background":
                audio.clip = audio_morning_background;
                break;
            case "audio_night_background":
                audio.clip = audio_night_background;
                break;
            case "audio_night2_background":
                audio.clip = audio_night2_background;
                break;
            case "audio_last_background":
                audio.clip = audio_last_background;
                break;
            case "audio_boss_background":
                audio.clip = audio_boss_background;
                break;

        }
        audio.Play();
    }
    public void Update()
    {
        if (boss.is_dead)
        {
            return;
        }
        play_time += Time.deltaTime;
        if(player.life == 0)
        {
            game_over();
        }

        if (player.is_shop && !player.is_night)
        {
            shop_open();
        }
        if (Input.GetButtonDown("escape"))
        {
            if (is_game_paused)
            {
                resume();
            }
            else
            {
                pause();
            }
        }
        start_wave();
        end_wave();
        level_stage();
    }
    public void start_wave()
    {
        if (!player.is_night)
        {
            player.is_night = false;
            is_battle = false;
            shop_fake.SetActive(false);
            start_botton.SetActive(true);
            start_botton_night.SetActive(false);
            start_button_effect.SetActive(true);
            if (change_morning_delay > 1f)
            {
                return;
            }
            change_morning_delay += Time.deltaTime * 0.3f; //3√ 

            for (int i = 0; i < 3; i++)
            {
                backgrounds[i].color = new Color(0.2f + change_morning_delay, 0.08f + change_morning_delay, 0.1f + change_morning_delay, 1);
            }
            Invoke("change_morning", 3f);
            if(morning_sounds_deley == 0)
            {
                player.play_sounds("audio_end_wave");
                play_sounds("audio_morning_background");
                morning_sounds_deley += 1;
            }
            
            obj_m.enemy_1 = false;
            obj_m.enemy_2 = false;
            obj_m.enemy_3 = false;
            change_night_delay = 0;
            night_sounds_deley = 0;
        }
        else
        {
            is_battle = true;
            shop.SetActive(false);
            start_botton.SetActive(false);
            start_botton_night.SetActive(true);
            start_button_effect.SetActive(false);
            if (change_night_delay > 1f)
            {
                return;
            }
            shop_fake.SetActive(true);
            change_night_delay += Time.deltaTime * 0.3f; //3√ 

            for (int i = 0; i < 3; i++)
            {
                backgrounds[i].color = new Color(1.3f - change_night_delay, 1.1f - change_night_delay, 1.2f - change_night_delay, 1);
            }
            Invoke("change_night", 3f);
            if(night_sounds_deley==0 && stage != 4 && stage != 5 && stage != 3)
            {
                play_sounds("audio_night2_background");
                night_sounds_deley += 1;
            }
            if (night_sounds_deley == 0 && stage!=4 && stage!=5)
            {
                play_sounds("audio_night_background");
                night_sounds_deley += 1;
            }
            if(night_sounds_deley ==0 && stage != 5)
            {
                play_sounds("audio_last_background");
                night_sounds_deley += 1;
            }
            if(night_sounds_deley==0 && stage == 5)
            {
                play_sounds("audio_boss_background");
            }
            obj_m.enemy_1 = true;
            obj_m.enemy_2 = true;
            obj_m.enemy_3 = true;
            change_morning_delay = 0;
            morning_sounds_deley = 0;
        }
    }
    
    void end_wave()
    {
        if(stage == 5)
        {
            return;
        }
        for(int i=0; i<stage; i++)
        {
            stage_n_cut[i] = true;
            if(!stage_n_cut[i+1] && cur_en_cnt >= stage_en_cnt[i] && !boss.is_last_stage)
            {
                player.is_night = false;
            }
        }


    }
    void level_stage()
    {
        
        if(stage == 0 && !is_battle)
        {
            obj_m.max_spwan_e1 = 15;
        }
        if (stage == 1 && !is_battle)
        {
            obj_m.max_spwan_e1 = 13;
            obj_m.max_spwan_e2 = 22;
        }
        if(stage == 2 && !is_battle)
        {
            obj_m.max_spwan_e1 = 25;
            obj_m.max_spwan_e2 = 30;
            obj_m.max_spwan_e3 = 5;
        }
        if(stage == 3 && !is_battle)
        {
            obj_m.max_spwan_e1 = 35;
            obj_m.max_spwan_e2 = 43;
            obj_m.max_spwan_e3 = 12;
        }
        if(stage == 5)
        {
            boss.is_last_stage = true;
            
            cur_en_cnt = 0;
        }

    }

    void LateUpdate()
    {
        stage_text.text = "WAVE " + stage;

        int hour = (int)(play_time / 3600);
        int min = (int)((play_time - hour * 3600) / 60);
        int seconds = (int)(play_time % 60);
        play_time_text.text = string.Format("{0:00}", hour) + ":" + string.Format("{0:00}", min) + ":" + string.Format("{0:00}", seconds);
        play_coin_text.text = string.Format("{0:00}", player.coin);
        play_life_text.text = "/ " + string.Format("{0:0}", player.life);
        play_ammo_text.text = "/ " + string.Format("{0:0}", player.ammo);
        play_grenate_text.text = "/ " + string.Format("{0:0}", player.has_granade);
        play_barricade_text.text = "/ " + string.Format("{0:0}", player.has_barricade);

        count_enemys();
        if (boss.is_last_stage)
        {
            boss_health_group.anchoredPosition = Vector3.up * 60;
            boss_health_bar.localScale = new Vector2((float)boss.cur_en_health / 400, 1);
        }
    }
    
    void count_enemys()
    {
        for(int i=0; i < stage; i++)
        {

            if (player.is_night)
            {
                all_en_text.text = string.Format("{0:000}", cur_en_cnt) + "/" + string.Format("{0:000}",stage_en_cnt[i]);
            }
            else
            {
                all_en_text.text = "000/000";
                cur_en_cnt = 0;
            }
        }

    }
    void change_night()
    {
        shop_fake.SetActive(false);
        shop_night.SetActive(true);
        streed_lemp.SetActive(false);
        streed_lemp_night.SetActive(true);
    }
    void change_morning()
    {
        shop.SetActive(true);
        shop_fake.SetActive(false);
        shop_night.SetActive(false);
        streed_lemp.SetActive(true);
        streed_lemp_night.SetActive(false);
    }
    void shop_open()
    {
        if (player.is_shop)
        {
            store.player_enter(player);
        }
        else
        {
            store.exit_player();
        }
    }
    public void game_end_scroll()
    {
        SceneManager.LoadScene(2);
    }
    public void exit_game()
    {
        Application.Quit();
    }
}

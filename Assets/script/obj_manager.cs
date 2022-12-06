using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class obj_manager : MonoBehaviour
{
    public Transform[] spwan_point;
    public GameObject[] enemy_obj;
    
    public GameObject barricade_prefab;
    public GameObject item_barricade_prefab;

    public GameObject grenate_prefab;
    public GameObject item_grenate_prefab;
    
    public GameObject item_mushiengun_prefab;
    
    public GameObject item_coin_prefab;


    public GameObject player_bullets_prefab;
    public GameObject player_bullets_effect_prefab;
    public GameObject enemy_bullets_prefab;
    public GameObject enemy_bullets2_prefab;


    public GameObject enemy_1_prefab;
    public GameObject enemy_2_prefab;
    public GameObject enemy_3_prefab;
    public GameObject enemy_1_die_prefab;
    public GameObject enemy_2_die_prefab;
    public GameObject enemy_3_die_prefab;



    public bool enemy_1 = true;
    public bool enemy_2 = true;
    public bool enemy_3 = true;

    public float cur_spwan_delay_e1;
    public float max_spwan_delay_e1;
    public int max_spwan_e1;

    public float cur_spwan_delay_e2;
    public float max_spwan_delay_e2;
    public int max_spwan_e2;

    public float cur_spwan_delay_e3;
    public float max_spwan_delay_e3;
    public int max_spwan_e3;

    public GameObject player;

    player pl;
    enemy en_1;
    enemy2 en_2;
    enemy3 en_3;
    boss boss;

    GameObject[] player_bullets;
    GameObject[] player_bullets_effect;
    GameObject[] enemy_bullets;
    GameObject[] enemy_bullets2;
    GameObject[] grenate;
    GameObject[] item_grenate;
    GameObject[] barricade;
    GameObject[] item_barricade;
    GameObject[] item_mushiengun;
    GameObject[] item_coin;

    GameObject[] enemy_11;
    GameObject[] enemy_22;
    GameObject[] enemy_33;
    GameObject[] enemy_11_die;
    GameObject[] enemy_22_die;
    GameObject[] enemy_33_die;


    GameObject[] target_pool;

    void Awake()
    {
        
        en_1 = GetComponent<enemy>();
        pl = FindObjectOfType<player>();
        boss = FindObjectOfType<boss>();
        player_bullets = new GameObject[10];
        player_bullets_effect = new GameObject[15];
        enemy_bullets = new GameObject[60];
        enemy_bullets2 = new GameObject[100];
        grenate = new GameObject[10];
        item_grenate = new GameObject[11];
        barricade = new GameObject[12];
        item_barricade = new GameObject[11];
        item_mushiengun = new GameObject[11];
        item_coin = new GameObject[100];
        enemy_11 = new GameObject[50];
        enemy_22 = new GameObject[50];
        enemy_33 = new GameObject[30];
        enemy_11_die = new GameObject[50];
        enemy_22_die = new GameObject[50];
        enemy_33_die = new GameObject[30];



        generate();
    }
    void Update()
    {
        if (boss.is_dead)
        {
            ending_all_destroy();
            return;
        }
        logic_spwan_enemys();
    }
    public void logic_spwan_enemys()
    {
        cur_spwan_delay_e1 += Time.deltaTime;
        if (cur_spwan_delay_e1 > max_spwan_delay_e1 && enemy_1 && max_spwan_e1 > 0)
        {
            spwan_enemy_1();
            cur_spwan_delay_e1 = 0;
            max_spwan_e1--;
        }
        cur_spwan_delay_e2 += Time.deltaTime;
        if (cur_spwan_delay_e2 > max_spwan_delay_e2 && enemy_2 && max_spwan_e2 > 0)
        {
            spwan_enemy_2();
            cur_spwan_delay_e2 = 0;
            max_spwan_e2--;

        }
        cur_spwan_delay_e3 += Time.deltaTime;
        if (cur_spwan_delay_e3 > max_spwan_delay_e3 && enemy_3 && max_spwan_e3 > 0)
        {
            spwan_enemy_3();
            cur_spwan_delay_e3 = 0;
            max_spwan_e3--;

        }
    }
    public void generate()
    {
        int ran_point = Random.Range(0, 5);
        
        for (int index = 0; index < grenate.Length; index++)
        {
            grenate[index] = Instantiate(grenate_prefab);
            grenate[index].SetActive(false);
        }
        for (int index = 0; index < barricade.Length; index++)
        {
            barricade[index] = Instantiate(barricade_prefab);
            barricade[index].SetActive(false);
        }
        for (int index = 0; index < player_bullets.Length; index++)
        {
            player_bullets[index] = Instantiate(player_bullets_prefab);
            player_bullets[index].SetActive(false);
        }
        for (int index = 0; index < player_bullets_effect.Length; index++)
        {
            player_bullets_effect[index] = Instantiate(player_bullets_effect_prefab);
            player_bullets_effect[index].SetActive(false);
        }
        for (int index = 0; index < enemy_bullets.Length; index++)
        {
            enemy_bullets[index] = Instantiate(enemy_bullets_prefab);
            enemy_bullets[index].SetActive(false);
        }
        for (int index = 0; index < enemy_bullets2.Length; index++)
        {
            enemy_bullets2[index] = Instantiate(enemy_bullets2_prefab);
            enemy_bullets2[index].SetActive(false);
        }


        for (int index = 0; index < enemy_11.Length; index++)
        {
            enemy_11[index] = Instantiate(enemy_1_prefab , spwan_point[ran_point].position, spwan_point[ran_point].rotation);
            enemy_11[index].SetActive(false);
        }
        for (int index = 0; index < enemy_22.Length; index++)
        {
            enemy_22[index] = Instantiate(enemy_2_prefab, spwan_point[ran_point].position, spwan_point[ran_point].rotation);
            enemy_22[index].SetActive(false);
        }
        for (int index = 0; index < enemy_33.Length; index++)
        {
            enemy_33[index] = Instantiate(enemy_3_prefab, spwan_point[ran_point].position, spwan_point[ran_point].rotation);
            enemy_33[index].SetActive(false);
        }


        for (int index = 0; index < enemy_11_die.Length; index++)
        {
            enemy_11_die[index] = Instantiate(enemy_1_die_prefab);
            enemy_11_die[index].SetActive(false);
        }
        for (int index = 0; index < enemy_22_die.Length; index++)
        {
            enemy_22_die[index] = Instantiate(enemy_2_die_prefab);
            enemy_22_die[index].SetActive(false);
        }
        for (int index = 0; index < enemy_33_die.Length; index++)
        {
            enemy_33_die[index] = Instantiate(enemy_3_die_prefab);
            enemy_33_die[index].SetActive(false);
        }


        for (int index = 0; index < item_barricade.Length; index++)
        {
            item_barricade[index] = Instantiate(item_barricade_prefab);
            item_barricade[index].SetActive(false);
        }
        for (int index = 0; index < item_grenate.Length; index++)
        {
            item_grenate[index] = Instantiate(item_grenate_prefab);
            item_grenate[index].SetActive(false);
        }
        for (int index = 0; index < item_mushiengun.Length; index++)
        {
            item_mushiengun[index] = Instantiate(item_mushiengun_prefab);
            item_mushiengun[index].SetActive(false);
        }
        for (int index = 0; index < item_coin.Length; index++)
        {
            item_coin[index] = Instantiate(item_coin_prefab);
            item_coin[index].SetActive(false);
        }
    }
    public GameObject Make_obj(string type)
    {
        switch (type)
        {
            case "enemy_11":
                target_pool = enemy_11;
                break;
            case "enemy_22":
                target_pool = enemy_22;
                break;
            case "enemy_33":
                target_pool = enemy_33;
                break;

            case "enemy_11_die":
                target_pool = enemy_11_die;
                break;
            case "enemy_22_die":
                target_pool = enemy_22_die;
                break;
            case "enemy_33_die":
                target_pool = enemy_33_die;
                break;

            case "player_bullets":
                target_pool = player_bullets;
                break;
            case "player_bullets_effect":
                target_pool = player_bullets_effect;
                break;
            case "enemy_bullets":
                target_pool = enemy_bullets;
                break;
            case "enemy_bullets2":
                target_pool = enemy_bullets2;
                break;
            case "grenate":
                target_pool = grenate;
                break;
            case "barricade":
                target_pool = barricade;
                break;

            case "item_barricade":
                target_pool = item_barricade;
                break;
            case "item_grenate":
                target_pool = item_grenate;
                break;
            case "item_mushiengun":
                target_pool = item_mushiengun;
                break;
            case "item_coin":
                target_pool = item_coin;
                break;
        }
        for (int index = 0; index < target_pool.Length; index++)
        {
            if (!target_pool[index].activeSelf)
            {
                target_pool[index].SetActive(true);
                return target_pool[index];
            }
        }
        return null;
    }
    public GameObject[] get_pool(string type)
    {
        switch (type)
        {
            case "enemy_11":
                target_pool = enemy_11;
                break;
            case "enemy_22":
                target_pool = enemy_22;
                break;
            case "enemy_33":
                target_pool = enemy_33;
                break;

            case "enemy_11_die":
                target_pool = enemy_11_die;
                break;
            case "enemy_22_die":
                target_pool = enemy_22_die;
                break;
            case "enemy_33_die":
                target_pool = enemy_33_die;
                break;

            case "player_bullets":
                target_pool = player_bullets;
                break;
            case "player_bullets_effect":
                target_pool = player_bullets_effect;
                break;
            case "enemy_bullets":
                target_pool = enemy_bullets;
                break;
            case "enemy_bullets2":
                target_pool = enemy_bullets2;
                break;
            case "grenate":
                target_pool = grenate;
                break;
            case "barricade":
                target_pool = barricade;
                break;

            case "item_barricade":
                target_pool = item_barricade;
                break;
            case "item_grenate":
                target_pool = item_grenate;
                break;
            case "item_mushiengun":
                target_pool = item_mushiengun;
                break;
            case "item_coin":
                target_pool = item_coin;
                break;
        }
        return target_pool;
    }
    
    void spwan_enemy_1()
    {

        GameObject enemies_1 = Make_obj("enemy_11");
        en_1 = enemies_1.GetComponent<enemy>();

        en_1.transform.position = spwan_point[Random.Range(0, 5)].position;
        en_1.target = player.transform;
        en_1.players = player;
    }
    void spwan_enemy_2()
    {
        GameObject enemies_2 = Make_obj("enemy_22");
        en_2 = enemies_2.GetComponent<enemy2>();

        en_2.transform.position = spwan_point[Random.Range(0, 5)].position;

        en_2.target = player.transform;
        en_2.players = player;
    }
    void spwan_enemy_3()
    {
        GameObject enemies_3 = Make_obj("enemy_33");
        en_3 = enemies_3.GetComponent<enemy3>();

        en_3.transform.position = spwan_point[Random.Range(0, 5)].position;

        en_3.target = player.transform;
        en_3.players = player;
    }
    
    void ending_all_destroy()
    {
        if (boss.is_dead)
        {
            for (int index = 0; index < enemy_bullets.Length; index++)
            {
                if (enemy_bullets[index].activeSelf)
                {
                    //Destroy(en_bullets[index]);
                    enemy_bullets[index].SetActive(false);
                }
            }
            for (int index = 0; index < enemy_bullets2.Length; index++)
            {
                if (enemy_bullets2[index].activeSelf)
                {
                    //Destroy(en_bullets[index]);
                    enemy_bullets2[index].SetActive(false);
                }
            }

            for (int index = 0; index < enemy_11.Length; index++)
            {
                if (enemy_11[index].activeSelf)
                {
                    en_1 = enemy_11[index].GetComponent<enemy>();
                    en_1.cur_en_health = 0;
                    en_1.boss_die();
                }
            }
            for (int index = 0; index < enemy_22.Length; index++)
            {
                if (enemy_22[index].activeSelf)
                {
                    en_2 = enemy_22[index].GetComponent<enemy2>();
                    en_2.cur_en_health = 0;
                    en_2.boss_die();
                }
            }
            for (int index = 0; index < enemy_33.Length; index++)
            {
                if (enemy_33[index].activeSelf)
                {
                    en_3 = enemy_33[index].GetComponent<enemy3>();
                    en_3.cur_en_health = 0;
                    en_3.boss_die();
                }
            }
        }
    }
}

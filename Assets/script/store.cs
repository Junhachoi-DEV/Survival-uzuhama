using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class store : MonoBehaviour
{
    public RectTransform ui_group;

    public int[] item_price;
    public Transform item_spot;
    public Text talk_text;
    public string[] talk_data;

    public GameObject[] item_obj;

    public int max_item_mushiengun_in_map;
    public int cur_item_mushiengun_in_map;

    public int max_item_grenate_in_map;
    public int cur_item_grenate_in_map;

    public int max_item_barricade_in_map;
    public int cur_item_barricade_in_map;


    player enter_player;
    obj_manager obj_m;

    private void Start()
    {
        obj_m = FindObjectOfType<obj_manager>();
    }

    public void player_enter(player player)
    {
        
        enter_player = player;
        ui_group.anchoredPosition = Vector2.zero;
    }
    public void exit_player()
    {
        enter_player.play_sounds("audio_store_out");
        enter_player.is_shop = false;
        ui_group.anchoredPosition = Vector2.down * 1000;
    }
    public void item_buy_mushiengun(int i)
    {
        if (cur_item_mushiengun_in_map >= max_item_mushiengun_in_map)
        {
            return;
        }
        int price = item_price[i];
        if(price > enter_player.coin)
        {
            enter_player.play_sounds("audio_not_buy");
            StopCoroutine(talk());
            StartCoroutine(talk());
            return;
        }
        enter_player.coin -= price;
        
        
        item_obj[0] = obj_m.Make_obj("item_mushiengun");
        item_obj[0].transform.position = item_spot.position;
        enter_player.play_sounds("audio_buy");
        cur_item_mushiengun_in_map += 1;

    }
    public void item_buy_grenate(int i)
    {
        if (cur_item_grenate_in_map >= max_item_grenate_in_map)
        {
            return;
        }
        int price = item_price[i];
        if (price > enter_player.coin)
        {
            enter_player.play_sounds("audio_not_buy");
            StopCoroutine(talk());
            StartCoroutine(talk());
            return;
        }
        enter_player.coin -= price;

        
        item_obj[1] = obj_m.Make_obj("item_grenate");
        item_obj[1].transform.position = item_spot.position;
        enter_player.play_sounds("audio_buy");
        cur_item_grenate_in_map += 1;
    }
    public void item_buy_barricade(int i)
    {
        if (cur_item_barricade_in_map >= max_item_barricade_in_map)
        {
            return;
        }
        int price = item_price[i];
        if (price > enter_player.coin)
        {
            enter_player.play_sounds("audio_not_buy");
            StopCoroutine(talk());
            StartCoroutine(talk());
            return;
        }
        enter_player.coin -= price;

        
        item_obj[2] = obj_m.Make_obj("item_barricade");
        item_obj[2].transform.position = item_spot.position;
        enter_player.play_sounds("audio_buy");
        cur_item_barricade_in_map += 1;

    }
    public void add_life(int i)
    {
        int price = item_price[i];
        if (price > enter_player.coin)
        {
            enter_player.play_sounds("audio_not_buy");
            StopCoroutine(talk());
            StartCoroutine(talk());
            return;
        }
        enter_player.life += 1;
        enter_player.play_sounds("audio_store_heart");
        enter_player.coin -= price;
    }
    IEnumerator talk()
    {
        talk_text.text = talk_data[1];
        yield return new WaitForSeconds(2f);
        talk_text.text = talk_data[0];

    }
}

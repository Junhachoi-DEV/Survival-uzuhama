using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Pool;

public class bullets : MonoBehaviour
{
    public int dmg;
    obj_manager obj_m;
    private void Start()
    {
        obj_m = FindObjectOfType<obj_manager>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "border")
        {
            gameObject.SetActive(false);
        }
        if (collision.gameObject.tag == "enemy")
        {
            gameObject.SetActive(false);
            GameObject ins_bullet_effect = obj_m.Make_obj("player_bullets_effect");
            ins_bullet_effect.transform.position = transform.position;
        }
        if(collision.gameObject.tag == "bottom")
        {
            gameObject.SetActive(false);
        }
        if (collision.gameObject.tag == "big_enemy")
        {
            gameObject.SetActive(false);
            GameObject ins_bullet_effect = obj_m.Make_obj("player_bullets_effect");
            ins_bullet_effect.transform.position = transform.position;
        }
        if (collision.gameObject.tag == "enemy_melee")
        {
            gameObject.SetActive(false);
            GameObject ins_bullet_effect = obj_m.Make_obj("player_bullets_effect");
            ins_bullet_effect.transform.position = transform.position;
        }
    }
}

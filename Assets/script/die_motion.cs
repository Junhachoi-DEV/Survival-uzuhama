using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class die_motion : MonoBehaviour
{
    player player;
    SpriteRenderer sprite;
  
    private void Awake()
    {
        player = FindObjectOfType<player>();
        sprite = FindObjectOfType<SpriteRenderer>();
    }

    private void Update()
    {
        if(player.transform.position.x < transform.position.x)
        {
            sprite.flipX = true;
        }
        else
        {
            sprite.flipX = false;
        }
        Invoke("destroy_this", 1f);
    }
    public void destroy_this()
    {
        gameObject.SetActive(false);
    }
}

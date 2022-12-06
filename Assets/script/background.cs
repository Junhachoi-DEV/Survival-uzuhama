using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class background : MonoBehaviour
{
    public float speed;
    public int start;
    public Transform[] sprites;
    public Transform background_pos;
    public Transform background_pos1;
    
    void Update()
    {
        move();
    }
    void move()
    {
        Vector3 cur_pos = transform.position;
        Vector3 next_pos = Vector3.right * speed * Time.deltaTime;
        transform.position = cur_pos + next_pos;

        if (sprites[start].position.x > background_pos.position.x)
        {
            sprites[start].position = background_pos1.position;

            start += 1;
            if(start == 3)
            {
                start = 0;
            }
        }
    }
    

}

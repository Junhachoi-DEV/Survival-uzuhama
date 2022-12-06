using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet_effect : MonoBehaviour
{

    
    void Update()
    {
        Invoke("bullet_destroy", 0.2f);
    }
    void bullet_destroy()
    {
        gameObject.SetActive(false);
    }
}

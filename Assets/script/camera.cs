using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    public Transform target;
    public float speed;

    public Vector2 center;
    public Vector2 size;

    float height;
    float width;

    void Start()
    {
        height = Camera.main.orthographicSize;
        width = height * Screen.width / Screen.height;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, size);
    }
    void LateUpdate()
    {
        // -10f는 z축
        //transform.position = new Vector3(target.position.x, 0, -10f);
        transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * speed);
        //transform.position = new Vector3(transform.position.x, 0, -10f);

        // -lx + center.x =최소값 , lx + center.x =최대값
        float lx = size.x * 0.5f - width;
        float clampx = Mathf.Clamp(transform.position.x, -lx + center.x, lx + center.x);
        
        float ly = size.y * 0.5f - height;
        float clampy = Mathf.Clamp(transform.position.y, -ly + center.y, ly + center.y);

        transform.position = new Vector3(clampx, clampy, -10f);
    }
}

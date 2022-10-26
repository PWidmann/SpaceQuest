using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    private float speed = 30f;
    private float maxLifeTime = 3f;

    void FixedUpdate()
    {
        maxLifeTime -= Time.fixedDeltaTime;

        if (maxLifeTime < 0)
        {
            Destroy(gameObject);
        }

        transform.Translate(0, 0, speed * Time.fixedDeltaTime);
        
    }
}

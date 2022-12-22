using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    private float speed = 60f;
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

    private void OnTriggerEnter(Collider other)
    {
        // If the bullet hits a monster
        if (other.GetComponent<IEnemy>() != null)
        {
            other.gameObject.GetComponent<IEnemy>().TakeDamage(10f);
        }

        Destroy(gameObject);
    }
}

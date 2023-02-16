using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    #region Members
    private float speed = 60f;
    private float maxLifeTime = 3f;
    #endregion

    #region Unity Methods
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
        if (other.GetComponent<SimpleEnemyController>() != null)
        {
            other.gameObject.GetComponent<SimpleEnemyController>().TakeDamage(35f);
        }

        Destroy(gameObject);
    }
    #endregion
}

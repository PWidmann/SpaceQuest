using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    private float speed = 2f;
    public Vector3 direction = new Vector3(0,0,0);

    void Update()
    {
        transform.Translate(direction * Time.deltaTime * speed);
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAttractor : MonoBehaviour
{
    public float gravity = -10f;

    public void Attract(Transform body)
    {
        Vector3 toAttractorDir = (body.position - transform.position).normalized;
        Vector3 bodyUp = body.up;
        Rigidbody rigidBody = body.GetComponent<Rigidbody>();

        body.rotation = Quaternion.FromToRotation(bodyUp, toAttractorDir) * body.rotation;
        rigidBody.AddForce(toAttractorDir * gravity);
    }
}

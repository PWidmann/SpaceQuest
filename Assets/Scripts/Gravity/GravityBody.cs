using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is meant to be placed on entities which are affected by gravity
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{
    #region Members
    GravityAttractor planet;
    Rigidbody rigidBody;
    #endregion

    #region UnityMethods
    private void Awake()
    {
        planet = GameObject.FindGameObjectWithTag("Planet").GetComponent<GravityAttractor>();
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.useGravity = false;
        rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void FixedUpdate()
    {
        if (planet != null)
        {
            planet.Attract(transform);
        }
    }
    #endregion
}

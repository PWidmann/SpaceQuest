using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    
    private List<Rigidbody> rigidbodies = new List<Rigidbody>();
    private List<Collider> rigidbodyColliders = new List<Collider>();

    void Start()
    {
        SearchForAllRagdollParts();
        DisableRigidbodyParts();
    }

    private void SearchForAllRagdollParts()
    {
        Rigidbody[] allRigidbodies;

        allRigidbodies = FindObjectsOfType<Rigidbody>();
        foreach (Rigidbody rigidbody in allRigidbodies)
        {
            if (rigidbody.gameObject.CompareTag("PlayerRig"))
            {
                rigidbodies.Add(rigidbody);
            }
        }

        Collider[] allColliders;

        allColliders = FindObjectsOfType<Collider>();
        foreach (Collider collider in allColliders)
        {
            if (collider.gameObject.CompareTag("PlayerRig"))
            {
                rigidbodyColliders.Add(collider);
            }
        }
    }

    public void EnableRigidbodyParts()
    {
        foreach (Collider coll in rigidbodyColliders)
        {
            coll.enabled = true;
        }

        foreach (Rigidbody rb in rigidbodies)
        {
            rb.useGravity = true;
        }
    }

    public void DisableRigidbodyParts()
    {
        foreach (Collider coll in rigidbodyColliders)
        {
            coll.enabled = false;
        }

        foreach (Rigidbody rb in rigidbodies)
        {
            rb.useGravity = false;
        }
    }
}

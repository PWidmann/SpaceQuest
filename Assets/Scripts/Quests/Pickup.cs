using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public bool questActive = true;

    public void PickUp()
    {
        Destroy(transform.gameObject);

        
    }
}

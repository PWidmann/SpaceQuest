using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text distanceText;

    private GameObject player;


    private void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        try 
        {
            player = GameObject.Find("Player");

        }
        catch
        {
            
        }



        if(player != null)
        distanceText.text = "Distance to world zero: " + Vector3.Distance(player.transform.position, Vector3.zero);
    }
}

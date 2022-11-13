using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance;

    private bool isInMenu = false;

    public bool IsInMenu { get => isInMenu; set => isInMenu = value; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        IsInMenu = false;
    }
}

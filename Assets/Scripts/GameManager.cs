using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{

    private static bool isInMenu = false;
    private static float mouseSensitivity = 3f;

    public static bool IsInMenu { get => isInMenu; set => isInMenu = value; }
    public static float MouseSensitivity { get => mouseSensitivity; set => mouseSensitivity = value; }
}

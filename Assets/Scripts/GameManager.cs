using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{

    private static bool isInMenu = false;
    private static float mouseSensitivity = 3f;

    private static float sfxVolume = 0.5f;
    private static float musicVolume = 0.5f;

    public static bool IsInMenu { get => isInMenu; set => isInMenu = value; }
    public static float MouseSensitivity { get => mouseSensitivity; set => mouseSensitivity = value; }
    public static float SfxVolume { get => sfxVolume; set => sfxVolume = value; }
    public static float MusicVolume { get => musicVolume; set => musicVolume = value; }
}

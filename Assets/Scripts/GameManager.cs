using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{

    private static bool isInMenu = false;
    private static float mouseSensitivity = 3f;

    private static float sfxVolume = 50f;
    private static float musicVolume = 50f;

    private static PlanetType lastPlanet = PlanetType.Poison;


    public static bool IsInMenu { get => isInMenu; set => isInMenu = value; }
    public static float MouseSensitivity { get => mouseSensitivity; set => mouseSensitivity = value; }
    public static float SfxVolume { get => sfxVolume; set => sfxVolume = value; }
    public static float MusicVolume { get => musicVolume; set => musicVolume = value; }

    public static PlanetType LastPlanet { get => lastPlanet; set => lastPlanet = value; }
}

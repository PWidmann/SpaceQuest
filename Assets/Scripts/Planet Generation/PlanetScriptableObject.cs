using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UIElements;

#region Global Enums
public enum PlanetType { GreenWater, GreenJungle, Ice, Desert, Lava, Poison }
public enum PlanetWeather { Clear, Cloudy, Storm, Foggy }
public enum WaterType { Normal, Lava, Poison, None}
#endregion

[CreateAssetMenu(fileName = "New Planet", menuName = "Planet Creaton/PlanetConfiguration")]
public class PlanetScriptableObject : ScriptableObject
{
    public PlanetType PlanetType;
    public Gradient TerrainHeightColor;
    public AnimationCurve TerrainHeightCurve;

    [Space(20)]
    [Header("Settings")]
    public bool exposeBaseSurface;
    public WaterType Watertype;

    [Space(20)]
    [Header("Prefabs To Spawn")]
    public GameObject[] GrassPrefabs;
    [Range(0,1)]
    public float GrassDensity;
    [Space(10)] 
    public GameObject[] StonePrefabs;
    [Range(0, 1)]
    public float StoneDensity;
    [Space(10)] 
    public GameObject[] TreePrefabs;
    [Range(0, 1)]
    public float TreeDensity;
    [Space(10)] 
    public GameObject[] BioMatter;
    [Range(0, 1)]
    public float BioMatterDensity;
}

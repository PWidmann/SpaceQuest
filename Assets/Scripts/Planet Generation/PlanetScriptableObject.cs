using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "New Planet", menuName = "Planet Creaton/PlanetConfiguration")]
public class PlanetScriptableObject : ScriptableObject
{
    #region Members
    public PlanetType PlanetType;
    public Gradient TerrainHeightColor;
    public AnimationCurve TerrainHeightCurve;

    [Header("Settings")]
    public bool exposeBaseSurface;
    [Space(10)]
    [Header("Prefabs To Spawn")]
    public GameObject[] TreePrefabs;
    [Space(10)] 
    public GameObject[] Pickups;
    [Space(10)]
    public GameObject randomEnemy;
    [Space(10)]
    public GameObject POI_enemy;
    [Space(10)]
    public GameObject boss;
    #endregion
}

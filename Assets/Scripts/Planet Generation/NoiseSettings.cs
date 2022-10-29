using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NoiseSettings
{
    public enum FilterType { Simple, Rigid };
    public FilterType filterType;

    public SimpleNoiseSettings simpleNoiseSettings;
    public RigidNoiseSettings rigidNoiseSettings;
    public bool useFirstLayerAsMask = false;
    private bool useWater = true;

    public NoiseSettings()
    {
        simpleNoiseSettings = new SimpleNoiseSettings();
        rigidNoiseSettings = new RigidNoiseSettings();
        filterType = FilterType.Simple;
        
    }
    public class SimpleNoiseSettings
    {
        public float strength = 0.07f;
        [Range(1, 8)]
        public int numLayers = 4;
        public float baseRoughness = 0.8f;
        public float roughness = 2.2f;
        public float persistence = 0.5f;
        public Vector3 centre = new Vector3(0.8f, 0.2f, 0);
        
        
        public float minValue = 0.7f;
        // minvalue 0.3 = no water, 0,7 water
    }

    [System.Serializable]
    public class RigidNoiseSettings : SimpleNoiseSettings
    {
        public float weightMultiplier = 0.8f;
    }


}
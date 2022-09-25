using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShapeSettings
{
    public float planetRadius = 1;
    public bool useFirstLayerAsMask = true;
    public NoiseSettings noiseSettings;

    public ShapeSettings()
    {
        noiseSettings = new NoiseSettings();
    }
}

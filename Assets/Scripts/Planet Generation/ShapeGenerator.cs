using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator
{
    ShapeSettings settings;
    INoiseFilter[] noiseFilters;

    public ShapeGenerator(ShapeSettings _shapeSettings)
    {
        this.settings = _shapeSettings;
        noiseFilters = new INoiseFilter[2];
        
        for (int i = 0; i < 2; i++)
        {
            noiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(settings.noiseSettings);
        }
    
    }

    public Vector3 CalculatePointOnPlanet(Vector3 _pointOnUnitSphere)
    {
        float firstLayerValue = 0;
        float elevation = 0;
    
        if (noiseFilters.Length > 0)
        {
            firstLayerValue = noiseFilters[0].Evaluate(_pointOnUnitSphere);
            elevation = firstLayerValue;
        }
    
        for (int i = 1; i < noiseFilters.Length; i++)
        {
            float mask = (settings.useFirstLayerAsMask) ? firstLayerValue : 1;
            elevation += noiseFilters[i].Evaluate(_pointOnUnitSphere) * mask;

        }
    
        return _pointOnUnitSphere * settings.planetRadius * (1 + elevation);
    }
}
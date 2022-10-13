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

        noiseFilters[0] = NoiseFilterFactory.CreateNoiseFilter(settings.noiseSettingsL1);
        noiseFilters[1] = NoiseFilterFactory.CreateNoiseFilter(settings.noiseSettingsL2);

    }

    public Vector3 CalculatePointOnPlanet(Vector3 _pointOnUnitSphere)
    {
        float firstLayerValue = 0;
        float elevation = 0;
    
        if (noiseFilters.Length > 0)
        {
            firstLayerValue = noiseFilters[0].Evaluate(_pointOnUnitSphere);
            elevation = firstLayerValue;

            float mask = (settings.useFirstLayerAsMask) ? firstLayerValue : 1;
            elevation += noiseFilters[1].Evaluate(_pointOnUnitSphere) * mask;
        }
    
        return _pointOnUnitSphere * settings.planetRadius * (1 + elevation);
    }
}
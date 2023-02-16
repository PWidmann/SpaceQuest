using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator
{
    #region Members
    private ShapeSettings settings;
    private INoiseFilter[] noiseFilters;
    #endregion

    #region Constructor
    public ShapeGenerator(ShapeSettings _shapeSettings)
    {
        settings = _shapeSettings;
        noiseFilters = new INoiseFilter[2];

        noiseFilters[0] = NoiseFilterFactory.CreateNoiseFilter(settings.noiseSettingsL1);
        noiseFilters[1] = NoiseFilterFactory.CreateNoiseFilter(settings.noiseSettingsL2);
    }
    #endregion

    #region Public Methods
    public Vector3 CalculatePointOnPlanet(Vector3 _pointOnUnitSphere, AnimationCurve _animCurve)
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

        float height = 1 + _animCurve.Evaluate(elevation);
        return _pointOnUnitSphere * settings.planetRadius * height;
    }
    #endregion
}
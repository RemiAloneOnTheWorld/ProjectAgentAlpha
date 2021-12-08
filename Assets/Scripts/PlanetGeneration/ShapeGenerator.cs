using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator
{
    ShapeSettings settings;
    INoiseFilter[] noiseFilters;
    public MinMax elevationMinMax;

    public ShapeGenerator(ShapeSettings settings) {
        this.settings = settings;
        this.noiseFilters = new INoiseFilter[settings.noiseLayers.Length];
        this.elevationMinMax = new MinMax();
        for (int i = 0; i < noiseFilters.Length; i++)
        {
            noiseFilters[i] = NoiseFilterFactory.CreatNoiseFilter(settings.noiseLayers[i].noiseSettings);
        }
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere) 
    {
        float firstlayerValue = 0;
        float elevation = 0;

        if(noiseFilters.Length > 0) {
            firstlayerValue = noiseFilters[0].Evaluate(pointOnUnitSphere);
            if(settings.noiseLayers[0].enabled)
                elevation = firstlayerValue;
        }

        for (int i = 1; i < noiseFilters.Length; i++)
        {
            if(settings.noiseLayers[i].enabled) {
                float mask = (settings.noiseLayers[i].useFirstLayerAsMask)? firstlayerValue : 1;
                elevation += noiseFilters[i].Evaluate(pointOnUnitSphere) * mask;
            }
        }

        elevation = settings.planetRadius * (1 + elevation);
        elevationMinMax.AddValue(elevation);
        return pointOnUnitSphere * elevation;
    }
}

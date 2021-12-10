using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ShapeSettings : ScriptableObject
{
    public float planetRadius = 1;

    public noiseLayer[] noiseLayers;
    
    [System.Serializable]
    public class noiseLayer
    {
        public bool enabled;
        public bool useFirstLayerAsMask;
        public NoiseSettings noiseSettings;
    }
}

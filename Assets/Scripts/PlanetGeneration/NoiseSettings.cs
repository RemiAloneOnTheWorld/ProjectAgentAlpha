using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseSettings
{
    public enum FilterType{Simple, Rigid};
    public FilterType filterType;

    public SimpleNoiseSettings simpleNoiseSettings;
    public RigidNoiseSettings rigidNoiseSettings;

    [System.Serializable]
    public class SimpleNoiseSettings {
        public float strength = 1;
        public float roughness = 2;
        public float persistence = .5f;
        public float baseRoughness = 1;
        public float minValue;
        [Range(1,8)]
        public int numLayers = 1;
        public Vector3 centre;
    }

    [System.Serializable]
    public class RigidNoiseSettings : SimpleNoiseSettings {
        public float weightMultiplier = .8f;
    }

}

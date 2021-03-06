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

    public void randomizeRigidSettings() {
        randomizeSimpleSettings();
        rigidNoiseSettings.weightMultiplier = Random.Range(0f,16f);
    }

    public void randomizeSimpleSettings() {
        simpleNoiseSettings.strength = Random.Range(.02f,.1f);
        simpleNoiseSettings.roughness = Random.Range(1.2f,1.8f);
        simpleNoiseSettings.persistence = Random.Range(.8f,1.1f);
        simpleNoiseSettings.baseRoughness = Random.Range(1.1f,1.5f);
        simpleNoiseSettings.numLayers = Random.Range(1,9);
        simpleNoiseSettings.centre = new Vector3(Random.Range(0,265), Random.Range(0,265), Random.Range(0,265));
        simpleNoiseSettings.minValue = Random.Range(2f,2.5f);
    }

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

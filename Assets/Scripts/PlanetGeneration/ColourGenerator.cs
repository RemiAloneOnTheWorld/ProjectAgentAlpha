using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourGenerator
{
    ColourSettings settings;

    public ColourGenerator(ColourSettings settings){
        this.settings = settings;
    }

    public void UpdateElevation(MinMax elevationMinMax) {
        settings.planetMaterial.SetVector("_elevationMinMax", new Vector4(elevationMinMax.min, elevationMinMax.max));
    }
}

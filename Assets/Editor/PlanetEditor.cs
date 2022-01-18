using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{
    Planet planet;
    Editor shapeEditor;
    Editor colourEditor;

    private void OnEnable() {
        planet = (Planet) target;
    }

    public override void OnInspectorGUI()
    {
        using(var check = new EditorGUI.ChangeCheckScope()){
            base.OnInspectorGUI();
            if(check.changed) {
                planet.GeneratePlanet();
            }
        }

        if(GUILayout.Button("Generate Planet")) {
            planet.GeneratePlanet();
        }

        if(GUILayout.Button("Random Planet")) {
            planet.RandomizePlanet();
            planet.GeneratePlanet();
        }

        DrawSettingsEditor(planet.shapeSettings , planet.OnShapeSettingsUpdated, ref planet.shapeSettingsFoldOut, ref shapeEditor);
        DrawSettingsEditor(planet.colourSettings, planet.OnColourSettingsUpdated, ref planet.colourSettingsFoldOut, ref colourEditor);
    }

    void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool foldOut, ref Editor editor) {
        if(settings !=null) {
            foldOut = EditorGUILayout.InspectorTitlebar(foldOut, settings);
            using(var check = new EditorGUI.ChangeCheckScope())
            {
                if(foldOut) {
                    CreateCachedEditor(settings, null, ref editor);
                    editor.OnInspectorGUI();

                    if(check.changed) {
                        if(onSettingsUpdated != null) {
                            onSettingsUpdated();
                        }
                    }
                }
            }
        }
    }
}

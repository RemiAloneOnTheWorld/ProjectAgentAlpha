 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField, Range(2,256)]
    private int resolution = 10;
    [SerializeField]
    private bool autoUpdate = true;
    public enum FaceRenderMask {All, Top, Bottom, Left, Right, Front, Back}
    public FaceRenderMask faceRenderMask;
    public ShapeSettings shapeSettings;
    public ColourSettings colourSettings;
    [HideInInspector]
    public bool shapeSettingsFoldOut;
    [HideInInspector]
    public bool colourSettingsFoldOut;
    private TerrainFace[] terrainFaces;
    private ShapeGenerator shapeGenerator = new ShapeGenerator();
    private ColourGenerator colourGenerator = new ColourGenerator();
    [SerializeField, HideInInspector]
    private MeshFilter[] meshFilters;

    private void Start() {
        GeneratePlanet();
    }

    void Initialize() {
        shapeGenerator.UpdateSettings(shapeSettings);
        colourGenerator.UpdateSettings(colourSettings);

        if(meshFilters == null || meshFilters.Length == 0)
            meshFilters = new MeshFilter[6];
        
        terrainFaces = new TerrainFace[6];
        Vector3[] directions = {Vector3.up, Vector3.down, Vector3.forward,Vector3.back, Vector3.left, Vector3.right};

        for (int i = 0; i < 6; i++)
        {
            if(meshFilters[i] == null) {
            GameObject meshObj = new GameObject("mesh");
            meshObj.transform.parent = transform;

            meshObj.AddComponent<MeshRenderer>();
            meshFilters[i] = meshObj.AddComponent<MeshFilter>();
            meshFilters[i].sharedMesh = new Mesh();
        }
        meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colourSettings.planetMaterial;

        terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);
        bool renderFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
        meshFilters[i].gameObject.SetActive(renderFace);
        }
    }

    public void GeneratePlanet() {
        Initialize();
        GenerateMesh();
        GenerateColours();
    }

    public void OnShapeSettingsUpdated() {
        if(autoUpdate) {
            Initialize();
            GenerateMesh();
        }
    }

    public void OnColourSettingsUpdated() {
        if(autoUpdate) {
            Initialize();
            GenerateColours();
        } 

    }

    void GenerateMesh() {
        for (int i = 0; i < 6; i++)
        {
            if(meshFilters[i].gameObject.activeSelf)
                terrainFaces[i].ConstructMesh();
        }

        colourGenerator.UpdateElevation(shapeGenerator.elevationMinMax);
    }

    void GenerateColours() {
        colourGenerator.UpdateColours();
        for (int i = 0; i < 6; i++)
        {
            if(meshFilters[i].gameObject.activeSelf)
                terrainFaces[i].UpdateUVs(colourGenerator);
        }
    }
}

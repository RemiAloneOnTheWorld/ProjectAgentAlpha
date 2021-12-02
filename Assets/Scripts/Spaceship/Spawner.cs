using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public enum GizmoType { Never, SelectedOnly, Always }
    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private Vector3 size;

    [SerializeField]
    private int minSpawnCount = 0;
    [SerializeField]
    private int maxSpawnCount = 0;
    [SerializeField]
    private Color colour;
    public GizmoType showSpawnRegion;

    public int crewCreationModules;

    private List<GameObject> cubeList;

    private int totalCubes;

    private void Start()
    {
        setSpawnerBasedOnModules();
        cubeList = new List<GameObject>();
        SpawnCubes();
    }
    
    private void setSpawnerBasedOnModules()
    {
        if (crewCreationModules > 1)
        {
            minSpawnCount = crewCreationModules - 1;
            maxSpawnCount = crewCreationModules;

        } else
        {
            minSpawnCount = 1;
            maxSpawnCount = 1;
        }
        
    }

    public void resetArea()
    {
        RemoveCubes();
        SpawnCubes();
    }

    private void SpawnCubes()
    {
        totalCubes = Random.Range(minSpawnCount, maxSpawnCount);
        for (int i = 0; i < totalCubes; i++)
        {
            float randX = Random.Range(size.x / 2 * -1 + transform.position.x, size.x / 2 + transform.position.x);
            float randY = Random.Range(size.y / 2 * -1 + transform.position.y, size.y / 2 + transform.position.y);
            float randZ = Random.Range(size.z / 2 * -1 + transform.position.z, size.z / 2 + transform.position.z);
            GameObject cube = Instantiate<GameObject>(prefab);
            cube.transform.localPosition = new Vector3(randX, randY, randZ);

            cubeList.Add(cube);
        }
    }

    private void RemoveCubes()
    {
        if (cubeList != null)
        {
            for (int i = 0; i < totalCubes; i++)
            {
                if (cubeList[i] != null)
                {
                    Destroy(cubeList[i]);
                }
            }
        }
        cubeList = new List<GameObject>();
    }

    private void OnDrawGizmos()
    {
        if (showSpawnRegion == GizmoType.Always)
        {
            DrawGizmos();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (showSpawnRegion == GizmoType.SelectedOnly)
        {
            DrawGizmos();
        }
    }

    private void DrawGizmos()
    {
        Gizmos.color = new Color(colour.r, colour.g, colour.b, 0.3f);
        Gizmos.DrawCube(transform.position, size);
    }
}
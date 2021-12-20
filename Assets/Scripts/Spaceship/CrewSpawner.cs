using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewSpawner : MonoBehaviour
{
    public enum GizmoType { Never, SelectedOnly, Always }
    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private float spawnRadius = 10;

    [SerializeField]
    private Color colour;
    public GizmoType showSpawnRegion;

    private List<GameObject> spaceships;

    private int totalCubes;

    private void Start()
    {
        spaceships = new List<GameObject>();
    }


    public void SpawnCube()
    {
        int shipAmount = SpaceshipManager.Spaceships;
        for (int i = 0; i < shipAmount; i++)
        {
            Vector3 pos = transform.position + Random.insideUnitSphere * spawnRadius;
            GameObject spaceship = Instantiate<GameObject>(prefab);
            spaceship.transform.position = pos;
            spaceships.Add(spaceship);            
        }
    }

    public void resetArea()
    {
        RemoveCubes();
    }

    private void RemoveCubes()
    {
        if (spaceships != null)
        {
            for (int i = 0; i < totalCubes; i++)
            {
                if (spaceships[i] != null)
                {
                    Destroy(spaceships[i]);
                }
            }
        }
        spaceships = new List<GameObject>();
    }

    private void OnDrawGizmos() {
        if(showSpawnRegion == GizmoType.Always) {
            DrawGizmos();
        }    
    }

    private void OnDrawGizmosSelected() {
        if(showSpawnRegion == GizmoType.SelectedOnly) {
            DrawGizmos();
        }
    }

    private void DrawGizmos() {
        Gizmos.color = new Color(colour.r, colour.g, colour.b, 0.3f);
        Gizmos.DrawSphere(transform.position, spawnRadius);
    }
}
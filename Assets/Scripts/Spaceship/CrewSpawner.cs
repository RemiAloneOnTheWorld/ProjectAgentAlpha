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
    private int totalShips;

    private void Start()
    {
        spaceships = new List<GameObject>();
        EventQueue.GetEventQueue().Subscribe(EventType.AttackPhase, SpawnShips);
        EventQueue.GetEventQueue().Subscribe(EventType.AttackPhaseOver, RemoveShips);
    }


    public void SpawnShips(EventData eventData)
    {
        SpaceshipManager station = GetComponentInParent<SpaceshipManager>();
        totalShips = station.Spaceships;
        for (int i = 0; i < totalShips; i++)
        {
            Vector3 pos = transform.position + Random.insideUnitSphere * spawnRadius;
            GameObject spaceship = Instantiate<GameObject>(prefab);
            spaceship.transform.position = pos;
            spaceships.Add(spaceship);
        }
    }

    private void RemoveShips(EventData eventData)
    {
        Debug.Log("destroying spaceships");
        if (spaceships != null)
        {
            for (int i = 0; i < totalShips; i++)
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
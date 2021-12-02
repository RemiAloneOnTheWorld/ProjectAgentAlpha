using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewCreationModule : Module
{
    [SerializeField] private float crewRespawnTime;
    private float _timePassed;
    GameObject crewSpawner;

    protected override void Start()
    {
        crewSpawner = GameObject.Find("CrewSpawner");
        base.Start();
    }

    void Update()
    {
        SpawnCrewOverTime();
    }

    private void SpawnCrewOverTime()
    {
        _timePassed += Time.deltaTime;
        if (_timePassed >= crewRespawnTime)
        {
            crewSpawner.GetComponent<Spawner>().SpawnCube();
            ((SpaceshipManager)GetBaseModule()).AddCrew(1);
            _timePassed = 0;
        }
    }
}

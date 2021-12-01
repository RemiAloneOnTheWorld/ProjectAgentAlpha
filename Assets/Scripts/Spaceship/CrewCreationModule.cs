using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewCreationModule : Module
{
    [SerializeField] private float crewRespawnTime;
    private float _timePassed;

    protected override void Start()
    {
        base.Start();
        UpdateSpawner();
    }

    void Update()
    {
        SpawnCrewOverTime();
    }

    private void SpawnCrewOverTime()
    {
        _timePassed = +Time.deltaTime;
        if (_timePassed >= crewRespawnTime)
        {
            //TODO: Call Spawner
            _timePassed = 0;
        }
    }

    private void UpdateSpawner()
    {
       GameObject crewSpawner = GameObject.Find("CrewSpawner");
       crewSpawner.GetComponent<Spawner>().crewCreationModules++;
    }
}

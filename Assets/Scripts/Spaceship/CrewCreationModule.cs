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
            //TODO: Spawn agent
            _timePassed = 0;
        }
    }
}

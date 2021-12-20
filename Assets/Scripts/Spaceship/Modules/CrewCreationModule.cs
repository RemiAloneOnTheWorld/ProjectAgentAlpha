using UnityEngine;

public class CrewCreationModule : Module
{
    [SerializeField] private int spaceships;

    protected override void Start()
    {
        ((SpaceshipManager)GetBaseModule()).AddSpaceships(spaceships);
    }

    public override void DestroyModule()
    {
        ((SpaceshipManager)GetBaseModule()).RemoveSpaceships(spaceships);
        base.DestroyModule();
    }
}

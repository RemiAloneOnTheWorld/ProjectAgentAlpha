using UnityEngine;

public class SpaceshipManager : Module {
    [SerializeField] private int spaceships;
    public int Spaceships { get; private set; }
    public float Money { get; private set; }

    protected override void Start() {
        base.Start();
        Spaceships = spaceships;
    }

    public void AddMoney(float amount) {
        Money += amount;
    }
}
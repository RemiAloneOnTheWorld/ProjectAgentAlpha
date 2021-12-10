using System;
using UnityEngine;

public class SpaceshipManager : Module {
    [SerializeField] private int spaceships;
    public int Spaceships { get; private set; }
    [SerializeField] private int startMoney;

    [SerializeField] private UIHandler uiHandler;
    [SerializeField] private bool orbit;
    [SerializeField] private float orbitSpeed;
    
    private float _x, _z;
    private GameObject _planet;

    public float Money { get; set; }
    
    protected override void Start() {
        base.Start();
        Spaceships = spaceships;
        Money = startMoney;

        //Set UI to values
        uiHandler.SetCurrencyTextValue(Money);
        uiHandler.SetSpaceshipTextValue(Spaceships);

        if (gameObject.CompareTag("BaseStation_2")) {
            _z = Mathf.PI;
            _x = Mathf.PI;
        }

        _planet = GameObject.FindWithTag("Planet");
    }

    private void Update() {
        if (orbit) {
            Orbit();
        }
    }

    public void AddMoney(float amount) {
        Money += amount;
        uiHandler.SetCurrencyTextValue(Money);
    }

    public void AddCrew(int amount) {
        Spaceships += amount;
        uiHandler.SetSpaceshipTextValue(Spaceships);
    }

    private void Orbit() {
        transform.position = new Vector3(Mathf.Cos(_x), 0, Mathf.Sin(_z)) * 150;
        _x += Time.deltaTime * orbitSpeed;
        _z += Time.deltaTime * orbitSpeed;
        transform.LookAt(_planet.transform);
    }
}
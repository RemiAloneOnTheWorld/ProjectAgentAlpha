using System.Globalization;
using TMPro;
using UnityEngine;

public class SpaceshipManager : Module {
    [SerializeField] private int spaceships;
    public int Spaceships { get; private set; }
    [SerializeField] private int startMoney;

    [SerializeField] private UIHandler uiHandler;

    public float Money { get; set; }
    
    protected override void Start() {
        base.Start();
        Spaceships = spaceships;
        Money = startMoney;

        //Set UI to values
        uiHandler.SetCurrencyTextValue(Money);
        uiHandler.SetSpaceshipTextValue(Spaceships);
    }

    public void AddMoney(float amount) {
        Money += amount;
        uiHandler.SetCurrencyTextValue(Money);
    }

    public void AddCrew(int amount) {
        Spaceships += amount;
        uiHandler.SetSpaceshipTextValue(Spaceships);
    }
}
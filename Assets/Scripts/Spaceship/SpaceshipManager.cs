using TMPro;
using UnityEngine;

public class SpaceshipManager : Module {
    [SerializeField] private int spaceships;
    public int Spaceships { get; private set; }
    public float Money { get; private set; }
    
    //UI
    [SerializeField] private TMP_Text currencyText;
    [SerializeField] private TMP_Text factoryText;

    protected override void Start() {
        base.Start();
        Spaceships = spaceships;
        currencyText.text = $"Currency: {Money.ToString()}";
        factoryText.text = $"Spaceships: {Spaceships.ToString()}";
    }

    public void AddMoney(float amount) {
        Money += amount;
        currencyText.text = $"Currency: {Money}";
    }
}
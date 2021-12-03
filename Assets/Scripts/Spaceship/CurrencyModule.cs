using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyModule : Module {
    [SerializeField] private float moneyPerSecond;
    private float _timePassed;

    protected override void Start() {
        base.Start();
        //TODO: Overwrite for currency module.
    }

    private void Update() {
        AddCurrencyOverTime();
    }

    private void AddCurrencyOverTime() {
        _timePassed += Time.deltaTime;
        if (_timePassed >= 1) {
            ((SpaceshipManager) GetBaseModule()).AddMoney(moneyPerSecond);
            _timePassed = 0;
        }
    }
}

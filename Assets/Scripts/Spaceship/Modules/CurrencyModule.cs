using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyModule : Module {
    [SerializeField] private float moneyPerSecond;
    [SerializeField] private bool testMoneyPerSecond;
    private float _timePassed;

    protected override void Start() {
        base.Start();
        if(!testMoneyPerSecond)
            EventQueue.GetEventQueue().Subscribe(EventType.PreparationPhaseOver, updateCurrency);
        //TODO: Overwrite for currency module.
    }

    private void Update() {
        if(testMoneyPerSecond)
            AddCurrencyOverTime();
    }

    private void updateCurrency(EventData eventData) {
        ((SpaceshipManager) GetBaseModule()).AddMoney(moneyPerSecond);	        
    }

    private void AddCurrencyOverTime() {
        _timePassed += Time.deltaTime;
        if (_timePassed >= 1) {
            ((SpaceshipManager) GetBaseModule()).AddMoney(moneyPerSecond);
            _timePassed = 0;
        }
    }
}

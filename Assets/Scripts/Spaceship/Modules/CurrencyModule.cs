using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyModule : Module {
    [SerializeField] private float currencyGenerated;
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
        ((SpaceshipManager) GetBaseModule()).AddMoney(currencyGenerated);	        
    }

    private void AddCurrencyOverTime() {
        _timePassed += Time.deltaTime;
        if (_timePassed >= 1) {
            ((SpaceshipManager) GetBaseModule()).AddMoney(currencyGenerated);
            _timePassed = 0;
        }
    }
}

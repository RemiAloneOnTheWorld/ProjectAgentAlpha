using System.Collections;
using UnityEngine;

public class PhaseGameManager : MonoBehaviour {
    [SerializeField] private float preparationTime;
    [SerializeField] private float attackTime;
    private bool _countdownRunning;

    private void Awake() {
        EventQueue.GetEventQueue().Subscribe(EventType.AttackPhase, StartAttackPhase);
    }
    
    void Start() {
        StartCoroutine(StartPrepPhase());
    }

    private IEnumerator StartPrepPhase() {
        //Preparation phase starts
        EventQueue.GetEventQueue().AddEvent(new PhaseUIEventData(EventType.PreparationPhase, "Preparation Phase"));
        StartCoroutine(StartCountdown(preparationTime));
        yield return new WaitWhile(() => _countdownRunning);
        //This event initiates the camera transition and fade
        EventQueue.GetEventQueue().AddEvent(new EventData(EventType.PreparationPhaseOver));
    }

    private void StartAttackPhase(EventData eventData) { 
        StartCoroutine(AttackPhase());
    }

    private IEnumerator AttackPhase() {
        yield return new WaitForEndOfFrame();
        StartCoroutine(StartCountdown(attackTime));
        yield return new WaitWhile(() => _countdownRunning);
        //This event initiates the camera transition and fade
        EventQueue.GetEventQueue().AddEvent(new EventData(EventType.AttackPhaseOver));
    }
    
    private IEnumerator StartCountdown(float time) {
        _countdownRunning = true;
        float timer = time;
        while (timer > 0) {
            timer -= Time.deltaTime;
            EventQueue.GetEventQueue().AddEvent(new PhaseTimeData(EventType.PhaseTimeUpdate, timer));
            yield return null;
        }

        _countdownRunning = false;
    }
}
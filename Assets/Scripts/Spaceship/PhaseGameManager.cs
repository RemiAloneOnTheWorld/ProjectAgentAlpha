using System.Collections;
using UnityEngine;

public class PhaseGameManager : MonoBehaviour {
    [SerializeField] private float preparationTime;
    [SerializeField] private float attackTime;
    private bool _countdownRunning;
    private string _currentPlayerName;
    public EventType EventType { get; private set; }

    private void Awake() {
        EventQueue.GetEventQueue().Subscribe(EventType.AttackPhase, StartAttackPhase);
        EventQueue.GetEventQueue().Subscribe(EventType.PlayerPreparationReady, PlayerIsReady);
    }
    
    void Start() {
        StartPrepPhase();
    }

    private void StartPrepPhase() {
        EventType = EventType.PreparationPhase;
        //Preparation phase starts
        EventQueue.GetEventQueue().AddEvent(new MessageEventData(EventType.InitPreparationPhase, "Ready? Press 'R' or 'North Button'"));
        EventQueue.GetEventQueue().AddEvent(new PhaseUIEventData(EventType.PreparationPhase, "Preparation Phase"));
    }

    private void PlayerIsReady(EventData eventData) {
        _currentPlayerName = ((PreparationReadyEventData) eventData).playerName;
        StartCoroutine(StartPreparationCountdown());
    }

    private IEnumerator StartPreparationCountdown() {
        yield return new WaitForEndOfFrame();
        StartCoroutine(StartCountdown(preparationTime));
        yield return new WaitWhile(() => _countdownRunning);
        //This event initiates the camera transition and fade
        EventQueue.GetEventQueue().AddEvent(new EventData(EventType.PreparationPhaseOver));
    }
    

    private void StartAttackPhase(EventData eventData) { 
        StartCoroutine(AttackPhase());
    }

    private IEnumerator AttackPhase() {
        EventType = EventType.AttackPhase;
        yield return new WaitForEndOfFrame();
        StartCoroutine(StartCountdown(attackTime));
        yield return new WaitWhile(() => _countdownRunning);
        //This event initiates the camera transition and fade
        EventQueue.GetEventQueue().AddEvent(new EventData(EventType.AttackPhaseOver));
        Debug.Log("Start spaceship destruction");
    }
    
    private IEnumerator StartCountdown(float time) {
        _countdownRunning = true;
        float timer = time;
        while (timer > 0) {
            timer -= Time.deltaTime;
            EventQueue.GetEventQueue().AddEvent(new PhaseTimeData(EventType == EventType.PreparationPhase ? 
                EventType.PrepPhaseTimeUpdate : EventType.AttackPhaseTimeUpdate, timer, _currentPlayerName));
            yield return null;
        }

        _countdownRunning = false;
    }
}
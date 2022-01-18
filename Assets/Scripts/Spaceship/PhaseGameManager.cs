using System.Collections;
using UnityEngine;

public class PhaseGameManager : MonoBehaviour {
    [SerializeField] private float preparationTime;
    [SerializeField] private float attackTime;
    [SerializeField] private float destructionTime;
    
    private bool _countdownRunning;
    private string _currentPlayerName;
    public static EventType EventType { get; private set; }

    private IEnumerator _prepCoroutine;
    private IEnumerator _countdownCoroutine;

    private void Awake() {
        EventQueue.GetEventQueue().Subscribe(EventType.AttackPhase, StartAttackPhase);
        
        EventQueue.GetEventQueue().Subscribe(EventType.DestructionPhase, StartDestructionPhase);
        EventQueue.GetEventQueue().Subscribe(EventType.PlayerPreparationReady, PlayerIsReady);
        EventQueue.GetEventQueue().Subscribe(EventType.PlayerDestructionReady, PlayerIsReady);
        EventQueue.GetEventQueue().Subscribe(EventType.DestructionPhaseOver, StartPrepPhase);
    }
    
    /*
     * 1. Start preparation phase by calling 'StartPrepPhase' - this sets text elements and the current phase.
     * 2. Players pressed 'ready' - 'PlayerIsReady' is called by event. Event is raised by UIHandler TODO: Move it from there. 
     *      - Phase text is set
     *      - Countdown gets started
     * 3. Countdown over, calls 'PreparationPhaseOver' event
     * 
     */
    
    void Start() {
        StartPrepPhase(null);
    }

    private void StartPrepPhase(EventData eventData) {
        EventType = EventType.PreparationPhase;
        StartCoroutine(PreparationPhase());
    }

    private IEnumerator PreparationPhase() {
        yield return new WaitForEndOfFrame();
        EventQueue.GetEventQueue().AddEvent(new EventData(EventType.PreparationPhase));
        EventQueue.GetEventQueue().AddEvent(new MessageEventData(EventType.InitPreparationPhase, 
            "Initiate attack? Press 'R' or 'North Button'"));
        EventQueue.GetEventQueue().AddEvent(new PhaseUIEventData(EventType.InFadeToPreparation, "Preparation Phase"));
    }
    
    private void StartDestructionPhase(EventData eventData) {
        EventType = EventType.DestructionPhase;
        StartCoroutine(DestructionPhase());
    }

    private IEnumerator DestructionPhase() {
        EventType = EventType.DestructionPhase;
        yield return new WaitForEndOfFrame();
        EventQueue.GetEventQueue().AddEvent(new PhaseUIEventData(EventType.InFadeToDestruction, 
            "Destruction Phase"));
        EventQueue.GetEventQueue().AddEvent(new MessageEventData(EventType.InitPreparationPhase, 
            "Done attacking? Press 'R' or 'North Button'"));
    }

    private void PlayerIsReady(EventData eventData) {
        PlayerReadyEventData playerReadyEventData = (PlayerReadyEventData) eventData;

        //This lets the player who started the countdown, abort it again.
        if (_countdownRunning) {
            if (_currentPlayerName == playerReadyEventData.playerName) {
                StartCoroutine(AbortReadyCountdown());
            }

            return;
        }
        
        _currentPlayerName = playerReadyEventData.playerName;
        
        _prepCoroutine = StartReadyCountdown(eventData.eventType == EventType.PlayerPreparationReady ?
            EventType.PreparationPhaseOver
            : EventType.DestructionPhaseOver);
        
        
        StartCoroutine(_prepCoroutine);
    }

    private IEnumerator StartReadyCountdown(EventType eventPublishType) {
        yield return new WaitForEndOfFrame();
        _countdownCoroutine = StartCountdown(eventPublishType == EventType.PreparationPhaseOver ? preparationTime :
            destructionTime);
        
        StartCoroutine(_countdownCoroutine);
        yield return new WaitWhile(() => _countdownRunning);
        
        //This event initiates the camera transition and fade
        EventQueue.GetEventQueue().AddEvent(new EventData(eventPublishType));
    }
    
    private void StartAttackPhase(EventData eventData) { 
        StartCoroutine(AttackPhase());
    }

    //Stops all running coroutines and starts preparation phase again.
    private IEnumerator AbortReadyCountdown() {
        StopCoroutine(_countdownCoroutine);
        StopCoroutine(_prepCoroutine);
        yield return null;
        _countdownRunning = false;
        yield return new WaitForEndOfFrame();

        if (EventType == EventType.PreparationPhase) {
            StartPrepPhase(null);
        }
        else if (EventType == EventType.DestructionPhase) {
            StartDestructionPhase(new EventData(EventType.DestructionPhase));
        }
    }

    private IEnumerator AttackPhase() {
        EventType = EventType.AttackPhase;
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

            EventType eventType = EventType.Debug;
            switch (EventType) {
                case EventType.PreparationPhase:
                    eventType = EventType.PrepPhaseTimeUpdate;
                    break;
                case EventType.AttackPhase:
                    eventType = EventType.AttackPhaseTimeUpdate;
                    break;
                case EventType.DestructionPhase:
                    eventType = EventType.DestructionPhaseTimeUpdate;
                    break;
            }
            
            EventQueue.GetEventQueue().AddEvent(new PhaseTimeData(eventType, timer, _currentPlayerName));
            
            
            
            yield return null;
        }

        _countdownRunning = false;
    }
    
    
}
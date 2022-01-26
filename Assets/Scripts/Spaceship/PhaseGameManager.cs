using System.Collections;
using UnityEngine;

public class PhaseGameManager : MonoBehaviour {
    [Header("Phase Times")] [SerializeField]
    private float preparationTime;

    [SerializeField] private float attackTime;
    [SerializeField] private float destructionTime;

    private bool _countdownRunning;
    private string _readyPlayer;

    public static EventType CurrentEventType { get; private set; }

    private IEnumerator _playerReadyCountdown;
    private IEnumerator _countdownCoroutine;

    private void Awake() {
        //Preparation phase
        //EventQueue.GetEventQueue().Subscribe(EventType.InFadeToPreparation, StartPreparationPhase);
        EventQueue.GetEventQueue().Subscribe(EventType.PreparationPhase, StartPreparationPhase);
        EventQueue.GetEventQueue().Subscribe(EventType.PlayerPreparationReady, PlayerIsReady);

        //Attack Phase
        EventQueue.GetEventQueue().Subscribe(EventType.AttackPhase, StartAttackPhase);

        //Destruction Phase
        EventQueue.GetEventQueue().Subscribe(EventType.DestructionPhase, StartDestructionPhase);
        EventQueue.GetEventQueue().Subscribe(EventType.PlayerDestructionReady, PlayerIsReady);
    }

    /*  Functionality
     *  --------------------------------------------------------------------------------------------------------------------
     *
     *
     *
     *
     * 
     * 1. Start preparation phase by calling 'StartPrepPhase' - this sets text elements and the current phase.
     * 2. Players pressed 'ready' - 'PlayerIsReady' is called by event. Event is raised by UIHandler TODO: Move it from there. 
     *      - Phase text is set
     *      - Countdown gets started
     * 3. Countdown over, calls 'PreparationPhaseOver' event
     * 
     */

    void Start() {
        EventQueue.GetEventQueue().AddEvent(new EventData(EventType.InFadeToPreparation));
        EventQueue.GetEventQueue().AddEvent(new EventData(EventType.PreparationPhase));
    }

    private void StartPreparationPhase(EventData eventData) {
        CurrentEventType = EventType.PreparationPhase;
    }

    private void StartDestructionPhase(EventData eventData) {
        CurrentEventType = EventType.DestructionPhase;
    }

    private void PlayerIsReady(EventData eventData) {
        PlayerReadyEventData playerReadyEventData = (PlayerReadyEventData) eventData;

        //This lets the player who started the countdown, abort it again.
        if (_countdownRunning) {
            if (_readyPlayer == playerReadyEventData.playerName) {
                StartCoroutine(AbortReadyCountdown());
            }

            return;
        }

        _readyPlayer = playerReadyEventData.playerName;

        _playerReadyCountdown = StartReadyCountdown(eventData.eventType == EventType.PlayerPreparationReady
            ? EventType.PreparationPhaseOver
            : EventType.DestructionPhaseOver);


        StartCoroutine(_playerReadyCountdown);
    }

    private IEnumerator StartReadyCountdown(EventType eventPublishType) {
        yield return new WaitForEndOfFrame();
        _countdownCoroutine =
            StartCountdown(eventPublishType == EventType.PreparationPhaseOver ? preparationTime : destructionTime);

        StartCoroutine(_countdownCoroutine);
        yield return new WaitWhile(() => _countdownRunning);

        //This event initiates the camera transition and fade
        EventQueue.GetEventQueue().AddEvent(new EventData(eventPublishType));
    }

    private void StartAttackPhase(EventData eventData) {
        StartCoroutine(AttackPhase());
    }

    private IEnumerator AttackPhase() {
        CurrentEventType = EventType.AttackPhase;
        yield return new WaitForEndOfFrame();
        StartCoroutine(StartCountdown(attackTime));
        yield return new WaitWhile(() => _countdownRunning);

        //This event initiates the camera transition and fade
        EventQueue.GetEventQueue().AddEvent(new EventData(EventType.AttackPhaseOver));
    }

    //Stops all running coroutines and starts preparation phase again.
    private IEnumerator AbortReadyCountdown() {
        StopCoroutine(_countdownCoroutine);
        StopCoroutine(_playerReadyCountdown);
        _countdownRunning = false;
        yield return new WaitForEndOfFrame();

        if (CurrentEventType == EventType.PreparationPhase) {
            EventQueue.GetEventQueue().AddEvent(new EventData(EventType.InFadeToPreparation));
        }
        else if (CurrentEventType == EventType.DestructionPhase) {
            EventQueue.GetEventQueue().AddEvent(new EventData(EventType.InFadeToDestruction));
        }
    }

    private IEnumerator StartCountdown(float time) {
        _countdownRunning = true;
        float timer = time;
        while (timer > 0) {
            timer -= Time.deltaTime;

            EventType eventType = EventType.Debug;
            switch (CurrentEventType) {
                case EventType.PreparationPhase:
                    eventType = EventType.PreparationPhaseTimeUpdate;
                    break;
                case EventType.AttackPhase:
                    eventType = EventType.AttackPhaseTimeUpdate;
                    break;
                case EventType.DestructionPhase:
                    eventType = EventType.DestructionPhaseTimeUpdate;
                    break;
            }

            EventQueue.GetEventQueue().AddEvent(new PhaseTimeData(eventType, timer, _readyPlayer));
            yield return null;
        }

        _countdownRunning = false;
    }
}
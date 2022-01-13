using TMPro;
using UnityEngine;

public class PhaseUIManager : MonoBehaviour {
    [SerializeField] private TMP_Text phase;
    [SerializeField] private TMP_Text time;

    private float _timer;

    private void Awake() {
        //EventQueue.GetEventQueue().Subscribe(EventType.PreparationPhase, SetText);
        EventQueue.GetEventQueue().Subscribe(EventType.InitPreparationPhase, SetTimeText);
        EventQueue.GetEventQueue().Subscribe(EventType.PrepPhaseTimeUpdate, SetTime);
        EventQueue.GetEventQueue().Subscribe(EventType.AttackPhaseTimeUpdate, SetTime);
        EventQueue.GetEventQueue().Subscribe(EventType.DestructionPhaseTimeUpdate, SetTime);
        EventQueue.GetEventQueue().Subscribe(EventType.InFadeToAttack, SetText);
        EventQueue.GetEventQueue().Subscribe(EventType.InFadeToPreparation, SetText);
        EventQueue.GetEventQueue().Subscribe(EventType.InFadeToDestruction, SetText);
    }

    private void SetText(EventData eventData) {
        PhaseUIEventData uiEventData = (PhaseUIEventData) eventData;
        phase.text = uiEventData.phaseName;
    }

    private void SetTime(EventData eventData) {
        PhaseTimeData timeData = (PhaseTimeData) eventData;
        int minutes = (int) timeData.time / 60;
        int seconds = (int) timeData.time % 60;

        switch (eventData.eventType) {
            case EventType.PrepPhaseTimeUpdate:
                time.text = $"{timeData.playerName} has initiated an attack! Time left: {minutes:00}:{seconds:00}";
                break;
            case EventType.AttackPhaseTimeUpdate:
                time.text = $"Time left: {minutes:00}:{seconds:00}";
                break;
            case EventType.DestructionPhaseTimeUpdate:
                time.text = $"{timeData.playerName}'s attack is over! Time left: {minutes:00}:{seconds:00}";
                break;
            default:
                Debug.LogError("Wrong EventType used on 'PhaseUIUpdate'");
                break;
        }
    }

    private void SetTimeText(EventData eventData) {
        MessageEventData messageEventData = (MessageEventData) eventData;
        time.text = messageEventData.message;
    }
}
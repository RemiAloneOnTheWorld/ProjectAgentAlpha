using TMPro;
using UnityEngine;

public class PhaseUIManager : MonoBehaviour {
    [SerializeField] private TMP_Text phase;
    [SerializeField] private TMP_Text time;

    private float _timer;

    private void Awake() {
        EventQueue.GetEventQueue().Subscribe(EventType.PreparationPhase, SetText);
        EventQueue.GetEventQueue().Subscribe(EventType.InitPreparationPhase, SetTimeText);
        EventQueue.GetEventQueue().Subscribe(EventType.PrepPhaseTimeUpdate, SetTime);
        EventQueue.GetEventQueue().Subscribe(EventType.AttackPhaseTimeUpdate, SetTime);
        EventQueue.GetEventQueue().Subscribe(EventType.InFadeToAttack, SetText);
    }

    private void SetText(EventData eventData) {
        PhaseUIEventData uiEventData = (PhaseUIEventData) eventData;
        phase.text = uiEventData.phaseName;
        if (eventData.eventType == EventType.InFadeToAttack) {
            time.text = " ";
        }
    }

    private void SetTime(EventData eventData) {
        PhaseTimeData timeData = (PhaseTimeData) eventData;
        int minutes = (int) timeData.time / 60;
        int seconds = (int) timeData.time % 60;
        if (eventData.eventType == EventType.PrepPhaseTimeUpdate) {
            time.text = $"{timeData.playerName} has initiated an attack! Time left: {minutes:00}:{seconds:00}";
        }
        else {
            time.text = $"Time left: {minutes:00}:{seconds:00}";
        }
    }

    private void SetTimeText(EventData eventData) {
        MessageEventData messageEventData = (MessageEventData) eventData;
        time.text = messageEventData.message;
    }
}
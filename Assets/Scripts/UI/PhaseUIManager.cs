using TMPro;
using UnityEngine;

public class PhaseUIManager : MonoBehaviour {
    [SerializeField] private TMP_Text phase;
    [SerializeField] private TMP_Text time;

    private float _timer;

    private void Awake() {
        EventQueue.GetEventQueue().Subscribe(EventType.PreparationPhase, SetText);
        EventQueue.GetEventQueue().Subscribe(EventType.PhaseTimeUpdate, SetTime);
        EventQueue.GetEventQueue().Subscribe(EventType.InFadeToAttack, SetText);
    }
    
    private void SetText(EventData eventData) {
        PhaseUIEventData uiEventData = (PhaseUIEventData) eventData;
        phase.text = uiEventData.phaseName;
    }

    private void SetTime(EventData eventData) {
        PhaseTimeData timeData = (PhaseTimeData) eventData;
        int minutes = (int) timeData.time / 60;
        int seconds = (int) timeData.time % 60;
        time.text = "Time left: " + minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}


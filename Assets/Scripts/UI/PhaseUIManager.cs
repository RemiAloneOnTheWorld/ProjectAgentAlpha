using TMPro;
using UnityEngine;

public class PhaseUIManager : MonoBehaviour {
    [SerializeField] private TMP_Text phase;
    [SerializeField] private TMP_Text time;

    private float _timer;

    private void Awake() {
        EventQueue.GetEventQueue().Subscribe(EventType.InFadeToPreparation, SetTimeText);
        EventQueue.GetEventQueue().Subscribe(EventType.InFadeToPreparation, SetPhaseText);

        EventQueue.GetEventQueue().Subscribe(EventType.InFadeToAttack, SetTimeText);
        EventQueue.GetEventQueue().Subscribe(EventType.InFadeToAttack, SetPhaseText);

        EventQueue.GetEventQueue().Subscribe(EventType.InFadeToDestruction, SetTimeText);
        EventQueue.GetEventQueue().Subscribe(EventType.InFadeToDestruction, SetPhaseText);

        EventQueue.GetEventQueue().Subscribe(EventType.PreparationPhaseTimeUpdate, SetTime);
        EventQueue.GetEventQueue().Subscribe(EventType.AttackPhaseTimeUpdate, SetTime);
        EventQueue.GetEventQueue().Subscribe(EventType.DestructionPhaseTimeUpdate, SetTime);

        EventQueue.GetEventQueue().Subscribe(EventType.GameOver, OnGameOver);
    }

    private void OnGameOver(EventData eventData) {
        phase.gameObject.SetActive(false);
        time.gameObject.SetActive(false);

        //Todo: Show game over UI and go back to menu or restart


    }

    private void SetPhaseText(EventData eventData) {
        switch (eventData.eventType) {
            case EventType.InFadeToPreparation:
                phase.text = "Preparation Phase";
                break;
            case EventType.InFadeToAttack:
                phase.text = "Spaceship Attack Phase";
                break;
            case EventType.InFadeToDestruction:
                phase.text = "Destruction Phase";
                break;
        }
    }

    private void SetTime(EventData eventData) {
        PhaseTimeData timeData = (PhaseTimeData)eventData;
        int minutes = (int)timeData.time / 60;
        int seconds = (int)timeData.time % 60;

        switch (eventData.eventType) {
            case EventType.PreparationPhaseTimeUpdate:
                time.text = $"{timeData.playerName} has initiated an attack! Time left: {minutes:00}:{seconds:00}";
                break;
            case EventType.AttackPhaseTimeUpdate:
                time.text = $"Spaceships are attacking! Time left: {minutes:00}:{seconds:00}";
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
        switch (eventData.eventType) {
            case EventType.InFadeToPreparation:
                time.text = "Initiate attack? Press 'R' or 'North Button'";
                break;
            case EventType.InFadeToAttack:
                time.text = "";
                break;
            case EventType.InFadeToDestruction:
                time.text = "Done attacking? Press 'R' or 'North Button'";
                break;
        }
    }
}
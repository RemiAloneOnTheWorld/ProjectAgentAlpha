using UnityEngine;

public class EventHandler : MonoBehaviour {
    void Update() {
        EventQueue.GetEventQueue().RaiseEvents();
    }
}
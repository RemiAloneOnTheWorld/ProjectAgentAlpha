using System.Collections.Generic;
using UnityEngine;

public class BoxCreationModule : MonoBehaviour {
    [SerializeField] private GameObject box;
    private List<Transform> _placementPositions = new List<Transform>();

    private void Awake() {
        EventQueue.GetEventQueue().Subscribe(EventType.PreparationPhase, GenerateBoxes);
        foreach (var component in transform.GetComponentsInChildren<Transform>()) {
            _placementPositions.Add(component);
        }
    }
    
    private void GenerateBoxes(EventData eventData) {
        foreach (var position in _placementPositions) {
            Instantiate(box, position.position, transform.rotation, transform);
        }
    }
}


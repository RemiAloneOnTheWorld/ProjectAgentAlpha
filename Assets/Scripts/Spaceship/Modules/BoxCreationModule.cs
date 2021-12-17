using System.Collections.Generic;
using UnityEngine;

public class BoxCreationModule : Module {
    [SerializeField] private GameObject box;
    private List<Transform> _placementPositions = new List<Transform>();

    private void Awake() {
        EventQueue.GetEventQueue().Subscribe(EventType.PreparationPhase, GenerateBoxes);
        for (int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i).CompareTag("Connection")) {
                continue;
            }
            
            _placementPositions.Add(transform.GetChild(i));
        }
    }
    
    private void GenerateBoxes(EventData eventData) {
        foreach (var position in _placementPositions) {
            Instantiate(box, position.position, transform.rotation, transform);
        }
    }
}


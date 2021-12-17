using UnityEngine;

public class BoxCreationModule : Module {
    [SerializeField] private GameObject box;
    [SerializeField] private Transform[] placementPositions;

    private void Awake() {
        EventQueue.GetEventQueue().Subscribe(EventType.PreparationPhase, GenerateBoxes);
    }
    
    private void GenerateBoxes(EventData eventData) {
        foreach (var position in placementPositions) {
            Instantiate(box, position.position, transform.rotation, transform);
        }
    }
    
    private void OnDrawGizmos() {
        Gizmos.color = new Color(255, 0, 255, 0.5f);
        foreach (var currentBox in placementPositions) {
            Gizmos.DrawCube(currentBox.position, currentBox.localScale);   
        }
    }
}


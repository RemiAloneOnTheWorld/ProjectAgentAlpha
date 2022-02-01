using UnityEngine;

public class BoxCreationModule : Module {
    [SerializeField] private GameObject[] boxes;
    [SerializeField] private Transform[] placementPositions;

    private void Awake() {
        EventQueue.GetEventQueue().Subscribe(EventType.PreparationPhase, GenerateBoxes);
    }
    
    private void GenerateBoxes(EventData eventData) {
        foreach (var position in placementPositions) {
            GameObject cube = Instantiate(boxes[Random.Range(0,boxes.Length)], position.position, transform.rotation, transform);
            Vector3 scale = cube.transform.localScale;
            scale *= .2f;
            cube.transform.localScale = scale;
        }
    }
    
    private void OnDrawGizmos() {
        Gizmos.color = new Color(255, 0, 255, 0.5f);
        foreach (var currentBox in placementPositions) {
            Gizmos.DrawCube(currentBox.position, currentBox.localScale);   
        }
    }

    public override void DestroyModuleWithSubs()
    {
        base.DestroyModuleWithSubs();
        EventQueue.GetEventQueue().Unsubscribe(EventType.PreparationPhase, GenerateBoxes);
    }
}


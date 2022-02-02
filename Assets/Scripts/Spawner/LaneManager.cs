using UnityEngine;

public class LaneManager : MonoBehaviour {
    [SerializeField] private GameObject xzCollider;
    [SerializeField] private GameObject yzCollider;

    private Vector3 _halfExtends;
    private Vector3 _position;

    private void Awake() {
        EventQueue.GetEventQueue().Subscribe(EventType.DestructionPhaseOver, DestroyBoxesInLane);
    }

    private void OnDisable() {
        EventQueue.GetEventQueue().Unsubscribe(EventType.DestructionPhaseOver, DestroyBoxesInLane);
    }

    private void Start() {
        _position = new Vector3(xzCollider.transform.position.x, yzCollider.transform.position.y,
            xzCollider.transform.position.z);

        _halfExtends = new Vector3(xzCollider.transform.lossyScale.x, yzCollider.transform.lossyScale.y,
            xzCollider.transform.lossyScale.z) / 2;
    }

    private void DestroyBoxesInLane(EventData eventData) {
        Collider[] colliders = Physics.OverlapBox(_position, _halfExtends, Quaternion.identity);
        for (int i = colliders.Length - 1; i >= 0; i--) {
            if (colliders[i].CompareTag("Block"))
                Destroy(colliders[i].gameObject);
        }
    }
}

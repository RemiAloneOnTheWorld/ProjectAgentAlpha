using UnityEngine;

public class PlanetRotation : MonoBehaviour {
    [SerializeField] private float rotationSpeed;
    private Vector3 _rotationAxis;
    
    private void Start() {
        _rotationAxis = Random.onUnitSphere;
    }

    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}

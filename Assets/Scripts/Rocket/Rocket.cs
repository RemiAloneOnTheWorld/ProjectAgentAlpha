using System.Collections;
using UnityEngine;

public class Rocket : MonoBehaviour {
    [SerializeField] private float speedCoefficient;
    [SerializeField] private Vector3 launchImpulse;
    public bool isFlying { get; private set; }
    private Rigidbody _rigidbody;
    private GameObject _module;

    public IEnumerator LaunchAttack(GameObject module) {
        _rigidbody = GetComponent<Rigidbody>();
        isFlying = true;
        _module = module;
        Vector3 initDistance = module.transform.position - transform.position;
        _rigidbody.AddForce(launchImpulse * speedCoefficient, ForceMode.Impulse);
        while (true) {
            Vector3 currentDistance = module.transform.position - transform.position;
            float term = 1 - Mathf.Min(0.5f, (currentDistance.magnitude / (initDistance.magnitude * 4)));
            _rigidbody.AddForce(speedCoefficient * term * currentDistance.normalized, ForceMode.Force);
            transform.rotation = Quaternion.LookRotation(_rigidbody.velocity, Vector3.up);
            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.TryGetComponent<Module>(out var module) || collision.gameObject.CompareTag("Connection")) {
            isFlying = false;
        }
        else {
            Debug.LogWarning("Rocket hit module, but not the correct one!");
        }
    }
}




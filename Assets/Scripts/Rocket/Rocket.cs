using System.Collections;
using UnityEngine;

public class Rocket : MonoBehaviour {
    [SerializeField] private float speedCoefficient;
    [SerializeField] private Vector3 launchImpulse;
    public bool isFlying { get; private set; }
    private Rigidbody _rigidbody;
    private Coroutine _coroutine;
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
            Debug.Log("Term is: " + term);
            _rigidbody.AddForce(speedCoefficient * term * currentDistance.normalized, ForceMode.Force);
            transform.rotation = Quaternion.LookRotation(_rigidbody.velocity, Vector3.up);
            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject == _module) {
            isFlying = false;
        }
        else {
            Debug.LogWarning("Rocket hit module, but not the correct one!");
        }
    }
}




using System;
using Starventure.Planets;
using UnityEngine;

namespace Starventure.Physics {
    [RequireComponent(typeof(Rigidbody))]
    public class StellarRigidbody : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        public Planet currentPlanet;
        public AtmosphereCollider currentAtmosphere;
        
        public bool useRegularGravity;

        private Vector3 _gravityVector;
        private float _regularDamping;
        private float _regularAngularDamping;

        private void Start() {
            if (!rb) {
                rb = GetComponent<Rigidbody>();
            }
            
            rb.useGravity = useRegularGravity;
            _regularDamping = rb.linearDamping;
            _regularAngularDamping = rb.angularDamping;
        }

        private void Update() {
            if (!currentPlanet && !useRegularGravity) {
                _gravityVector = Vector3.zero;
                rb.linearDamping = 0;
                rb.angularDamping = 0;
                return;
            }

            if (useRegularGravity) {
                rb.useGravity = true;
                rb.linearDamping = _regularDamping;
                rb.angularDamping = _regularAngularDamping;
                return;
            }

            _gravityVector = currentPlanet.gravity.CalculateGravityVector(rb.position);
        }

        private void OnTriggerEnter(Collider other) {
            if (!other.CompareTag(CustomTags.Atmosphere)) {
                return;
            }

            currentAtmosphere = other.GetComponent<AtmosphereCollider>();
            if (!currentAtmosphere) {
                Debug.LogError($"{other.name} is tagged with {CustomTags.Atmosphere}, but no {nameof(AtmosphereCollider)} is present!");
                return;
            }

            currentPlanet = currentAtmosphere.planet;
            Debug.Log($"Found planet: {currentPlanet.name}");
        }

        private void OnTriggerExit(Collider other) {
            if (other == currentAtmosphere.baseCollider) {
                Debug.Log($"Left planet: {currentPlanet.name}");
                currentAtmosphere = null;
                currentPlanet = null;
            }
        }

        private void OnDrawGizmosSelected() {
            if (!currentPlanet || !rb || !Application.isPlaying) {
                return;
            }
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(rb.position, rb.position + _gravityVector);
        }
    }
}

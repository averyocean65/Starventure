using UnityEngine;

namespace Starventure.Physics {
    [RequireComponent(typeof(Rigidbody))]
    public class StellarRigidbody : StellarObject
    {
        public Rigidbody rb;
        
        public bool useRegularGravity;

        public Vector3 GravityVector { get; protected set; }
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
            if (useRegularGravity) {
                rb.useGravity = true;
                rb.linearDamping = _regularDamping;
                rb.angularDamping = _regularAngularDamping;
                return;
            }

            rb.useGravity = false;
            
            if (!currentPlanet) {
                GravityVector = Vector3.zero;
                rb.linearDamping = 0;
                rb.angularDamping = 0;
            }

            float dampingMultiplier = currentPlanet.gravity.CalculateDampingMultiplier(rb.position);
            rb.linearDamping = _regularDamping * dampingMultiplier;
            rb.angularDamping = _regularAngularDamping * dampingMultiplier;
        }

        private void FixedUpdate() {
            if (!currentPlanet || useRegularGravity) {
                return;
            }

            GravityVector = currentPlanet.gravity.CalculateGravityVector(rb.position);
            rb.AddForce(GravityVector, ForceMode.Acceleration);
        }

        private void OnDrawGizmosSelected() {
            if (!currentPlanet || !rb || !Application.isPlaying) {
                return;
            }
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(rb.position, rb.position + GravityVector);
        }
    }
}

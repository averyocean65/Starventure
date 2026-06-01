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

        private void Start() {
            if (!rb) {
                rb = GetComponent<Rigidbody>();
            }
            
            rb.useGravity = false;
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
    }
}

using System;
using Starventure.Planets;
using UnityEngine;
using UnityEngine.Events;

namespace Starventure {
    public class StellarObject : MonoBehaviour {
        public Planet currentPlanet;
        public AtmosphereCollider currentAtmosphere;

        public UnityEvent<Planet> OnEnterPlanet;
        public UnityEvent<Planet> OnExitPlanet;

        private void OnTriggerEnter(Collider other) {
            if (!other.CompareTag(CustomTags.Atmosphere)) {
                return;
            }

            currentAtmosphere = other.GetComponent<AtmosphereCollider>();
            if (!currentAtmosphere) {
                Debug.LogError($"{other.name} is tagged with {CustomTags.Atmosphere}, but no {nameof(AtmosphereCollider)} is present!");
                return;
            }

            OnEnterPlanet.Invoke(currentPlanet);
            
            currentPlanet = currentAtmosphere.planet;
            currentPlanet.RegisterVisitor(gameObject);
            Debug.Log($"Found planet: {currentPlanet.name}");
        }

        private void OnTriggerExit(Collider other) {
            if (other == currentAtmosphere.baseCollider) {
                Debug.Log($"Left planet: {currentPlanet.name}");
                
                OnExitPlanet.Invoke(currentPlanet);
                currentPlanet.UnregisterVisitor(gameObject);
            
                currentAtmosphere = null;
                currentPlanet = null;

                transform.parent = null;
            }
        }
    }
}

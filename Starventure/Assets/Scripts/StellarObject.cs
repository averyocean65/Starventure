using Starventure.Planets;
using UnityEngine;

namespace Starventure {
    public class StellarObject : MonoBehaviour {
        public Planet currentPlanet;
        public AtmosphereCollider currentAtmosphere;
        
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
    }
}

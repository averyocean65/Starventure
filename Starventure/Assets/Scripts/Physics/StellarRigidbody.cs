using System;
using Starventure.Planets;
using UnityEngine;

namespace Starventure.Physics {
    [RequireComponent(typeof(Rigidbody))]
    public class StellarRigidbody : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        public Planet currentPlanet;

        private void Start() {
            if (!rb) {
                rb = GetComponent<Rigidbody>();
            }
        }
    }
}

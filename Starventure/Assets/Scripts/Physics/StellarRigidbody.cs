using System;
using UnityEngine;

namespace Starventure.Physics {
    [RequireComponent(typeof(Rigidbody))]
    public class StellarRigidbody : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;

        private void Start() {
            if (!rb) {
                return;
            }

            rb = GetComponent<Rigidbody>();
        }
    }
}

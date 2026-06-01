using System;
using UnityEngine;

namespace Starventure.Planets {
    [RequireComponent(typeof(SphereCollider))]
    public class AtmosphereCollider : MonoBehaviour {
        [SerializeField] private Planet planet;
        [SerializeField] private SphereCollider baseCollider;

        private void Start() {
            if (!baseCollider) {
                baseCollider = GetComponent<SphereCollider>();
            }

            baseCollider.tag = CustomTags.Atmosphere;
            baseCollider.center = baseCollider.transform.position - planet.gravity.core.position;
            baseCollider.radius = planet.gravity.Radius;
            baseCollider.isTrigger = true;
        }
    }
}

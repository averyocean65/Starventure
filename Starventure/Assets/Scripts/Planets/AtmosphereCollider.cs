using Starventure.Physics.Gravity;
using UnityEngine;

namespace Starventure.Planets {
    [RequireComponent(typeof(Collider))]
    public class AtmosphereCollider : MonoBehaviour {
        public Planet planet;
        public Collider baseCollider;

        private void Start() {
            if (!baseCollider) {
                baseCollider = GetComponent<Collider>();
            }
            
            baseCollider.tag = CustomTags.Atmosphere;
            baseCollider.isTrigger = true;

            if (planet.gravity is SphericalGravity sg && baseCollider is SphereCollider sc) {
                sc.center = baseCollider.transform.position - sg.root.position;
                sc.radius = sg.Radius / transform.lossyScale.x;
            }
        }
    }
}

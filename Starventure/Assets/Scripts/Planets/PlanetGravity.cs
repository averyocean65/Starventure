using System;
using UnityEngine;

namespace Starventure.Planets {
    public class PlanetGravity : MonoBehaviour {
        public Transform core;
        public float innerRadius = 10.0f;
        public float outerRadius = 5.0f;
        public float gravity = 9.81f;
        public AnimationCurve gravityLossCurve = AnimationCurve.Linear(0, 1, 1, 0);

        private void OnDrawGizmos() {
            if(!core) {
                return;
            }
            
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(core.position, innerRadius);

            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(core.position, innerRadius + outerRadius);
        }
    }
}

using System;
using UnityEngine;

namespace Starventure.Planets {
    public class PlanetGravity : MonoBehaviour {
        public float Radius => innerRadius + outerRadius;
        
        public Transform core;
        public float innerRadius = 10.0f;
        public float outerRadius = 5.0f;
        public float gravity = 9.81f;
        public AnimationCurve gravityLossCurve = AnimationCurve.Linear(0, 1, 1, 0);
        public AnimationCurve dampingCurve = AnimationCurve.Linear(0, 1, 1, 0);

        private void OnDrawGizmos() {
            if(!core) {
                return;
            }
            
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(core.position, innerRadius);

            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(core.position, Radius);
        }

        public float CalculateGravityStrength(Vector3 objectPosition) {
            float distance = Vector3.Distance(core.position, objectPosition);
            if (distance <= innerRadius) {
                return gravity;
            }

            if (distance >= Radius) {
                return 0;
            }
            
            return gravityLossCurve.Evaluate((distance - innerRadius) / outerRadius) * gravity;
        }
        
        public float CalculateDampingMultiplier(Vector3 objectPosition) {
            float distance = Vector3.Distance(core.position, objectPosition);
            if (distance <= innerRadius) {
                return 1;
            }

            if (distance >= Radius) {
                return 0;
            }
            
            return gravityLossCurve.Evaluate((distance - innerRadius) / outerRadius);
        }

        public Vector3 CalculateGravityDirection(Vector3 objectPosition) {
            return (core.position - objectPosition).normalized;
        }
        
        public Vector3 CalculateGravityVector(Vector3 objectPosition) {
            return CalculateGravityDirection(objectPosition) * CalculateGravityStrength(objectPosition);
        }
    }
}

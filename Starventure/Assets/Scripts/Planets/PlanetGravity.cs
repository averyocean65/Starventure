using System;
using UnityEngine;

namespace Starventure.Planets {
    public class PlanetGravity : MonoBehaviour {
        private float RadiusScale => core.lossyScale.x;
        
        public float Radius => InnerRadius + OuterRadius;
        public float InnerRadius => innerRadius * RadiusScale;
        public float OuterRadius => outerRadius * RadiusScale;
        
        public Transform core;
        [SerializeField] private float innerRadius = 10.0f;
        [SerializeField] private float outerRadius = 5.0f;
        public float gravity = 9.81f;
        public AnimationCurve gravityLossCurve = AnimationCurve.Linear(0, 1, 1, 0);
        public AnimationCurve dampingCurve = AnimationCurve.Linear(0, 1, 1, 0);

        private void OnDrawGizmos() {
            if(!core) {
                return;
            }
            
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(core.position, InnerRadius);

            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(core.position, Radius);
        }

        public virtual float CalculateGravityStrength(Vector3 objectPosition) {
            float distance = Vector3.Distance(core.position, objectPosition);
            if (distance <= InnerRadius) {
                return gravity;
            }

            if (distance >= Radius) {
                return 0;
            }
            
            return gravityLossCurve.Evaluate((distance - InnerRadius) / OuterRadius) * gravity;
        }
        
        public virtual float CalculateDampingMultiplier(Vector3 objectPosition) {
            float distance = Vector3.Distance(core.position, objectPosition);
            if (distance <= InnerRadius) {
                return 1;
            }

            if (distance >= Radius) {
                return 0;
            }
            
            return gravityLossCurve.Evaluate((distance - InnerRadius) / OuterRadius);
        }

        public virtual Vector3 CalculateGravityDirection(Vector3 objectPosition) {
            return (core.position - objectPosition).normalized;
        }
        
        public Vector3 CalculateGravityVector(Vector3 objectPosition) {
            return CalculateGravityDirection(objectPosition) * CalculateGravityStrength(objectPosition);
        }
    }
}

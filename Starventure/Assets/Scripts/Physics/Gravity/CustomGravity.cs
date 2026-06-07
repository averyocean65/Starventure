using System;
using UnityEngine;

namespace Starventure.Physics.Gravity {
    public abstract class CustomGravity : MonoBehaviour {
        public Transform root;
        public float gravity = 9.81f;
        public AnimationCurve gravityLossCurve = AnimationCurve.Linear(0, 1, 1, 0);
        public AnimationCurve dampingCurve = AnimationCurve.Linear(0, 1, 1, 0);

        public abstract float CalculateGravityStrength(Vector3 objectPosition);

        public abstract float CalculateDampingMultiplier(Vector3 objectPosition);

        public abstract Vector3 CalculateGravityDirection(Vector3 objectPosition);
        
        public Vector3 CalculateGravityVector(Vector3 objectPosition) {
            return CalculateGravityDirection(objectPosition) * CalculateGravityStrength(objectPosition);
        }
    }
}

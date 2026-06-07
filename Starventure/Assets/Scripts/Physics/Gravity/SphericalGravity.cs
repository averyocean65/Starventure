using Starventure.Planets;
using UnityEngine;

namespace Starventure.Physics {
	public class SphericalGravity : CustomGravity {
		private float RadiusScale => root.lossyScale.x;
		public float Radius => InnerRadius + OuterRadius;
		public float InnerRadius => innerRadius * RadiusScale;
		public float OuterRadius => outerRadius * RadiusScale;
		
		[SerializeField] private float innerRadius = 10.0f;
		[SerializeField] private float outerRadius = 5.0f;
		
		public override float CalculateGravityStrength(Vector3 objectPosition) {
			float distance = Vector3.Distance(root.position, objectPosition);
			if (distance <= InnerRadius) {
				return gravity;
			}

			if (distance >= Radius) {
				return 0;
			}
            
			return gravityLossCurve.Evaluate((distance - InnerRadius) / OuterRadius) * gravity;
		}
        
		public override float CalculateDampingMultiplier(Vector3 objectPosition) {
			float distance = Vector3.Distance(root.position, objectPosition);
			if (distance <= InnerRadius) {
				return 1;
			}

			if (distance >= Radius) {
				return 0;
			}
            
			return gravityLossCurve.Evaluate((distance - InnerRadius) / OuterRadius);
		}

		public override Vector3 CalculateGravityDirection(Vector3 objectPosition) {
			return (root.position - objectPosition).normalized;
		}
		
		private void OnDrawGizmos() {
			if(!root) {
				return;
			}
            
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireSphere(root.position, InnerRadius);

			Gizmos.color = Color.magenta;
			Gizmos.DrawWireSphere(root.position, Radius);
		}
	}
}
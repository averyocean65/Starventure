using System;
using Starventure.Physics;
using UnityEngine;

namespace Starventure.Player {
	public class PlayerController : MonoBehaviour {
		[SerializeField] private StellarRigidbody srb;
		private Rigidbody _rb;

		private void Start() {
			if (!srb) {
				srb = GetComponent<StellarRigidbody>();
			}

			_rb = srb.rb;
		}

		private void Update() {
			if (!srb.currentPlanet) {
				return;
			}

			transform.up = -srb.currentPlanet.gravity.CalculateGravityDirection(transform.position);
		}
	}
}

using System;
using Starventure.Physics;
using UnityEngine;

namespace Starventure.Player {
	public class PlayerController : MonoSingleton<PlayerController> {
		[SerializeField] private StellarRigidbody srb;
		[SerializeField] private float acceleration = 5.0f;
		[SerializeField] private float maxSpeed = 5.0f;
		private Rigidbody _rb;
		private Vector2 _moveInput;

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

			_moveInput = InputManager.Player.Move.ReadValue<Vector2>().normalized;
		}

		private void FixedUpdate() {
			Vector3 movement = transform.right * _moveInput.x + transform.forward * _moveInput.y;
			_rb.AddForce(movement * acceleration, ForceMode.Acceleration);

			Vector3 movementVelocity = _rb.linearVelocity - srb.GravityVector;
			if (movementVelocity.magnitude > maxSpeed) {
				_rb.AddForce(movementVelocity.normalized * maxSpeed + srb.GravityVector, ForceMode.VelocityChange);
			}
		}
	}
}

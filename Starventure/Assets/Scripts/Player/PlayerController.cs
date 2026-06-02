using System;
using Starventure.Physics;
using UnityEngine;

namespace Starventure.Player {
	public class PlayerController : MonoSingleton<PlayerController> {
		[SerializeField] private StellarRigidbody srb;
		[SerializeField] private Transform orientation;
		[SerializeField] private float speed = 5.0f;
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
			Vector3 movementDir = orientation.right * _moveInput.x + orientation.forward * _moveInput.y;
			Vector3 predictedPos = Vector3.MoveTowards(transform.position, transform.position + movementDir, speed * Time.deltaTime);
			_rb.MovePosition(predictedPos);
		}
	}
}

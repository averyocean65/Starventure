
using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Starventure.Physics;
using Starventure.Planets;
using Unity.VisualScripting;
using UnityEngine;

namespace Starventure.Player {
	public class PlayerController : MonoSingleton<PlayerController> {
		[SerializeField] private StellarRigidbody srb;
		[SerializeField] private Transform orientation;
		
		[Header("Movement")]
		[SerializeField] private float acceleration = 10.0f;
		[SerializeField] private float movementHaltForce = 0.5f;
		[SerializeField] private float speed = 5.0f;
		[SerializeField] private float sprintSpeed = 5.0f;
		[SerializeField] private float movementHaltThreshold = 0.05f;
		
		[Header("Jetpack")]
		[SerializeField] private float jetpackLaunchForce = 5.0f;
		[SerializeField] private float jetpackCooldown = 0.1f;
		[SerializeField] private DepletableResource stamina;

		[Header("Planet Relocation")]
		[SerializeField] private float realignSpeed = 1.0f;
		
		[SerializeField] private Transform groundCheck;
		[SerializeField] private float groundCheckRadius;
		[SerializeField] private LayerMask groundCheckLayer;

		private float _currentSpeed => _isSprinting ? sprintSpeed : speed;
		
		private bool _isGrounded;

		private TweenerCore<Vector3, Vector3, VectorOptions> _tweener;
		private bool _isTweenerRunning;
		private bool _canDisableGravity;
		private bool _canDisableJetpack;
		
		private bool _isSprinting = false;
		private bool _sprintRefill = false;

		private Vector3 _gravity;
		
		private Rigidbody _rb;
		
		private Vector2 _moveInput;
		private Vector3 _approximatedMovementVelocity;

		private void Start() {
			if (!srb) {
				srb = GetComponent<StellarRigidbody>();
			}
			
			_rb = srb.rb;
			srb.OnEnterPlanet.AddListener(OnEnterPlanet);
		}

		private void UpdateIsGrounded() {
			if (!srb.currentPlanet) {
				_isGrounded = false;
				return;
			}

			_isGrounded = UnityEngine.Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundCheckLayer,
				QueryTriggerInteraction.Ignore);
		}

		private void Update() {
			if (!_isTweenerRunning && srb.currentPlanet && srb.GravityDirection != Vector3.zero) {
				_gravity = srb.GravityDirection;
			}
			
			transform.up = -_gravity;
			_moveInput = InputManager.Player.Move.ReadValue<Vector2>().normalized;
			
			UpdateIsGrounded();

			if (srb.currentPlanet && (InputManager.Player.Jump.WasPressedThisFrame() && _isGrounded)) {
				StartCoroutine(IJetpackLaunchCooldown());
				
				_canDisableGravity = true;
				_rb.AddForce(transform.up * jetpackLaunchForce, ForceMode.Impulse);
				stamina.Deplete();
			}
			
			if (((InputManager.Player.Jump.WasReleasedThisFrame() || _isGrounded)
			     && _canDisableGravity && _canDisableJetpack)
			    || stamina.IsEmpty) {
				_canDisableJetpack = false;
				stamina.Deplete(false);
			}

			if (InputManager.Player.Sprint.WasPressedThisFrame()) {
				_sprintRefill = false;
			}
			
			if (InputManager.Player.Sprint.IsPressed() 
			    && _moveInput.magnitude >= movementHaltThreshold 
			    && !_sprintRefill
			    && !_isSprinting) {
				_isSprinting = true;
				stamina.Deplete();
			}

			if ((InputManager.Player.Sprint.WasReleasedThisFrame()|| _moveInput.magnitude < movementHaltThreshold || stamina.IsEmpty)
			    && _isSprinting) {
				_isSprinting = false;
				_sprintRefill = true;
				stamina.Deplete(false);
			}

			if (!srb.currentPlanet) {
				stamina.ResetCounter();
			}
			
			srb.disableGravity = InputManager.Player.Jump.IsPressed() && _canDisableGravity;
		}

		private IEnumerator IJetpackLaunchCooldown() {
			_canDisableJetpack = false;
			yield return new WaitForSeconds(jetpackCooldown);
			_canDisableJetpack = true;
		}

		private void FixedUpdate() {
			if (srb.currentPlanet) {
				_rb.angularVelocity = Vector3.zero;
			}
			
			Vector3 movementDir = orientation.right * _moveInput.x + orientation.forward * _moveInput.y;

			if (_moveInput.magnitude < movementHaltThreshold && !srb.disableGravity) {
				_rb.AddForce(-_approximatedMovementVelocity.normalized * movementHaltForce, ForceMode.Acceleration);
			}
			
			_rb.AddForce(movementDir * acceleration, ForceMode.Acceleration);

			Vector3 gravityDeduction = srb.currentPlanet ? _gravity : Vector3.zero;
			_approximatedMovementVelocity = _rb.linearVelocity - gravityDeduction;
			
			if (_approximatedMovementVelocity.magnitude > _currentSpeed) {
				_rb.linearVelocity = _approximatedMovementVelocity.normalized * _currentSpeed + gravityDeduction;
			}
		}

		// i tried my best to make this work with DOTween, but I just can't figure it out, so... screw it!
		private IEnumerator ISmoothGravityRedirect() {
			float percentage = 0.0f;
			Vector3 initialGravity = _gravity;
			
			_isTweenerRunning = true;
			while (percentage < 1.0f) {
				float timeStep = Time.deltaTime * realignSpeed;
				
				_gravity = Vector3.Lerp(initialGravity, srb.GravityDirection, percentage);
				yield return new WaitForSeconds(timeStep);
				percentage += timeStep;
			}

			_isTweenerRunning = false;
		}
		
		private void OnEnterPlanet(Planet arg0) {
			StopAllCoroutines();
			StartCoroutine(ISmoothGravityRedirect());
			_canDisableGravity = true;
			_canDisableJetpack = true;
		}

		private void OnDrawGizmos() {
			if (!groundCheck) {
				return;
			}
			
			// i know, i know.. nested ternary operators are an eyesore, but I'm like... lazy!
			Color color = Application.isPlaying ? (_isGrounded ? Color.green : Color.red) : Color.green;
			Gizmos.color = color;
			Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
		}
	}
}

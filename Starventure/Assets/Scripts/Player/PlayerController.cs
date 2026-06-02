
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Starventure.Physics;
using Starventure.Planets;
using UnityEngine;

namespace Starventure.Player {
	public class PlayerController : MonoSingleton<PlayerController> {
		[SerializeField] private StellarRigidbody srb;
		[SerializeField] private Transform orientation;
		[SerializeField] private float speed = 5.0f;
		[SerializeField] private float jumpForce = 10.0f;

		[SerializeField] private float realignSpeed = 1.0f;
		
		[SerializeField] private Transform groundCheck;
		[SerializeField] private float groundCheckRadius;
		[SerializeField] private LayerMask groundCheckLayer;

		private bool _isGrounded;

		private TweenerCore<Quaternion, Vector3, QuaternionOptions> _tweener;
		private bool _isTweenerRunning;
		
		private Rigidbody _rb;
		private Vector2 _moveInput;

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
			UpdateIsGrounded();
			
			if (!srb.currentPlanet) {
				return;
			}

			_isTweenerRunning = _tweener != null && _tweener.IsActive() && !_tweener.IsComplete();
			if (_isTweenerRunning) {
				return;
			}

			transform.up = -srb.GravityDirection;

			_moveInput = InputManager.Player.Move.ReadValue<Vector2>().normalized;

			if (InputManager.Player.Jump.WasPressedThisFrame() && _isGrounded) {
				_rb.AddForce(-srb.GravityDirection * jumpForce, ForceMode.Impulse);
			}
		}

		private void FixedUpdate() {
			Vector3 movementDir = orientation.right * _moveInput.x + orientation.forward * _moveInput.y;
			Vector3 predictedPos = Vector3.MoveTowards(transform.position, transform.position + movementDir, speed * Time.deltaTime);
			_rb.MovePosition(predictedPos);
		}
		
		private void OnEnterPlanet(Planet arg0) {
			GameObject dummy = new GameObject();
			dummy.transform.up = -srb.GravityDirection;

			_tweener = transform.DORotate(dummy.transform.eulerAngles, realignSpeed);
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

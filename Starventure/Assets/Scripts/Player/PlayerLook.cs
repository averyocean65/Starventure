using System;
using UnityEngine;

namespace Starventure.Player {
    public class PlayerLook : MonoBehaviour {
        private const float SensitivityMultiplier = 0.1f;
        
        [SerializeField] private Transform orientation;
        [SerializeField] private new Transform camera;
        [SerializeField] private Vector2 lookSensitivity = Vector2.one;
        
        [Tooltip("X represent the upper bound, Y represents the lower bound.")]
        [SerializeField] private Vector2 viewClamp = new Vector2(-90, 90);

        private Vector2 _lookDelta;
        private float _xRotation;

        private void Start() {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update() {
            _lookDelta = InputManager.Player.Look.ReadValue<Vector2>() * lookSensitivity * SensitivityMultiplier;
            camera.Rotate(0, _lookDelta.x, 0, Space.Self);

            _xRotation += _lookDelta.y;
            _xRotation = Mathf.Clamp(_xRotation, viewClamp.x, viewClamp.y);
            
            camera.localEulerAngles = new Vector3(-_xRotation, camera.localEulerAngles.y, 0);
            orientation.localEulerAngles = new Vector3(0, camera.localEulerAngles.y, 0);
        }
    }
}
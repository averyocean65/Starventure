using System;
using UnityEngine;

namespace Starventure.Player {
    public class PlayerLook : MonoBehaviour {
        [SerializeField] private Transform orientation;
        [SerializeField] private new Transform camera;
        [SerializeField] private Vector2 lookSensitivity;

        private Vector2 _lookDelta;

        private void Update() {
            _lookDelta = InputManager.Player.Look.ReadValue<Vector2>() * lookSensitivity;
            orientation.Rotate(0, _lookDelta.x, 0, Space.Self);
            camera.Rotate(_lookDelta.y, _lookDelta.x, 0, Space.Self);
        }
    }
}
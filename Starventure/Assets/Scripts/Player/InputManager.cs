using System;
using Starventure;
using Starventure.Generated;

namespace Starventure.Player {
	public class InputManager : MonoSingleton<InputManager> {
		public static PlayerInputActions.PlayerActions Player => Instance._inputActions.Player;
		public static PlayerInputActions.UIActions UI => Instance._inputActions.UI;
		private PlayerInputActions _inputActions;

		private void OnEnable() {
			_inputActions ??= new PlayerInputActions();
			_inputActions.Enable();
		}

		private void OnDisable() {
			if (_inputActions == null) {
				return;
			}
			
			_inputActions.Disable();
		}
	}
}
using System;
using UnityEngine;

namespace Starventure {
	public class MonoSingleton<T> : MonoBehaviour
		where T : MonoSingleton<T> {
		public static T Instance { get; private set; }

		public static bool InstanceExists() {
			return Instance;
		}

		protected virtual void Awake() {
			if (InstanceExists()) {
				Destroy(this);
				return;
			}

			Instance = (T)this;
		}
	}
}
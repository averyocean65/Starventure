using System;
using System.Collections.Generic;
using UnityEngine;

namespace Starventure.Planets {
    public class Planet : MonoBehaviour {
        public PlanetGravity gravity;
        [SerializeField] private Transform visitorParent;
        private readonly Dictionary<GameObject, Transform> _parentMap = new Dictionary<GameObject, Transform>();
        
        public void RegisterVisitor(GameObject visitor) {
            if (!visitorParent) {
                Debug.LogError($"{nameof(visitorParent)} is not set!");
                return;
            }
            
            if(_parentMap.ContainsKey(visitor)) {
                return;
            }
            
            _parentMap.Add(visitor, visitor.transform.parent);
            visitor.transform.SetParent(visitorParent, true);
            
            Debug.Log($"Registered: {visitor.name}");
        }

        public void UnregisterVisitor(GameObject visitor) {
            if (!_parentMap.TryGetValue(visitor, out var parent)) {
                return;
            }

            visitor.transform.SetParent(parent);
            _parentMap.Remove(visitor);
            
            Debug.Log($"Unregistered: {visitor.name}");
        }
    }
}

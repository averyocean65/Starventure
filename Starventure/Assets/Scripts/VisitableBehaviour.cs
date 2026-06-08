using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Starventure {
    public class VisitableBehaviour : MonoBehaviour {
        private const float OrphanCooldown = 0.1f;
        
        [SerializeField] private Transform visitorParent;
        private readonly Dictionary<GameObject, Transform> _parentMap = new Dictionary<GameObject, Transform>();
        private readonly Dictionary<GameObject, Transform> _recentOrphanMap = new Dictionary<GameObject, Transform>();
        
        public void RegisterVisitor(GameObject visitor) {
            if (!visitorParent) {
                Debug.LogError($"{nameof(visitorParent)} is not set!");
                return;
            }

            if (_parentMap.ContainsKey(visitor) || _recentOrphanMap.ContainsKey(visitor)) {
                return;
            }

            _parentMap.Add(visitor, visitor.transform.parent);

            Debug.Log(visitor.transform.parent
                ? $"Registered: {visitor.name} (original parent: {visitor.transform.parent.name})"
                : $"Registered: {visitor.name}");

            visitor.transform.SetParent(visitorParent, true);
        }

        public void UnregisterVisitor(GameObject visitor) {
            if (!_parentMap.TryGetValue(visitor, out var parent)) {
                return;
            }

            visitor.transform.SetParent(parent);
            _parentMap.Remove(visitor);

            Debug.Log($"Unregistered: {visitor.name}");

            _recentOrphanMap.Add(visitor, parent);
            StartCoroutine(IOrphanCooldown(visitor));
        }

        private IEnumerator IOrphanCooldown(GameObject visitor) {
            yield return new WaitForSecondsRealtime(OrphanCooldown);
            _recentOrphanMap.Remove(visitor);
        }
    }
}
using System;
using UnityEngine;
using Starventure.Physics;
    
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Starventure.Physics {
    public class Orbitor : MonoBehaviour {
        private const float TwoPI = 6.2831853072f; 
        
        [SerializeField] private Rigidbody rb;
        
        [SerializeField] private float speed = 1.0f;
        public Transform orbitCenter;
        public Vector2 ellipseSize = Vector2.one;
        private Vector2 scaledEllipseSize;

        private float _theta;
        private float PeriodLength => TwoPI / speed;

        private void Start() {
            transform.parent = orbitCenter;
            scaledEllipseSize = new Vector2(ellipseSize.x / orbitCenter.lossyScale.x,
                ellipseSize.y / orbitCenter.lossyScale.z);
        }

        private void Update() {
            _theta += speed * Time.deltaTime;
            _theta %= PeriodLength;
            
            // (a*cos(x)+b, c*sin(x)+d)
            Vector3 sampled = new Vector3(scaledEllipseSize.x * Mathf.Cos(_theta),
                0,
                scaledEllipseSize.y * Mathf.Sin(_theta));

            transform.localPosition = sampled;
        }

        private void OnEnable() {
            if (!rb) {
                return;
            }
            
            rb.isKinematic = true;
        }
        
        private void OnDisable() {
            if (!rb) {
                return;
            }
            
            rb.isKinematic = false;
        }
    }
}

#if UNITY_EDITOR
namespace Starventure.Editor {
    [CustomEditor(typeof(Orbitor))]
    public class OrbitorEditor : UnityEditor.Editor {
        private void OnSceneGUI() {
            Orbitor t = target as Orbitor;
            
            if(!t.orbitCenter) {
                return;
            }
            
            Handles.color = Color.yellow;
            Handles.matrix = Matrix4x4.TRS(t.orbitCenter.position, t.orbitCenter.rotation,
                new Vector3(t.ellipseSize.x, 0, t.ellipseSize.y));
            Handles.DrawWireDisc(Vector3.zero, Vector3.up, 1.0f);
        }
    }
}
#endif
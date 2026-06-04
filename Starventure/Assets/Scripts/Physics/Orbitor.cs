using System;
using UnityEngine;
using Starventure.Physics;
    
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Starventure.Physics {
    public class Orbitor : MonoBehaviour { 
        private const float SpeedScale = 0.01f; 
        
        [SerializeField] private Rigidbody rb;
        
        [SerializeField] private float speed = 1.0f;
        public float startOffset = 0.0f;
        public Transform orbitCenter;
        public Vector2 ellipseSize = Vector2.one;
        private Vector2 _scaledEllipseSize;

        private float _theta;

        private void Start() {
            transform.parent = orbitCenter;
            _scaledEllipseSize = new Vector2(ellipseSize.x / orbitCenter.lossyScale.x,
                ellipseSize.y / orbitCenter.lossyScale.z);

            _theta = startOffset;
        }

        public Vector3 SampleRawPosition(float time) {
            return SampledScaledPosition(time, Vector2.one);
        }

        public Vector3 SampledScaledPosition(float time, Vector2 scale) {
            // (a*cos(x)+b, 0, c*sin(x)+d)
            return new Vector3(scale.x * Mathf.Cos(time),
                0,
                scale.y * Mathf.Sin(time)
            );
        }

        private void Update() {
            _theta += speed * SpeedScale * Time.deltaTime;
            transform.localPosition = SampledScaledPosition(_theta, _scaledEllipseSize);
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
                new Vector3(t.ellipseSize.x, 1, t.ellipseSize.y));
            Handles.DrawWireDisc(Vector3.zero, Vector3.up, 1.0f);

            Handles.matrix = Matrix4x4.TRS(Vector3.zero, t.orbitCenter.rotation, Vector3.one);
            Handles.DrawWireCube(t.SampledScaledPosition(t.startOffset, t.ellipseSize) + t.orbitCenter.position, Vector3.one);
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            if (GUILayout.Button("Place At Point")) {
                Orbitor t = target as Orbitor;

                Matrix4x4 transformMatrix = Matrix4x4.TRS(Vector3.zero, t.orbitCenter.rotation, t.transform.lossyScale);
                Vector3 sampledPoint = t.SampledScaledPosition(t.startOffset, t.ellipseSize) + t.orbitCenter.position;
                sampledPoint = transformMatrix.MultiplyPoint(sampledPoint);
                
                t.transform.position = sampledPoint;
            }
        }
    }
}
#endif
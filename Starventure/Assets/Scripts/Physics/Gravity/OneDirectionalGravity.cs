using System;
using UnityEngine;

#if UNITY_EDITOR
using Starventure.Physics.Gravity;
using UnityEditor;
#endif

namespace Starventure.Physics.Gravity {
    public class OneDirectionalGravity : CustomGravity {
        public override float CalculateGravityStrength(Vector3 objectPosition) {
            return 1;
        }
        
        public override float CalculateDampingMultiplier(Vector3 objectPosition) {
            return 1;
        }

        public override Vector3 CalculateGravityDirection(Vector3 objectPosition) {
            return root.forward;
        }
    }
}

#if UNITY_EDITOR

namespace Starventure.Editor {
    [CustomEditor(typeof(OneDirectionalGravity))]
    public class OneDirectionalGravityEditor : UnityEditor.Editor {
        private const float ArrowSize = 2;
        private const float OffsetScale = 5;

        private void OnSceneGUI() {
            OneDirectionalGravity gravity = target as OneDirectionalGravity;

            if (!gravity.root) {
                return;
            }

            Handles.color = Color.red;
            Handles.matrix = Matrix4x4.TRS(gravity.root.position, gravity.root.rotation, gravity.root.lossyScale);
            
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    Vector3 offset = new Vector3(i - 1, j - 1, 0) * OffsetScale;
                    offset.z = -ArrowSize;
                    
                    Handles.ArrowHandleCap(i + j,
                        offset,
                        Quaternion.identity,
                        ArrowSize,
                        EventType.Repaint);
                }
            }
        }
    }
}

#endif
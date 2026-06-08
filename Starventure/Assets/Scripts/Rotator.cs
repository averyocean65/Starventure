using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Rotator : MonoBehaviour {
    public Vector3 rotationAxis;
    public float speed = 1.0f;
    
    private void Update() {
        transform.Rotate(rotationAxis, Time.deltaTime * speed);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Rotator))]
public class RotatorEditor : UnityEditor.Editor {
    private void OnSceneGUI() {
        Rotator rotator = target as Rotator;

        Handles.color = Color.white;
        // Handles.ArrowHandleCap(1, rotator.transform.position, Quaternion.Euler(rotator.rotationAxis), 1,
        //     EventType.Repaint);
    }
}
#endif
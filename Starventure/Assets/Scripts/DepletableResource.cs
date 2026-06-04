using System;
using UnityEngine;
using UnityEngine.UI;

public class DepletableResource : MonoBehaviour {
    [SerializeField] private float maxValue = 1.0f;
    [SerializeField] private float depletionRate = 1.0f;
    [SerializeField] private float restorationRate = 1.0f;

    [SerializeField] private Slider uiSlider;
    
    public float Value { get; protected set; }
    public bool IsDepleting => _consumers > 0;
    public bool IsRestoring => _restorers > 0;
    public bool IsFull => Mathf.Approximately(Value, maxValue);
    public bool IsEmpty => Mathf.Approximately(Value, 0);
    
    private int _consumers = 0;
    private int _restorers = 0;
    

    public void Deplete(bool startDepleting = true) {
        if (startDepleting) {
            _consumers++;
            return;
        }
        
        if (_consumers == 0) {
            return;
        }

        _consumers--;
    }
    
    public void Restore(bool startRestoring = true) {
        if (startRestoring) {
            _restorers++;
            return;
        }

        if (_restorers == 0) {
            return;
        }

        _restorers--;
    }

    private void Start() {
        Value = maxValue;
        uiSlider.maxValue = maxValue;
        uiSlider.minValue = 0;
    }

    private void Update() {
        if (IsDepleting) {
            Value -= depletionRate * _consumers * Time.deltaTime;
        }

        if (IsRestoring) {
            Value += restorationRate * _restorers * Time.deltaTime;
        }

        Value = Mathf.Clamp(Value, 0, maxValue);
        uiSlider.value = Value;
    }

    public void InstantRestore() {
        Value = maxValue;
    }

    public void ResetCounters() {
        _consumers = 0;
        _restorers = 0;
    }
}

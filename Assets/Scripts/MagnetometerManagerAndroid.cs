using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class MagnetometerManagerAndroid : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float samplingFrequency = 60f;
    [SerializeField] private float magnetDetectionThreshold = 50f;

    [Header("Debug (Read Only)")]
    [SerializeField] private Vector3 rawMagneticField;
    [SerializeField] private Vector3 baselineField;
    [SerializeField] private Vector3 relativeField;
    [SerializeField] private float relativeMagnitude;
    [SerializeField] private string detectedPolarity = "None";
    [SerializeField] private bool isCalibrated = false;

    private MagneticFieldSensor sensor;
    private bool usingLegacyCompass = false;

    private Vector3 baselineAccumulator;
    private int baselineSampleCount = 0;
    private const int BASELINE_SAMPLES_NEEDED = 60;

    [Space, Header("UI Elements")]
    [SerializeField] TMP_Text MagnetPolarityText;
    [SerializeField] string TextContent;

    void OnEnable()
    {
        sensor = MagneticFieldSensor.current;

        if (sensor != null)
        {
            Debug.Log("Using InputSystem MagneticFieldSensor");
            InputSystem.EnableDevice(sensor);
            sensor.samplingFrequency = samplingFrequency;
        }
        else
        {
            Debug.Log("MagneticFieldSensor not found. Falling back to legacy compass."); 
            usingLegacyCompass = true;

            Input.compass.enabled = true;
        }
    }

    void OnDisable()
    {
        if (sensor != null)
            InputSystem.DisableDevice(sensor);
    }

    void Update()
    {
        if (!TryGetMagneticField(out rawMagneticField))
            return;

        if (!isCalibrated)
        {
            CollectBaseline();
            return;
        }

        relativeField = rawMagneticField - baselineField;
        relativeMagnitude = relativeField.magnitude;

        DetectPolarity();
        UpdateUI();
    }

    bool TryGetMagneticField(out Vector3 field)
    {
        field = Vector3.zero;

        if (!usingLegacyCompass && sensor != null && sensor.enabled)
        {
            field = sensor.magneticField.ReadValue();
            return true;
        }

        if (usingLegacyCompass && Input.compass.enabled)
        {
            field = Input.compass.rawVector;
            return true;
        }

        return false;
    }

    private void CollectBaseline()
    {
        baselineAccumulator += rawMagneticField;
        baselineSampleCount++;

        if (baselineSampleCount >= BASELINE_SAMPLES_NEEDED)
        {
            baselineField = baselineAccumulator / baselineSampleCount;
            isCalibrated = true;

            Debug.Log($"Calibration complete. Baseline: {baselineField} | Magnitude: {baselineField.magnitude:F1} µT");
        }
    }

    public void Recalibrate()
    {
        isCalibrated = false;
        baselineAccumulator = Vector3.zero;
        baselineSampleCount = 0;

        Debug.Log("Recalibrating baseline...");
    }

    private void UpdateUI()
    {
        TextContent = $"Field Magnitude: {relativeMagnitude:F1} µT\nPolarity: {detectedPolarity}";
        MagnetPolarityText.text = TextContent;
    }

    private void DetectPolarity()
    {
        if (relativeMagnitude < magnetDetectionThreshold)
        {
            detectedPolarity = "No magnet detected";
            return;
        }

        float dominantAxis = relativeField.z;

        detectedPolarity = dominantAxis > 0 ? "NORTH pole" : "SOUTH pole";

        Debug.Log($"Polarity: {detectedPolarity} | Relative magnitude: {relativeMagnitude:F1} µT");
    }
}
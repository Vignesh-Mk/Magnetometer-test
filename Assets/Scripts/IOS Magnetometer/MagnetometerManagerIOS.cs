using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MagnetometerManagerIOS : MonoBehaviour
{
    [SerializeField] TMP_Text VectorText;

    [Header("Prototype UI References")]
    [SerializeField] GameObject NorthPoleImage;
    [SerializeField] GameObject SouthPoleImage;

    [Space]

    [SerializeField] TMP_Text PolarityText;
    [SerializeField] TMP_Text IntensityText;

    float detectionThreshold = 100f;

    void Start()
    {
        NorthPoleImage.SetActive(false);
        SouthPoleImage.SetActive(false);

        MagnetometerIOS.Start();
    }

    void OnDestroy()
    {
        MagnetometerIOS.Stop();       
    }

    void Update()
    {
        Vector3 field = MagnetometerIOS.GetField();

        float intensity = field.magnitude;
        float gaussIntensity = intensity / 100; // 1 µt = 0.01G

        string magnetPolarity;

        if (intensity < detectionThreshold)
        {
            magnetPolarity = "No magnets detected";

            NorthPoleImage.SetActive(false);
            SouthPoleImage.SetActive(false);
        }
        else
        {
            if (field.z > 0)
            {
                magnetPolarity = "North Pole";

                NorthPoleImage.SetActive(true);
                SouthPoleImage.SetActive(false);
            }
            else
            {
                magnetPolarity = "South Pole";

                NorthPoleImage.SetActive(false);
                SouthPoleImage.SetActive(true);
            }
        }

        VectorText.text =
            $"Field Intensity: {intensity:F2} µT\nMagnet Polarity: {magnetPolarity}";

        PolarityText.text = $"{magnetPolarity}";

        //IntensityText.text = $"{intensity:F2} µT";
        IntensityText.text = $"{gaussIntensity:F2} G";
    }
}
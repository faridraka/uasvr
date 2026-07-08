using UnityEngine;

public class ForkliftIndicatorController : MonoBehaviour
{
    [Header("Indicator Objects")]
    public GameObject engineIndicator;
    public GameObject emergencyIndicator;
    public GameObject brakeIndicator;

    [Header("Test Indicator")]
    public bool engineOn = false;
    public bool emergencyOn = false;
    public bool brakeOn = true;

    void Update()
    {
        if (engineIndicator != null)
            engineIndicator.SetActive(engineOn);

        if (emergencyIndicator != null)
            emergencyIndicator.SetActive(emergencyOn);

        if (brakeIndicator != null)
            brakeIndicator.SetActive(brakeOn);
    }
}
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    [Header("Zones")]
    [SerializeField] private GameObject safetyZone;
    [SerializeField] private GameObject machineZone;
    [SerializeField] private GameObject targetZone;
    [SerializeField] private GameObject finishZone;

    private void Start()
    {
        if (TrainingManager.Instance != null)
        {
            TrainingManager.Instance.OnStepChanged += HandleStepChanged;

            // langsung sync dengan step saat ini
            HandleStepChanged(TrainingManager.Instance.currentStep);
        }
        else
        {
            Debug.LogError("TrainingManager.Instance belum ditemukan!");
        }
    }

    private void OnDestroy()
    {
        if (TrainingManager.Instance != null)
            TrainingManager.Instance.OnStepChanged -= HandleStepChanged;
    }

    private void HandleStepChanged(TrainingManager.TrainingStep step)
    {
        Debug.Log("ZoneManager menerima step: " + step);

        switch (step)
        {
            case TrainingManager.TrainingStep.SafetyCheck:
                SetZones(true, false, false, false);
                break;

            case TrainingManager.TrainingStep.EngineStart:
                SetZones(false, true, false, false);
                TrainingManager.Instance.SetPlayerInSafeZone(false);
                break;

            case TrainingManager.TrainingStep.Operate:
            case TrainingManager.TrainingStep.MoveLoad:
                SetZones(false, true, true, false);
                break;

            case TrainingManager.TrainingStep.StopMachine:
                SetZones(false, true, false, false);
                break;

            case TrainingManager.TrainingStep.Complete:
                SetZones(false, false, false, true);
                break;
        }
    }

    private void SetZones(bool safety, bool machine, bool target, bool finish)
    {
        SetZone(safetyZone, safety);
        SetZone(machineZone, machine);
        SetZone(targetZone, target);
        SetZone(finishZone, finish);
    }

    private void SetZone(GameObject zone, bool active)
    {
        if (zone != null)
            zone.SetActive(active);
    }
}

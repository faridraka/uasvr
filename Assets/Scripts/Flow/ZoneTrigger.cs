using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    public enum ZoneType
    {
        SafetyZone,
        DangerZone,
        TargetArea,
        MachineArea,
        FinishZone
    }

    [Header("Setting Zone")]
    public ZoneType zoneType;

    [Tooltip("Khusus TargetArea: index target 0/1/2, buat dicocokin sama AdvancedLoadTask")]
    public int targetIndex = 0;

    void OnTriggerEnter(Collider other)
    {
        if (zoneType == ZoneType.SafetyZone && other.CompareTag("Player"))
        {
            TrainingManager.Instance.SetPlayerInSafeZone(true);
            Debug.Log("Player masuk Safety Zone");
        }

        if (zoneType == ZoneType.MachineArea && other.CompareTag("Player"))
        {
            TrainingManager.Instance.SetPlayerInMachineArea(true);
            Debug.Log("Player masuk Machine Area");
        }

        if (zoneType == ZoneType.DangerZone)
        {
            if (other.CompareTag("Player"))
                TrainingManager.Instance.ReportDangerZoneEntered("Player");
            else if (other.CompareTag("Load"))
                TrainingManager.Instance.ReportDangerZoneEntered("Load");
        }

        if (zoneType == ZoneType.TargetArea && other.CompareTag("Load"))
        {
            LoadCargo load = other.GetComponent<LoadCargo>();

            if (load != null)
                load.OnEnterTargetArea(targetIndex);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (zoneType == ZoneType.SafetyZone && other.CompareTag("Player"))
        {
            TrainingManager.Instance.SetPlayerInSafeZone(false);
            Debug.Log("Player keluar Safety Zone");
        }

        if (zoneType == ZoneType.MachineArea && other.CompareTag("Player"))
        {
            TrainingManager.Instance.SetPlayerInMachineArea(false);
            Debug.Log("Player keluar Machine Area");
        }
    }
}

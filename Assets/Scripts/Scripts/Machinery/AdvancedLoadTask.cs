using UnityEngine;
using System.Collections.Generic;

// [BONUS] Nyimpen data 3 tipe crate berbeda dan mastiin tiap crate nyampe target yang cocok.
public class AdvancedLoadTask : MonoBehaviour
{
    public int totalTargets = 3;
    private HashSet<int> deliveredTargets = new HashSet<int>();

    public void CheckDelivery(LoadCargo cargo, int targetIndex)
    {
        if (cargo.cargoTypeIndex == targetIndex)
        {
            deliveredTargets.Add(targetIndex);
            Debug.Log($"[AdvancedLoadTask] Crate tipe {cargo.cargoTypeIndex} terkirim ke target {targetIndex} dengan benar.");

            if (deliveredTargets.Count >= totalTargets)
            {
                TrainingManager.Instance.ReportLoadDelivered();
            }
        }
        else
        {
            TrainingManager.Instance.ReportDangerZoneEntered("Crate salah target");
        }
    }
}

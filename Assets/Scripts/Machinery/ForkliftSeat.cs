using UnityEngine;

public class ForkliftSeat : InteractableObject
{
    [SerializeField] private ForkliftDriveController driveController;
    [SerializeField] private bool requireMachineArea = true;

    protected override void Start()
    {
        base.Start();

        if (driveController == null)
            driveController = GetComponent<ForkliftDriveController>();

        if (driveController == null)
            driveController = GetComponentInParent<ForkliftDriveController>();

        if (driveController == null)
            driveController = GetComponentInChildren<ForkliftDriveController>();
    }

    public override void Interact(GameObject interactor)
    {
        if (requireMachineArea && TrainingManager.Instance != null && !TrainingManager.Instance.isInMachineArea)
        {
            TrainingManager.Instance.ShowWarning("Masuk ke Machine Area dulu sebelum naik forklift.");
            return;
        }

        if (driveController == null)
        {
            Debug.LogWarning("ForkliftSeat belum menemukan ForkliftDriveController.");
            return;
        }

        if (driveController.IsPlayerInside()) return;

        driveController.EnterVehicle(interactor);
        FlashHighlight();
    }

    public override string GetPrompt()
    {
        if (requireMachineArea && TrainingManager.Instance != null && !TrainingManager.Instance.isInMachineArea)
            return "Masuk Machine Area untuk naik forklift";

        if (driveController.IsPlayerInside())
            return "Tekan X untuk keluar forklift";

        return "Tekan E untuk naik forklift";
    }
}

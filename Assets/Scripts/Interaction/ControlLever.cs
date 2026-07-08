using UnityEngine;

// Tuas kontrol buat gerakin garpu/lengan alat berat. Hubungkan ke HeavyMachineryController
// lewat Inspector (drag object yang punya script itu).
public class ControlLever : InteractableObject
{
    public HeavyMachineryController machineryController;

    public override void Interact(GameObject interactor)
    {
        TrainingManager.Instance.OperateControl();

        if (machineryController != null)
            machineryController.ActivateLift();

        FlashHighlight();
        if (AudioManager.Instance != null) AudioManager.Instance.PlayLever();
    }
}

using UnityEngine;

// Tombol darurat. Sekali ditekan, semua kontrol mesin langsung dikunci lewat TrainingManager.
public class EmergencyStopButton : InteractableObject
{
    public override void Interact(GameObject interactor)
    {
        TrainingManager.Instance.TriggerEmergencyStop();
        FlashHighlight();
        if (AudioManager.Instance != null) AudioManager.Instance.PlayWarningBeep();
    }

    public override string GetPrompt() => "TEKAN E UNTUK EMERGENCY STOP";
}

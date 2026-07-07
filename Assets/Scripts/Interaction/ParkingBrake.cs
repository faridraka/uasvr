using UnityEngine;

// Rem parkir - lebih ke feedback prosedural tambahan, ga wajib nge-block flow utama.
public class ParkingBrake : InteractableObject
{
    private bool isEngaged = false;

    public override void Interact(GameObject interactor)
    {
        isEngaged = !isEngaged;
        FlashHighlight();
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClick();
    }

    public override string GetPrompt()
    {
        return isEngaged ? "Tekan E untuk lepas Parking Brake" : "Tekan E untuk Parking Brake";
    }
}

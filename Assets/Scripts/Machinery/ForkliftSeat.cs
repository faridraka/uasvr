using UnityEngine;

// Tempel di collider kursi/tangga forklift (bukan raycast dari player biasa, tapi objek yang diinteract
// buat NAIK ke forklift). Habis ini kamera otomatis pindah ke cockpit view.
public class ForkliftSeat : InteractableObject
{
    public ForkliftDriveController driveController;

    public override void Interact(GameObject interactor)
    {
        if (driveController == null) return;
        if (driveController.IsPlayerInside()) return;

        driveController.EnterVehicle(interactor);
        FlashHighlight();
    }

    public override string GetPrompt() => "Tekan E untuk naik forklift";
}
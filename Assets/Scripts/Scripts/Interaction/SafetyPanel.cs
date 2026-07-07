using UnityEngine;

// Panel safety check. Player interact di sini dulu sebelum boleh nyalain mesin.
public class SafetyPanel : InteractableObject
{
    public override void Interact(GameObject interactor)
    {
        TrainingManager.Instance.CompleteSafetyCheck();
        FlashHighlight();
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClick();
    }
}

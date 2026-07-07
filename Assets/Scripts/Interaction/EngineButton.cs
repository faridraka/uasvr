using UnityEngine;

// Tombol start/stop engine. Ditekan sekali = start, ditekan lagi = coba stop
// (tapi TrainingManager yang validasi apakah boleh stop atau belum).
public class EngineButton : InteractableObject
{
    public override void Interact(GameObject interactor)
    {
        if (!TrainingManager.Instance.isEngineOn)
        {
            TrainingManager.Instance.TryStartEngine();
            if (TrainingManager.Instance.isEngineOn && AudioManager.Instance != null)
                AudioManager.Instance.PlayEngineStart();
        }
        else
        {
            bool wasOn = TrainingManager.Instance.isEngineOn;
            TrainingManager.Instance.TryStopEngine();
            if (wasOn && !TrainingManager.Instance.isEngineOn && AudioManager.Instance != null)
                AudioManager.Instance.PlayEngineStop();
        }
        FlashHighlight();
    }

    public override string GetPrompt()
    {
        return TrainingManager.Instance.isEngineOn ? "Tekan E untuk Stop Machine" : "Tekan E untuk Start Engine";
    }
}

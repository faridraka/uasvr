using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SafetyPanel : InteractableObject
{
    [Header("Safety Toggles")]
    [SerializeField] private Toggle helmToggle;
    [SerializeField] private Toggle rompiToggle;
    [SerializeField] private Toggle sarungTanganToggle;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI statusText;

    private bool helmDone;
    private bool rompiDone;
    private bool sarungTanganDone;

    protected override void Start()
    {
        base.Start();

        statusText.text = "Atribut keselamatan belum terdeteksi";

        helmToggle.onValueChanged.AddListener(OnHelmToggle);
        rompiToggle.onValueChanged.AddListener(OnRompiToggle);
        sarungTanganToggle.onValueChanged.AddListener(OnSarungTanganToggle);
    }

    private bool CanUseSafetyPanel(Toggle toggle)
    {
        if (TrainingManager.Instance.isPlayerInSafeZone)
            return true;

        toggle.SetIsOnWithoutNotify(false);
        TrainingManager.Instance.ShowWarning("Masuk ke Safe Zone terlebih dahulu.");

        return false;
    }

    private void OnHelmToggle(bool isOn)
    {
        if (!isOn || helmDone) return;
        if (!CanUseSafetyPanel(helmToggle)) return;

        helmDone = true;
        helmToggle.interactable = false;

        statusText.text = "Helm terpasang";
        PlayClick();
        CheckAllSafetyCompleted();
    }

    private void OnRompiToggle(bool isOn)
    {
        if (!isOn || rompiDone) return;
        if (!CanUseSafetyPanel(rompiToggle)) return;

        rompiDone = true;
        rompiToggle.interactable = false;

        statusText.text = "Rompi terpasang";
        PlayClick();
        CheckAllSafetyCompleted();
    }

    private void OnSarungTanganToggle(bool isOn)
    {
        if (!isOn || sarungTanganDone) return;
        if (!CanUseSafetyPanel(sarungTanganToggle)) return;

        sarungTanganDone = true;
        sarungTanganToggle.interactable = false;

        statusText.text = "Sarung tangan terpasang";
        PlayClick();
        CheckAllSafetyCompleted();
    }

    private void CheckAllSafetyCompleted()
    {
        if (helmDone && rompiDone && sarungTanganDone)
        {
            statusText.text = "Semua atribut keselamatan sudah terpasang";

            TrainingManager.Instance.CompleteSafetyCheck();
            FlashHighlight();
        }
    }

    public override void Interact(GameObject interactor)
    {
        FlashHighlight();
    }

    private void PlayClick()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayClick();
    }
}

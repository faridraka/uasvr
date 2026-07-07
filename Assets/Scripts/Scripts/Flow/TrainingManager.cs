using System;
using UnityEngine;

// TrainingManager = otak utama game. Semua step, validasi urutan, dan event ada di sini.
// Script lain (UI, Audio, Interactable) tinggal subscribe ke event2 ini, ga perlu saling refer langsung.
public class TrainingManager : MonoBehaviour
{
    public static TrainingManager Instance { get; private set; }

    public enum TrainingStep
    {
        NotStarted,
        SafetyCheck,
        EngineStart,
        Operate,
        MoveLoad,
        StopMachine,
        Complete
    }

    [Header("State Saat Ini (buat debug di Inspector)")]
    public TrainingStep currentStep = TrainingStep.NotStarted;

    [Header("Status Flags")]
    public bool isPlayerInSafeZone = false;
    public bool isSafetyCheckDone = false;
    public bool isEngineOn = false;
    public bool isInMachineArea = false;
    public bool isLoadDelivered = false;
    public bool isEmergencyStopped = false;

    // Event2 ini yang bikin script lain (UI, Audio, dll) bisa "dengerin" perubahan tanpa saling nempel
    public event Action<TrainingStep> OnStepChanged;
    public event Action<string> OnWarning;
    public event Action OnMistake;
    public event Action OnTrainingComplete;
    public event Action OnEmergencyStop;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        ChangeStep(TrainingStep.SafetyCheck);
    }

    void ChangeStep(TrainingStep newStep)
    {
        currentStep = newStep;
        OnStepChanged?.Invoke(currentStep);
        Debug.Log("[TrainingManager] Step berubah ke: " + newStep);
    }

    // Dipanggil dari SafetyPanel.cs
    public void CompleteSafetyCheck()
    {
        if (isEmergencyStopped) return;

        if (!isPlayerInSafeZone)
        {
            OnWarning?.Invoke("Kamu harus berada di Safe Zone dulu sebelum safety check!");
            return;
        }

        isSafetyCheckDone = true;
        ChangeStep(TrainingStep.EngineStart);
    }

    // Dipanggil dari EngineButton.cs
    public void TryStartEngine()
    {
        if (isEmergencyStopped) return;

        if (!isSafetyCheckDone)
        {
            OnWarning?.Invoke("Safety Check belum selesai! Engine tidak bisa dinyalakan.");
            OnMistake?.Invoke();
            return;
        }

        if (!isInMachineArea)
        {
            OnWarning?.Invoke("Kamu harus berada di area operator/cockpit dulu.");
            return;
        }

        isEngineOn = true;
        ChangeStep(TrainingStep.Operate);
    }

    // Dipanggil dari ControlLever.cs
    public void OperateControl()
    {
        if (isEmergencyStopped) return;

        if (!isEngineOn)
        {
            OnWarning?.Invoke("Mesin belum menyala!");
            OnMistake?.Invoke();
            return;
        }

        if (currentStep == TrainingStep.Operate)
        {
            ChangeStep(TrainingStep.MoveLoad);
        }
    }

    // Dipanggil dari AdvancedLoadTask.cs atau LoadCargo.cs kalau ga pakai advanced task
    public void ReportLoadDelivered()
    {
        if (isEmergencyStopped) return;

        isLoadDelivered = true;
        if (currentStep == TrainingStep.MoveLoad)
        {
            ChangeStep(TrainingStep.StopMachine);
        }
    }

    // Dipanggil dari EngineButton.cs pas ditekan lagi buat matiin mesin
    public void TryStopEngine()
    {
        if (isEmergencyStopped) return;

        if (!isLoadDelivered)
        {
            OnWarning?.Invoke("Load belum sampai target, jangan matikan mesin dulu.");
            OnMistake?.Invoke();
            return;
        }

        isEngineOn = false;
        ChangeStep(TrainingStep.Complete);
        OnTrainingComplete?.Invoke();
    }

    // Dipanggil dari ZoneTrigger.cs
    public void SetPlayerInSafeZone(bool value) => isPlayerInSafeZone = value;
    public void SetPlayerInMachineArea(bool value) => isInMachineArea = value;

    public void ReportDangerZoneEntered(string what)
    {
        OnWarning?.Invoke(what + " memasuki Danger Zone!");
        OnMistake?.Invoke();
    }

    // Dipanggil dari EmergencyStopButton.cs
    public void TriggerEmergencyStop()
    {
        isEmergencyStopped = true;
        isEngineOn = false;
        OnEmergencyStop?.Invoke();
        OnWarning?.Invoke("EMERGENCY STOP AKTIF! Semua kontrol dikunci.");
    }

    public void ResetEmergencyStop()
    {
        isEmergencyStopped = false;
    }
}

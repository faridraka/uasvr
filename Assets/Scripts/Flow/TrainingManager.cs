using System;
using UnityEngine;

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

    [Header("State Saat Ini")]
    public TrainingStep currentStep = TrainingStep.NotStarted;

    [Header("Status Flags")]
    public bool isPlayerInSafeZone = false;
    public bool isSafetyCheckDone = false;
    public bool isEngineOn = false;
    public bool isInMachineArea = false;
    public bool isLoadDelivered = false;
    public bool isEmergencyStopped = false;

    [ContextMenu("DEBUG - Set EngineStart")]
    public void DebugSetEngineStart()
    {
        ChangeStep(TrainingStep.EngineStart);
    }

    [ContextMenu("DEBUG - Set Operate")]
    public void DebugSetOperate()
    {
        ChangeStep(TrainingStep.Operate);
    }

    [ContextMenu("DEBUG - Set MoveLoad")]
    public void DebugSetMoveLoad()
    {
        ChangeStep(TrainingStep.MoveLoad);
    }

    [ContextMenu("DEBUG - Set StopMachine")]
    public void DebugSetStopMachine()
    {
        ChangeStep(TrainingStep.StopMachine);
    }

    [ContextMenu("DEBUG - Set Complete")]
    public void DebugSetComplete()
    {
        ChangeStep(TrainingStep.Complete);
    }

    public event Action<TrainingStep> OnStepChanged;
    public event Action<string> OnWarning;
    public event Action OnMistake;
    public event Action OnTrainingComplete;
    public event Action OnEmergencyStop;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

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

    public void ShowWarning(string message)
    {
        OnWarning?.Invoke(message);
    }

    public void CompleteSafetyCheck()
    {
        if (isEmergencyStopped) return;

        if (!isPlayerInSafeZone)
        {
            ShowWarning("Kamu harus berada di Safe Zone dulu sebelum safety check!");
            return;
        }

        isSafetyCheckDone = true;
        ChangeStep(TrainingStep.EngineStart);
    }

    public void TryStartEngine()
    {
        if (isEmergencyStopped) return;

        if (!isSafetyCheckDone)
        {
            ShowWarning("Safety Check belum selesai! Engine tidak bisa dinyalakan.");
            OnMistake?.Invoke();
            return;
        }

        if (!isInMachineArea)
        {
            ShowWarning("Kamu harus berada di area operator/cockpit dulu.");
            return;
        }

        isEngineOn = true;
        ChangeStep(TrainingStep.Operate);
    }

    public void OperateControl()
    {
        if (isEmergencyStopped) return;

        if (!isEngineOn)
        {
            ShowWarning("Mesin belum menyala!");
            OnMistake?.Invoke();
            return;
        }

        if (currentStep == TrainingStep.Operate)
        {
            ChangeStep(isLoadDelivered ? TrainingStep.StopMachine : TrainingStep.MoveLoad);
        }
    }

    public void ReportLoadDelivered()
    {
        if (isEmergencyStopped) return;

        isLoadDelivered = true;

        if (currentStep == TrainingStep.MoveLoad)
        {
            ChangeStep(TrainingStep.StopMachine);
        }
    }

    public void TryStopEngine()
    {
        if (isEmergencyStopped) return;

        if (!isInMachineArea)
        {
            ShowWarning("Kamu harus berada di Machine Area untuk stop machine.");
            OnMistake?.Invoke();
            return;
        }

        if (!isLoadDelivered)
        {
            ShowWarning("Load belum sampai target, jangan matikan mesin dulu.");
            OnMistake?.Invoke();
            return;
        }

        isEngineOn = false;
        CompleteStopMachineIfReady();
    }

    public void SetPlayerInSafeZone(bool value)
    {
        isPlayerInSafeZone = value;
    }

    public void SetPlayerInMachineArea(bool value)
    {
        isInMachineArea = value;

        if (value)
            CompleteStopMachineIfReady();
    }

    private void CompleteStopMachineIfReady()
    {
        if (isEmergencyStopped) return;
        if (!isLoadDelivered) return;
        if (isEngineOn) return;
        if (!isInMachineArea) return;
        if (currentStep == TrainingStep.Complete) return;

        ChangeStep(TrainingStep.Complete);
        OnTrainingComplete?.Invoke();
    }

    public void ReportDangerZoneEntered(string what)
    {
        ShowWarning(what + " memasuki Danger Zone!");
        OnMistake?.Invoke();
    }

    public void TriggerEmergencyStop()
    {
        isEmergencyStopped = true;
        isEngineOn = false;

        OnEmergencyStop?.Invoke();
        ShowWarning("EMERGENCY STOP AKTIF! Semua kontrol dikunci.");
    }

    public void ResetEmergencyStop()
    {
        isEmergencyStopped = false;
    }
}

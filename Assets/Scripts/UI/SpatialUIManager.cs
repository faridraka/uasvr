using UnityEngine;
using TMPro;

// Nempel di World Space Canvas. Ngatur checklist 4 langkah, status mesin, status safety, dan warning.
public class SpatialUIManager : MonoBehaviour
{
    [Header("Checklist Texts (4 langkah)")]
    public TMP_Text checklistSafetyCheck;
    public TMP_Text checklistEngineStart;
    public TMP_Text checklistMoveLoad;
    public TMP_Text checklistStopMachine;

    [Header("Status Texts")]
    public TMP_Text engineStatusText;
    public TMP_Text safetyStatusText;
    public TMP_Text warningText;
    public TMP_Text endStateText;

    void OnEnable()
    {
        TrainingManager.Instance.OnStepChanged += UpdateChecklist;
        TrainingManager.Instance.OnWarning += ShowWarning;
        TrainingManager.Instance.OnTrainingComplete += ShowComplete;
    }

    void OnDisable()
    {
        if (TrainingManager.Instance == null) return;
        TrainingManager.Instance.OnStepChanged -= UpdateChecklist;
        TrainingManager.Instance.OnWarning -= ShowWarning;
        TrainingManager.Instance.OnTrainingComplete -= ShowComplete;
    }

    void Update()
    {
        if (TrainingManager.Instance == null) return;
        if (safetyStatusText != null)
            safetyStatusText.text = TrainingManager.Instance.isPlayerInSafeZone ? "Safe Zone" : "Safety Check Required";
        if (engineStatusText != null)
            engineStatusText.text = TrainingManager.Instance.isEngineOn ? "Engine On - Operating" : "Engine Off";
    }

    void UpdateChecklist(TrainingManager.TrainingStep step)
    {
        SetDone(checklistSafetyCheck, step >= TrainingManager.TrainingStep.EngineStart);
        SetDone(checklistEngineStart, step >= TrainingManager.TrainingStep.Operate);
        SetDone(checklistMoveLoad, step >= TrainingManager.TrainingStep.StopMachine);
        SetDone(checklistStopMachine, step >= TrainingManager.TrainingStep.Complete);
    }

    void SetDone(TMP_Text text, bool done)
    {
        if (text == null) return;
        text.color = done ? Color.green : Color.white;
        text.fontStyle = done ? FontStyles.Strikethrough : FontStyles.Normal;
    }

    void ShowWarning(string msg)
    {
        if (warningText == null) return;
        warningText.text = msg;
        CancelInvoke(nameof(ClearWarning));
        Invoke(nameof(ClearWarning), 2.5f);
    }

    void ClearWarning()
    {
        if (warningText != null) warningText.text = "";
    }

    void ShowComplete()
    {
        if (endStateText != null) endStateText.text = "Training Complete";
    }
}

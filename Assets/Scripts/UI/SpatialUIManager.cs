using UnityEngine;
using TMPro;
using System.Collections;

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
    public TMP_Text endStateText;

    [Header("Prompt / Warning Text")]
    public TMP_Text messageText;

    private Coroutine warningRoutine;

    void OnEnable()
    {
        if (TrainingManager.Instance == null) return;

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

    void Start()
    {
        if (messageText != null)
            messageText.text = "";

        if (endStateText != null)
            endStateText.text = "";
    }

    void Update()
    {
        if (TrainingManager.Instance == null) return;

        if (safetyStatusText != null)
        {
            if (TrainingManager.Instance.isSafetyCheckDone)
                safetyStatusText.text = "Safety Check Complete";
            else if (TrainingManager.Instance.isPlayerInSafeZone)
                safetyStatusText.text = "Safe Zone";
            else
                safetyStatusText.text = "Safety Check Required";
        }

        if (engineStatusText != null)
        {
            engineStatusText.text = TrainingManager.Instance.isEngineOn
                ? "Engine On - Operating"
                : "Engine Off";
        }
    }

    public void ShowPrompt(string text)
    {
        if (messageText == null) return;
        if (warningRoutine != null) return;

        messageText.color = Color.white;
        messageText.text = text;
    }

    public void ClearPrompt()
    {
        if (messageText == null) return;
        if (warningRoutine != null) return;

        messageText.text = "";
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
        if (messageText == null) return;

        if (warningRoutine != null)
            StopCoroutine(warningRoutine);

        warningRoutine = StartCoroutine(WarningRoutine(msg));
    }

    IEnumerator WarningRoutine(string msg)
    {
        messageText.color = Color.red;
        messageText.text = msg;

        yield return new WaitForSeconds(2.5f);

        messageText.color = Color.white;
        messageText.text = "";

        warningRoutine = null;
    }

    void ShowComplete()
    {
        if (endStateText != null)
            endStateText.text = "Training Complete";
    }
}

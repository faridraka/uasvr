using UnityEngine;
using TMPro;
using System.Collections;

public class TrainingUI : MonoBehaviour
{
    [SerializeField] private TMP_Text messageText;

    private Coroutine warningRoutine;

    void Start()
    {
        if (messageText != null)
            messageText.text = "";

        TrainingManager.Instance.OnWarning += ShowWarning;
    }

    void OnDestroy()
    {
        if (TrainingManager.Instance != null)
            TrainingManager.Instance.OnWarning -= ShowWarning;
    }

    public void ShowPrompt(string text)
    {
        if (warningRoutine != null) return;

        messageText.color = Color.white;
        messageText.text = text;
    }

    public void ClearPrompt()
    {
        if (warningRoutine != null) return;

        messageText.text = "";
    }

    private void ShowWarning(string warning)
    {
        if (warningRoutine != null)
            StopCoroutine(warningRoutine);

        warningRoutine = StartCoroutine(WarningRoutine(warning));
    }

    private IEnumerator WarningRoutine(string warning)
    {
        messageText.color = Color.red;
        messageText.text = warning;

        yield return new WaitForSeconds(2f);

        messageText.color = Color.white;
        messageText.text = "";

        warningRoutine = null;
    }
}

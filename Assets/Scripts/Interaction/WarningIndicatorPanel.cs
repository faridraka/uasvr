using UnityEngine;
using System.Collections;

public class WarningIndicatorPanel : MonoBehaviour
{
    [Header("Indicator")]
    public Light indicatorLight;

    public Color safeColor = Color.green;
    public Color warningColor = Color.red;

    private void OnEnable()
    {
        if (TrainingManager.Instance != null)
        {
            TrainingManager.Instance.OnWarning += HandleWarning;
        }
        else
        {
            Debug.LogWarning("TrainingManager tidak ditemukan di Scene!");
        }

        if (indicatorLight != null)
        {
            indicatorLight.color = safeColor;
        }
    }

    private void OnDisable()
    {
        if (TrainingManager.Instance != null)
        {
            TrainingManager.Instance.OnWarning -= HandleWarning;
        }
    }

    private void HandleWarning(string message)
    {
        StopAllCoroutines();
        StartCoroutine(FlashRed());
    }

    private IEnumerator FlashRed()
    {
        if (indicatorLight == null)
            yield break;

        indicatorLight.color = warningColor;

        yield return new WaitForSeconds(1f);

        indicatorLight.color = safeColor;
    }
}

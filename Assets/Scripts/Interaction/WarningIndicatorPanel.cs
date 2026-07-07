using UnityEngine;
using System.Collections;

// Bukan interactable, ini panel visual yang otomatis nyala merah tiap kali ada warning
// dari TrainingManager (misal salah urutan, masuk danger zone, dll).
public class WarningIndicatorPanel : MonoBehaviour
{
    public Light indicatorLight;
    public Color safeColor = Color.green;
    public Color warningColor = Color.red;

    void OnEnable()
    {
        TrainingManager.Instance.OnWarning += HandleWarning;
    }

    void OnDisable()
    {
        if (TrainingManager.Instance == null) return;
        TrainingManager.Instance.OnWarning -= HandleWarning;
    }

    void HandleWarning(string msg)
    {
        StopAllCoroutines();
        StartCoroutine(FlashRed());
    }

    IEnumerator FlashRed()
    {
        if (indicatorLight != null) indicatorLight.color = warningColor;
        yield return new WaitForSeconds(1f);
        if (indicatorLight != null) indicatorLight.color = safeColor;
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Screen Space Overlay - layar berkedip merah pas ada mistake/danger.
public class MetaOverlayFX : MonoBehaviour
{
    public Image flashOverlay;
    public float flashDuration = 0.3f;

    void OnEnable()
    {
        TrainingManager.Instance.OnMistake += FlashRed;
    }

    void OnDisable()
    {
        if (TrainingManager.Instance == null) return;
        TrainingManager.Instance.OnMistake -= FlashRed;
    }

    void FlashRed()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        if (flashOverlay == null) yield break;
        Color c = flashOverlay.color;
        c.a = 0.4f;
        flashOverlay.color = c;
        yield return new WaitForSeconds(flashDuration);
        c.a = 0f;
        flashOverlay.color = c;
    }
}

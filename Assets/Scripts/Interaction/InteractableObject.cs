using UnityEngine;
using System.Collections;

// Base class abstract, tinggal di-extend sama SafetyPanel, EngineButton, dll.
// Udah include highlight effect (ganti warna sebentar pas kena interact).
public abstract class InteractableObject : MonoBehaviour, IInteractable
{
    [Header("Prompt Text")]
    public string promptMessage = "Tekan E untuk berinteraksi";

    [Header("Highlight (opsional, boleh dikosongin)")]
    public Renderer objectRenderer;
    public Color highlightColor = Color.yellow;
    private Color originalColor;

    protected virtual void Start()
    {
        if (objectRenderer != null)
            originalColor = objectRenderer.material.color;
    }

    public virtual string GetPrompt() => promptMessage;

    public abstract void Interact(GameObject interactor);

    protected void FlashHighlight()
    {
        if (objectRenderer == null) return;
        StopAllCoroutines();
        StartCoroutine(HighlightRoutine());
    }

    private IEnumerator HighlightRoutine()
    {
        objectRenderer.material.color = highlightColor;
        yield return new WaitForSeconds(0.3f);
        objectRenderer.material.color = originalColor;
    }
}

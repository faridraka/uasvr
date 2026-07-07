using UnityEngine;
using TMPro;

// Tempel di kamera player. Nembakin raycast ke depan, deteksi objek IInteractable,
// munculin prompt text, dan panggil Interact() pas E ditekan.
public class InteractionRaycast : MonoBehaviour
{
    public float interactRange = 3f;
    public LayerMask interactableLayer;
    public TMP_Text promptText;
    public KeyCode interactKey = KeyCode.E;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                if (promptText != null)
                {
                    promptText.gameObject.SetActive(true);
                    promptText.text = interactable.GetPrompt();
                }

                if (Input.GetKeyDown(interactKey))
                {
                    interactable.Interact(gameObject);
                }
                return;
            }
        }

        if (promptText != null) promptText.gameObject.SetActive(false);
    }
}

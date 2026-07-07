using UnityEngine;

public class InteractionRaycast : MonoBehaviour
{
    [Header("Interaction")]
    public float interactRange = 3f;
    public LayerMask interactableLayer;
    public KeyCode interactKey = KeyCode.E;

    [Header("UI")]
    [SerializeField] private TrainingUI trainingUI;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer))
        {
            IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();

            if (interactable != null)
            {
                trainingUI.ShowPrompt(interactable.GetPrompt());

                if (Input.GetKeyDown(interactKey))
                {
                    interactable.Interact(gameObject);
                }

                return;
            }
        }

        trainingUI.ClearPrompt();
    }
}

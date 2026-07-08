using UnityEngine;

public class InteractionRaycast : MonoBehaviour
{
    [Header("Interaction")]
    public float interactRange = 3f;
    public LayerMask interactableLayer;
    public KeyCode interactKey = KeyCode.E;

    [Header("UI")]
    [SerializeField] private SpatialUIManager spatialUI;

    [SerializeField] private GameObject interactorRoot;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer))
        {
            IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();

            if (interactable != null)
            {
                if (spatialUI != null)
                    spatialUI.ShowPrompt(interactable.GetPrompt());

                if (Input.GetKeyDown(interactKey))
                {
                    GameObject interactor = interactorRoot != null ? interactorRoot : gameObject;
                    interactable.Interact(interactor);
                }

                return;
            }
        }

        ForkliftDriveController activeVehicle = ForkliftDriveController.ActiveVehicle;

        if (activeVehicle != null && activeVehicle.IsPlayerInside())
        {
            if (spatialUI != null)
                spatialUI.ShowPrompt(activeVehicle.GetExitPrompt());

            if (Input.GetKeyDown(activeVehicle.exitKey))
                activeVehicle.ExitVehicle();

            return;
        }

        if (spatialUI != null)
            spatialUI.ClearPrompt();
    }
}

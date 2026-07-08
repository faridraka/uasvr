using UnityEngine;

public class ForkliftDriveController : MonoBehaviour
{
    public static ForkliftDriveController ActiveVehicle { get; private set; }

    [Header("Point")]
    public Transform driverSeatPoint;
    public Transform exitPoint;

    [Header("Input")]
    public KeyCode exitKey = KeyCode.X;

    private GameObject currentPlayer;
    private PlayerController playerController;
    private CharacterController characterController;
    private Transform originalPlayerParent;
    private Vector3 originalPlayerLocalPosition;
    private Quaternion originalPlayerLocalRotation;

    private bool isPlayerInside;

    public void EnterVehicle(GameObject player)
    {
        if (isPlayerInside) return;

        playerController = player.GetComponent<PlayerController>();
        if (playerController == null)
            playerController = player.GetComponentInParent<PlayerController>();

        characterController = player.GetComponent<CharacterController>();
        if (characterController == null)
            characterController = player.GetComponentInParent<CharacterController>();

        currentPlayer = player;

        if (playerController != null)
            currentPlayer = playerController.gameObject;
        else if (characterController != null)
            currentPlayer = characterController.gameObject;

        originalPlayerParent = currentPlayer.transform.parent;
        originalPlayerLocalPosition = currentPlayer.transform.localPosition;
        originalPlayerLocalRotation = currentPlayer.transform.localRotation;

        if (playerController != null)
            playerController.SetMovementEnabled(false);

        if (characterController != null)
            characterController.enabled = false;

        if (driverSeatPoint != null)
        {
            currentPlayer.transform.SetParent(driverSeatPoint);
            currentPlayer.transform.localPosition = Vector3.zero;
            currentPlayer.transform.localRotation = Quaternion.identity;
        }

        isPlayerInside = true;
        ActiveVehicle = this;
    }

    public void ExitVehicle()
    {
        if (!isPlayerInside) return;

        if (currentPlayer != null)
        {
            currentPlayer.transform.SetParent(originalPlayerParent);

            if (exitPoint != null)
            {
                currentPlayer.transform.position = exitPoint.position;
                currentPlayer.transform.rotation = exitPoint.rotation;
            }
            else
            {
                currentPlayer.transform.localPosition = originalPlayerLocalPosition;
                currentPlayer.transform.localRotation = originalPlayerLocalRotation;
            }
        }

        if (characterController != null)
            characterController.enabled = true;

        if (playerController != null)
            playerController.SetMovementEnabled(true);

        currentPlayer = null;
        playerController = null;
        characterController = null;
        isPlayerInside = false;

        if (ActiveVehicle == this)
            ActiveVehicle = null;
    }

    public bool IsPlayerInside()
    {
        return isPlayerInside;
    }

    public string GetExitPrompt()
    {
        return "Tekan " + exitKey + " untuk keluar";
    }
}

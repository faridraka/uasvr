using UnityEngine;

public class HeavyMachineryController : MonoBehaviour
{
    [Header("Driving")]
    public float moveSpeed = 5f;
    public float turnSpeed = 80f;
    [SerializeField] private ForkliftDriveController driveController;
    [SerializeField] private bool requireEngineOn = false;

    [Header("Lift")]
    public Transform lift;
    public float liftSpeed = 2f;
    public float minHeight = -1f;
    public float maxHeight = 3f;

    private bool liftUp = false;
    private bool liftDown = false;

    void Awake()
    {
        if (driveController == null)
            driveController = GetComponent<ForkliftDriveController>();

        if (driveController == null)
            driveController = GetComponentInParent<ForkliftDriveController>();

        if (driveController == null)
            driveController = GetComponentInChildren<ForkliftDriveController>();
    }

    void Update()
    {
        if (!CanControl())
        {
            liftUp = false;
            liftDown = false;
            return;
        }

        float move = Input.GetAxis("Vertical");
        float turn = Input.GetAxis("Horizontal");

        transform.Translate(Vector3.forward * move * moveSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up * turn * turnSpeed * Time.deltaTime);

        liftUp = Input.GetKey(KeyCode.R);
        liftDown = Input.GetKey(KeyCode.F);

        if (lift != null)
        {
            Vector3 pos = lift.localPosition;

            if (liftUp)
                pos.y += liftSpeed * Time.deltaTime;

            if (liftDown)
                pos.y -= liftSpeed * Time.deltaTime;

            pos.y = Mathf.Clamp(pos.y, minHeight, maxHeight);

            lift.localPosition = pos;
        }
    }

    private bool CanControl()
    {
        if (driveController == null) return false;
        if (!driveController.IsPlayerInside()) return false;

        if (TrainingManager.Instance != null)
        {
            if (requireEngineOn && !TrainingManager.Instance.isEngineOn) return false;
            if (TrainingManager.Instance.isEmergencyStopped) return false;
        }

        return true;
    }

    public void ActivateLift()
    {
        if (!CanControl()) return;
        liftUp = true;
    }

    public void DeactivateLift()
    {
        liftUp = false;
    }

    public void LowerLift()
    {
        if (!CanControl()) return;
        liftDown = true;
    }

    public void StopLowerLift()
    {
        liftDown = false;
    }
}

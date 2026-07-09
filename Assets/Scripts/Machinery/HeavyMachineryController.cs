using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HeavyMachineryController : MonoBehaviour
{
    private const float InputDeadZone = 0.05f;

    [Header("Driving")]
    public float moveSpeed = 3.5f;
    public float turnSpeed = 50f;
    public float acceleration = 1.8f;
    public float turnAcceleration = 1.8f;
    [Range(0.1f, 1f)] public float reverseSpeedMultiplier = 0.45f;
    [Range(0.1f, 1f)] public float reverseTurnMultiplier = 0.45f;
    [SerializeField] private ForkliftDriveController driveController;
    [SerializeField] private bool requireEngineOn = true;

    [Header("Lift")]
    public Transform lift;
    public float liftSpeed = 2f;
    public float minHeight = -1f;
    public float maxHeight = 3f;

    private bool liftUp = false;
    private bool liftDown = false;
    private float currentMove = 0f;
    private float currentTurn = 0f;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.constraints |= RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

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
            currentMove = 0f;
            currentTurn = 0f;
            return;
        }

        float targetMove = Input.GetAxis("Vertical");
        float targetTurn = Input.GetAxis("Horizontal");
        bool wantsLiftUp = Input.GetKey(KeyCode.R);
        bool wantsLiftDown = Input.GetKey(KeyCode.F);

        if ((Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.F)) && AudioManager.Instance != null)
            AudioManager.Instance.PlayLever();

        ReportOperateControl(targetMove, targetTurn, wantsLiftUp, wantsLiftDown);

        currentMove = Mathf.MoveTowards(currentMove, targetMove, acceleration * Time.deltaTime);
        currentTurn = Mathf.MoveTowards(currentTurn, targetTurn, turnAcceleration * Time.deltaTime);

        liftUp = wantsLiftUp;
        liftDown = wantsLiftDown;

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

    void FixedUpdate()
    {
        if (!CanControl()) return;

        float effectiveMoveSpeed = currentMove < 0f ? moveSpeed * reverseSpeedMultiplier : moveSpeed;
        float effectiveTurnSpeed = currentMove < 0f ? turnSpeed * reverseTurnMultiplier : turnSpeed;

        Vector3 nextPosition = rb.position + transform.forward * currentMove * effectiveMoveSpeed * Time.fixedDeltaTime;
        Quaternion nextRotation = rb.rotation * Quaternion.Euler(0f, currentTurn * effectiveTurnSpeed * Time.fixedDeltaTime, 0f);

        rb.MovePosition(nextPosition);
        rb.MoveRotation(nextRotation);
    }

    private bool CanControl()
    {
        if (driveController == null) return false;
        if (!driveController.IsPlayerInside()) return false;

        if (requireEngineOn && TrainingManager.Instance == null)
            return false;

        if (TrainingManager.Instance != null)
        {
            if (requireEngineOn && !TrainingManager.Instance.isEngineOn) return false;
            if (TrainingManager.Instance.isEmergencyStopped) return false;
        }

        return true;
    }

    private void ReportOperateControl(float moveInput, float turnInput, bool wantsLiftUp, bool wantsLiftDown)
    {
        TrainingManager trainingManager = TrainingManager.Instance;
        if (trainingManager == null || trainingManager.currentStep != TrainingManager.TrainingStep.Operate)
            return;

        bool hasControlInput =
            Mathf.Abs(moveInput) > InputDeadZone ||
            Mathf.Abs(turnInput) > InputDeadZone ||
            wantsLiftUp ||
            wantsLiftDown;

        if (hasControlInput)
            trainingManager.OperateControl();
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

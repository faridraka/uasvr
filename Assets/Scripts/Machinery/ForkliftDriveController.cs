using UnityEngine;

// Script INTI buat nyetir forklift. Beda sama HeavyMachineryController.cs (yang cuma angkat fork) —
// ini yang bikin BADAN forkliftnya bisa jalan maju/mundur/belok pakai Rigidbody physics beneran.
[RequireComponent(typeof(Rigidbody))]
public class ForkliftDriveController : MonoBehaviour
{
    [Header("Driving Settings")]
    public float moveSpeed = 5f;
    public float turnSpeed = 60f;

    [Header("Titik Masuk & Keluar")]
    public Transform driverSeatPoint;
    public Transform exitPoint;

    [Header("Kamera")]
    public GameObject playerCameraObject;
    public GameObject forkliftCameraObject;

    [Header("Tombol Keluar")]
    public KeyCode exitKey = KeyCode.F;

    private Rigidbody rb;
    private GameObject currentPlayer;
    private PlayerController playerController;
    private bool isPlayerInside = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Kunci rotasi X/Z biar forklift ga kejungkir pas belok/nabrak, tapi tetap bisa muter di Y (belok)
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        if (forkliftCameraObject != null) forkliftCameraObject.SetActive(false);
    }

    void Update()
    {
        if (!isPlayerInside) return;

        if (Input.GetKeyDown(exitKey))
        {
            ExitVehicle();
            return;
        }

        // Cuma bisa disetir kalau mesin nyala dan ga lagi emergency stop
        if (TrainingManager.Instance.isEngineOn && !TrainingManager.Instance.isEmergencyStopped)
        {
            HandleDriving();
        }
    }

    void HandleDriving()
    {
        float move = Input.GetAxis("Vertical") * moveSpeed;
        float turn = Input.GetAxis("Horizontal") * turnSpeed;

        Vector3 newPos = rb.position + transform.forward * move * Time.deltaTime;
        rb.MovePosition(newPos);

        Quaternion turnRotation = Quaternion.Euler(0f, turn * Time.deltaTime, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }

    // Dipanggil dari ForkliftSeat.cs pas player interact sama kursi/tangga forklift
    public void EnterVehicle(GameObject player)
    {
        if (isPlayerInside) return;

        currentPlayer = player;
        playerController = player.GetComponent<PlayerController>();

        if (playerController != null) playerController.enabled = false;
        player.transform.position = driverSeatPoint.position;

        if (playerCameraObject != null) playerCameraObject.SetActive(false);
        if (forkliftCameraObject != null) forkliftCameraObject.SetActive(true);

        isPlayerInside = true;
    }

    public void ExitVehicle()
    {
        if (!isPlayerInside) return;

        if (currentPlayer != null)
        {
            currentPlayer.transform.position = exitPoint.position;
            if (playerController != null) playerController.enabled = true;
        }

        if (forkliftCameraObject != null) forkliftCameraObject.SetActive(false);
        if (playerCameraObject != null) playerCameraObject.SetActive(true);

        isPlayerInside = false;
        currentPlayer = null;
    }

    public bool IsPlayerInside() => isPlayerInside;
}
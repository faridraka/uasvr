using UnityEngine;

public class HeavyMachineryController : MonoBehaviour
{
    [Header("Forklift Movement")]
    public float moveSpeed = 5f;
    public float turnSpeed = 80f;

    [Header("Lift")]
    public Transform lift;
    public float liftSpeed = 8f;
    public float minHeight = 1f;
    public float maxHeight = 8f;

    private float currentHeight;

    void Start()
    {
        currentHeight = lift.localPosition.y;
    }

    void Update()
    {
        // Jalan maju mundur
        float move = Input.GetAxis("Vertical");
        transform.Translate(Vector3.forward * move * moveSpeed * Time.deltaTime);

        // Belok
        float turn = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * turn * turnSpeed * Time.deltaTime);

        // Naik (Q)
        if (Input.GetKey(KeyCode.Q))
        {
            currentHeight += liftSpeed * Time.deltaTime;
        }

        // Turun (E)
        if (Input.GetKey(KeyCode.E))
        {
            currentHeight -= liftSpeed * Time.deltaTime;
        }

        currentHeight = Mathf.Clamp(currentHeight, minHeight, maxHeight);

        Vector3 pos = lift.localPosition;
        pos.y = currentHeight;
        lift.localPosition = pos;
    }
}
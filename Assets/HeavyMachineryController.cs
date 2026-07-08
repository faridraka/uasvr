using UnityEngine;

public class HeavyMachineryController : MonoBehaviour
{
    [Header("Driving")]
    public float moveSpeed = 5f;
    public float turnSpeed = 80f;

    [Header("Lift")]
    public Transform lift;
    public float liftSpeed = 2f;
    public float minHeight = 0.5f;
    public float maxHeight = 3f;

    private bool liftUp = false;
    private bool liftDown = false;

    void Update()
    {
        // Gerak forklift
        float move = Input.GetAxis("Vertical");
        float turn = Input.GetAxis("Horizontal");

        transform.Translate(Vector3.forward * move * moveSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up * turn * turnSpeed * Time.deltaTime);

        // Kontrol keyboard
        liftUp = Input.GetKey(KeyCode.R);
        liftDown = Input.GetKey(KeyCode.F);

        // Gerak lift
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

    public void ActivateLift()
    {
        liftUp = true;
    }

    public void DeactivateLift()
    {
        liftUp = false;
    }

    public void LowerLift()
    {
        liftDown = true;
    }

    public void StopLowerLift()
    {
        liftDown = false;
    }
}
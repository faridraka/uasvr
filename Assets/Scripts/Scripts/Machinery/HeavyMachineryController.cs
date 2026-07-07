using UnityEngine;
using System.Collections;

// Kontrol pergerakan fisik alat berat (garpu forklift naik-turun / lengan crane).
// Rigidbody di-set kinematic karena gerakannya kita atur manual via MovePosition,
// tapi tetap kepake buat physics interaction pas nyentuh/gotong LoadCargo.
public class HeavyMachineryController : MonoBehaviour
{
    public Transform forkTransform;
    public float liftHeight = 1.5f;
    public float liftSpeed = 1f;

    private Rigidbody forkRigidbody;
    private Vector3 startPos;
    private bool isLifting = false;

    void Start()
    {
        forkRigidbody = forkTransform.GetComponent<Rigidbody>();
        if (forkRigidbody != null)
        {
            forkRigidbody.isKinematic = true;
        }
        startPos = forkTransform.position;
    }

    public void ActivateLift()
    {
        if (!isLifting)
            StartCoroutine(LiftRoutine());
    }

    IEnumerator LiftRoutine()
    {
        isLifting = true;
        Vector3 target = startPos + Vector3.up * liftHeight;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * liftSpeed;
            Vector3 newPos = Vector3.Lerp(startPos, target, t);
            if (forkRigidbody != null)
                forkRigidbody.MovePosition(newPos);
            else
                forkTransform.position = newPos;
            yield return null;
        }

        isLifting = false;
    }

    public void LowerFork()
    {
        StartCoroutine(LowerRoutine());
    }

    IEnumerator LowerRoutine()
    {
        Vector3 current = forkTransform.position;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * liftSpeed;
            Vector3 newPos = Vector3.Lerp(current, startPos, t);
            if (forkRigidbody != null)
                forkRigidbody.MovePosition(newPos);
            else
                forkTransform.position = newPos;
            yield return null;
        }
    }
}

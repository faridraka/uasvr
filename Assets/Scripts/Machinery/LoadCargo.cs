using UnityEngine;

// Ditempel di crate/pallet. Nyimpen status kepick-up atau engga, dan ngasih tau
// AdvancedLoadTask/TrainingManager pas nyampe target area.
[RequireComponent(typeof(Rigidbody))]
public class LoadCargo : MonoBehaviour
{
    [Tooltip("0 = ringan, 1 = sedang, 2 = berat/berbahaya - dipakai AdvancedLoadTask")]
    public int cargoTypeIndex = 0;

    private Rigidbody rb;
    private Transform originalParent;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalParent = transform.parent;
    }

    public void PickUp(Transform holdPoint)
    {
        rb.isKinematic = true;
        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
    }

    public void Drop()
    {
        transform.SetParent(originalParent);
        rb.isKinematic = false;
    }

    // Dipanggil dari ZoneTrigger.cs pas load masuk TargetArea
    public void OnEnterTargetArea(int targetIndex)
    {
        AdvancedLoadTask task = FindObjectOfType<AdvancedLoadTask>();
        if (task != null)
        {
            task.CheckDelivery(this, targetIndex);
        }
        else
        {
            TrainingManager.Instance.ReportLoadDelivered();
        }
    }

    // Dipanggil kalau load dijatuhkan tanpa prosedur yang benar
    public void ForceDropIncorrectly()
    {
        rb.isKinematic = false;
        rb.AddForce(Vector3.down * 2f, ForceMode.Impulse);
    }
}

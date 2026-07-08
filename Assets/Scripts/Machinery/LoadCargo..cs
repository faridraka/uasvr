using UnityEngine;

public class LoadCargo : MonoBehaviour
{
    [Header("Cargo Type")]
    public int cargoTypeIndex = 0;

    [Header("Load Point")]
    public Transform loadPoint;

    private GameObject currentCargo;
    private Transform originalParent;

    void Update()
    {
        if (currentCargo != null)
        {
            currentCargo.transform.position = loadPoint.position;
            currentCargo.transform.rotation = loadPoint.rotation;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentCargo != null)
            {
                Drop();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (other.CompareTag("Cargo") && currentCargo == null)
            {
                currentCargo = other.gameObject;
                originalParent = currentCargo.transform.parent;

                Rigidbody rb = currentCargo.GetComponent<Rigidbody>();
                rb.isKinematic = true;

                currentCargo.transform.SetParent(loadPoint);
                currentCargo.transform.localPosition = Vector3.zero;
                currentCargo.transform.localRotation = Quaternion.identity;
            }
        }
    }

    public void Drop()
    {
        Rigidbody rb = currentCargo.GetComponent<Rigidbody>();
        rb.isKinematic = false;

        currentCargo.transform.SetParent(originalParent);
        currentCargo = null;
    }

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

    public void ForceDropIncorrectly()
    {
        if (currentCargo == null) return;

        Rigidbody rb = currentCargo.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddForce(Vector3.down * 2f, ForceMode.Impulse);
    }
}

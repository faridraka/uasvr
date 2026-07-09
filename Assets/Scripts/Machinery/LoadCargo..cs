using UnityEngine;

public class LoadCargo : MonoBehaviour
{
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
            if (IsLoadObject(other.gameObject) && currentCargo == null)
            {
                currentCargo = other.gameObject;
                originalParent = currentCargo.transform.parent;

                Rigidbody rb = currentCargo.GetComponent<Rigidbody>();
                if (rb != null)
                    rb.isKinematic = true;

                currentCargo.transform.SetParent(loadPoint);
                currentCargo.transform.localPosition = Vector3.zero;
                currentCargo.transform.localRotation = Quaternion.identity;
            }
        }
    }

    public void Drop()
    {
        if (currentCargo == null) return;

        Rigidbody rb = currentCargo.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = false;

        currentCargo.transform.SetParent(originalParent);
        currentCargo = null;
    }

    public void ForceDropIncorrectly()
    {
        if (currentCargo == null) return;

        Rigidbody rb = currentCargo.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.AddForce(Vector3.down * 2f, ForceMode.Impulse);
        }
    }

    private bool IsLoadObject(GameObject obj)
    {
        if (obj == null) return false;

        return obj.tag == "Load" || obj.tag == "Cargo";
    }
}

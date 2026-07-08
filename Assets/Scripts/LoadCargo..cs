using UnityEngine;

public class LoadCargo : MonoBehaviour
{
    public Transform loadPoint;

    private GameObject currentCargo;

    void Update()
    {
        // Cargo selalu mengikuti LoadPoint
        if (currentCargo != null)
        {
            currentCargo.transform.position = loadPoint.position;
            currentCargo.transform.rotation = loadPoint.rotation;
        }

        // Lepas cargo
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentCargo != null)
            {
                Rigidbody rb = currentCargo.GetComponent<Rigidbody>();

                rb.isKinematic = false;

                currentCargo.transform.SetParent(null);

                currentCargo = null;
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

                Rigidbody rb = currentCargo.GetComponent<Rigidbody>();

                rb.isKinematic = true;

                currentCargo.transform.SetParent(loadPoint);

                currentCargo.transform.localPosition = Vector3.zero;

                currentCargo.transform.localRotation = Quaternion.identity;
            }
        }
    }
}
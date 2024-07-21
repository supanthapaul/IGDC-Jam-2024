using LockAndDoor;
using UnityEngine;

public class InteractionRay : MonoBehaviour
{
    private Camera cam;
    private Lock currentLock;
    private Vector3 screenMidpoint = new(0.5f, 0.5f, 0f);

    private Ray gazeRay;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gazeRay = cam.ViewportPointToRay(screenMidpoint);
        if (Physics.Raycast(gazeRay, out RaycastHit hit, 50f))
        {
            if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Lock"))
                return;

            if (currentLock == null)
            {
                currentLock = hit.transform.GetComponent<Lock>();
                return;
            }
            else if (currentLock.transform.position != hit.transform.position) //different lock
            {
                currentLock = hit.transform.GetComponent<Lock>();
            }
            currentLock.Fill();
        }
    }
}

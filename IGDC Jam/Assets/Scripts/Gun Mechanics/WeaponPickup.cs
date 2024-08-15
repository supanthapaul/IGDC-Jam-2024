using Audio;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    private Vector3 camPos;
    private Transform camTransform;
    [SerializeField] private Transform childTransform;
    private void Start()
    {
        camTransform = Camera.main.transform;
    }

    private void Update()
    {
        camPos = new Vector3(camTransform.position.x, 0f, camTransform.position.z);
        transform.LookAt(camTransform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (other.TryGetComponent(out FPController controller))
        {
            AudioManager.instance.PlaySound("equip", transform.position);
            controller.weaponsHolder.AddWeapon();
        }
        Destroy(gameObject);
    }
}

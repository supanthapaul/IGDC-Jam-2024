using UnityEngine;

public class LockLookAt : MonoBehaviour
{

    private Vector3 lookAtVector;
    private Transform playerTransform;

    private void Start()
    {
        playerTransform = GameManager.Instance.playerController.transform;
    }
    // Update is called once per frame
    void Update()
    {
        lookAtVector = new Vector3(playerTransform.position.x, playerTransform.position.y, playerTransform.position.z);
        lookAtVector -= transform.position;
        transform.rotation = Quaternion.LookRotation(lookAtVector, Vector3.up);
    }
}

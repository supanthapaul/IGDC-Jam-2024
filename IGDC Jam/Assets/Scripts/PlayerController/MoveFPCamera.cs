using UnityEngine;

public class MoveFPCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraTargetPos;
    
    private void Update()
    {
        transform.position = cameraTargetPos.position;
    }
}

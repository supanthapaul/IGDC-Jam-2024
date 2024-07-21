using DG.Tweening;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private Vector3 rotationOffset;
    [SerializeField] private float duration = 0.5f;
    
    
    public void Rotate()
    {
        transform.DOLocalRotate( transform.localEulerAngles + rotationOffset, duration).SetEase(Ease.OutBack);
    }
}

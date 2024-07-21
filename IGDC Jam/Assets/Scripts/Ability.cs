using Health_System;
using UnityEngine;

public enum Abilities
{
    ForwardWalk,
    HorizontalLook,
    VerticalLook,
    Strafe,
    Jump,
    Dash,
    Wallrun,
    Crouch,
    Slide,
    Fire,
    Reload
}
public class Ability : MonoBehaviour
{
    public Abilities nextUnlock;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&&other.TryGetComponent(out IHealth _))
        {
            GameManager.Instance.GiveAbility(nextUnlock);
            GameManager.Instance.SetAllRestrictions();
            Destroy(gameObject);
        }
  
        
    }

}

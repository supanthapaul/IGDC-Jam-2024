using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TriggerType
{
    AUTOMATIC, //M4A1 from CS2
    SEMIAUTO, //Deagle from CS2
    CHARGE    //Charge Rifle from Apex, or Railgun from Helldivers 2
}
[CreateAssetMenu(fileName = "Gun Stats", menuName = "Gun")]
public class GunStats : ScriptableObject
{
    public TriggerType triggerType;
    public int maxCapacity;
    [Tooltip("Shots per second")] public float fireRate; //also substitutes for minimum downtime between shots
                                                         //fired in SemiAuto weapons
    [Tooltip("In seconds")] public float reloadTime;

}

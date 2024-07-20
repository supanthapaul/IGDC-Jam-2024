using UnityEngine;

public enum BulletType
{
    RAYCAST, //Instant cast (All bullets in CS)
    TRAVEL, //Bullets travel in a straight line and take time to hit a target(Like Kiriko from Overwatch)
    PHYSICAL //Like grenade launchers that are shot in arcs and have physics applied to them 
}

[CreateAssetMenu(fileName = "Bullet Stats ", menuName = "Bullet")]
public class BulletStats : ScriptableObject
{
    public BulletType bulletType;
    public Bullet bulletObject;
    public int damage;
    public float speed;
    //public float critMultiplier;
    //We can also add status effect infliction like bleed or aim knockback according to the game type
    //for a barebones structure, these will do
}

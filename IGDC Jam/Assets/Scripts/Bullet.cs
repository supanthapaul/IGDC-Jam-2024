using Health_System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    public GameObject onHitEffect;
    public void Init(float speed, int damage)
    {
        this.speed = speed;
        this.damage = damage;
    }
    void Start()
    {
        Destroy(gameObject, 5f);    
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime * transform.forward, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag(other.tag)) return;

        if(other.TryGetComponent(out IHealth healthComponent))
        {
            healthComponent.TakeDamage(damage);
        }
        onHitEffect.SetActive(true);
        onHitEffect.transform.parent = null;
        Destroy(gameObject);
    }
}

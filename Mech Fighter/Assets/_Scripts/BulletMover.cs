using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMover : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] [Range(0f, 200f)] private float force = 100f;
    [SerializeField] [Range(0f, 10f)] private float lifeTime = 5f;
    [SerializeField] private float damage = 1f;
    [SerializeField] private bool canHurtPlayer = false;
    private float remainingLifeTime;

    void Awake()
    {
        remainingLifeTime = lifeTime;
        rb.AddForce(transform.forward * force, ForceMode.Impulse);
    }
    void Update()
    {
        remainingLifeTime -= Time.deltaTime;
        if (remainingLifeTime <= 0f)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            // hurt the object
        }
        else if (collision.gameObject.tag == "Player" && canHurtPlayer)
        {
            // hurt the player
        }
        //delete self
        Destroy(this.gameObject);
    }
    public float GetDamage()
    {
        return damage;
    }
    public bool CanHurtPlayer()
    {
        return canHurtPlayer;
    }
    private void OnDestroy()
    {
        rb = null;
    }
}

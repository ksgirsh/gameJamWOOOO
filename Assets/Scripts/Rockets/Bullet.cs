using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float attackDamage = 25f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Alien")
        {
            Health alieHealth = coll.gameObject.GetComponent<Health>();
            StartCoroutine(alieHealth.TakeDamage(attackDamage));
        }
    }
}

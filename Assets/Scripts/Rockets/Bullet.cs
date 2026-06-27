using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float attackDamage = 25f;
    [SerializeField] AudioClip[] sfx;

    [SerializeField] bool isTrap;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SoundFXManager.instance.PlaySoundEffectClip(sfx[0], transform.position, 1f);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Alien")
        {
            Health alieHealth = coll.gameObject.GetComponent<Health>();
            StartCoroutine(alieHealth.TakeDamage(attackDamage));

            if (isTrap)
            {
                Health thisHealth = gameObject.GetComponent<Health>();
                StartCoroutine(alieHealth.TakeDamage(40f));

            }
        }
    }
}

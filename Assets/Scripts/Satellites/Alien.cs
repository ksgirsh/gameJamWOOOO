using UnityEngine;
using System.Collections;

public class Alien : MonoBehaviour
{
    private Health heal;
    float shrinkF = 1f;
    Camera mc;
    FadeIn fade;
    Rigidbody2D rb;
    SelectControl control;

    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float attackDamage = 30f;

    [Header("Sound")]
    //0 is alien damage, 1 is alien destroy, 2 is planet damage, 3 is hookdmg
    [SerializeField] AudioClip[] sfx;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    WaveController alienControl;


    void Start()
    {
        GameObject camObj = GameObject.FindGameObjectWithTag("MainCamera");
        mc = camObj.GetComponent<Camera>();
        heal = gameObject.GetComponent<Health>();
        fade = gameObject.GetComponent<FadeIn>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        GameObject manager = GameObject.FindGameObjectWithTag("Player");
        control = manager.GetComponent<SelectControl>();
        alienControl = manager.GetComponent<WaveController>();

        transform.position = RandomPointInRing(Vector2.zero);
        
        Vector2 dir = -((transform.position).normalized);
        rb.AddForce(dir * moveSpeed, ForceMode2D.Impulse);

        float angle = (Mathf.Atan2(dir.y, dir.x)) * Mathf.Rad2Deg;
        Quaternion quatRotate = Quaternion.Euler(0f, 0f, angle);
        transform.rotation = quatRotate;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if ((coll.gameObject.tag == "Satellite") || (coll.gameObject.tag == "Planet"))
        {

            Health satHealth = coll.gameObject.GetComponent<Health>();
            if (satHealth != null)
            {

                StartCoroutine(satHealth.TakeDamage(attackDamage));

                if (coll.gameObject.tag == "Planet")
                {
                    Destroy(gameObject, 0.42f);
                }
            }

        }

    }

    void OnMouseEnter()
    {
        control.SelectTrigger(gameObject);
    }

    void OnMouseExit()
    {
        control.EraseTrigger();
    }


    float SphereOfInfluence()
    {
        float cachedRad = 0f;
        GameObject[] allSats = GameObject.FindGameObjectsWithTag("Satellite");

        foreach (GameObject sat in allSats)
        {
            if (cachedRad < sat.GetComponent<Satellite>().orbitRadius)
            {
                cachedRad = sat.GetComponent<Satellite>().orbitRadius;
            }

        }


        return (cachedRad + 5f);
    }


    //credit to Emolk on the unity forums for saving me mental energy and being lovely
    public Vector2 RandomPointInRing(Vector2 origin)
    {

        var randomDirection = (Random.insideUnitCircle).normalized;

        var randomDistance = Random.Range(SphereOfInfluence() + 10f, (SphereOfInfluence() + 25f));


        Vector2 point = origin + (randomDirection * randomDistance);
        return point;
    }
}


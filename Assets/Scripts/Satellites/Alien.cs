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

        transform.position = CalcSpawnPos();
        
        Vector2 dir = -((transform.position).normalized);
        rb.AddForce(dir * moveSpeed, ForceMode2D.Impulse);

        float angle = (Mathf.Atan2(dir.y, dir.x)) * Mathf.Rad2Deg;
        Quaternion quatRotate = Quaternion.Euler(0f, 0f, angle);
        transform.rotation = quatRotate;
    }

    Vector2 CalcSpawnPos()
    {
        Vector2 camDim = new Vector2(mc.pixelWidth, mc.pixelHeight);


        float aspect = (camDim.x / camDim.y);
        float wHeight = mc.orthographicSize * 2;
        float wWidth = wHeight * aspect;

        //Debug.Log("WIDTH: " + wWidth + "  HEIGHT: " + wHeight);

        Vector2 wDim = new Vector2(wWidth, wHeight);

        Vector2 point = mc.transform.position + (Vector3)(wDim / 2);
        // Debug.Log(point);

        Vector2 spawnPos = new Vector2((point.x + Random.Range(2, 8)), Random.Range((point.y - wHeight) * shrinkF, point.y * shrinkF));

        float maxRad = SphereOfInfluence();

        if (spawnPos.y < (maxRad + 2f) && spawnPos.y > -(maxRad + 2f))
        {
            int coinFlip = Random.Range(0, 2);
            if (coinFlip == 0)
            {
                spawnPos.y = (maxRad + 2f);
            }
            else
            {
                spawnPos.y = -(maxRad + 2f);
            }
        }

        return spawnPos;
        //x will be point.x + some margin
        //y will be random between point.y * shrinkF and (point.y - wHeight) * shrinkF
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


        return cachedRad;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if ((coll.gameObject.tag == "Satellite") || (coll.gameObject.tag == "Planet"))
        {

            Health satHealth = coll.gameObject.GetComponent<Health>();
            if (satHealth != null)
            {
                StartCoroutine(satHealth.TakeDamage(attackDamage));
            }

            if (coll.gameObject.tag == "Planet")
            {
                Destroy(gameObject, 0.42f);
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


}

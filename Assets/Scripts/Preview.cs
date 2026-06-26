using UnityEngine;
using System.Collections;

public class Preview : MonoBehaviour
{
    public GameObject realVersion;
    public Camera mainCam;
    [SerializeField] string identifier;

    [HideInInspector] public float cost;
    [HideInInspector] public int metalCost;
    private RocketControl houston;


    bool placeable = true;
    bool touching = false;
    [SerializeField] Color placeableColor;
    [SerializeField] Color unplaceableColor;
    SpriteRenderer rend;

    float maxRange;

    [Header("Sound")]
    //0 is placing, 1 is deny
    [SerializeField] AudioClip[] sfx;
    BuyControl buy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        mainCam = (GameObject.FindGameObjectWithTag("MainCamera")).GetComponent<Camera>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        houston = player.GetComponent<RocketControl>();
        rend = gameObject.GetComponent<SpriteRenderer>();


        buy = player.GetComponent<BuyControl>();
        buy.buying = true;
        maxRange = GetSphereOfInfluence();

        if (realVersion.GetComponent<DefensiveSatellite>() != null)
        {
            Transform rangeCircle = transform.GetChild(0);

            float range = realVersion.GetComponent<DefensiveSatellite>().fireRange;

            rangeCircle.localScale *= range;

        }

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = WorldMousePos();
        if (Input.GetButtonDown("Fire1") && placeable)
        {
            GameObject real = GameObject.Instantiate(realVersion, transform.position, Quaternion.identity);

            if (identifier == "Satellite")
            {
                //reminder to remove the "parent addendum" later
                GameObject parent = GameObject.FindGameObjectWithTag("Planet");
                real.GetComponent<Satellite>().orbitRadius = (Mathf.Abs(transform.position.magnitude) - parent.transform.localScale.x);
                real.GetComponent<Satellite>().orbitAngle = Mathf.Atan2(transform.position.y, transform.position.x);
                //placed hook sfx
                AudioClip place = sfx[0];
                SoundFXManager.instance.PlaySoundEffectClip(place, Vector2.zero, 1f);
                buy.buying = false;

            }

            houston.savedDistance -= cost;
            houston.savedMetal -= metalCost;
            Destroy(gameObject);
        } else if (!placeable && (Input.GetButtonDown("Fire1")))
        {
            //1 is deny
            AudioClip place = sfx[1];
            SoundFXManager.instance.PlaySoundEffectClip(place, Vector2.zero, 1f);
        }

        if (Input.GetButtonDown("Fire2"))
        {
            //cancel purchase
            Destroy(gameObject);
            buy.buying = false;

        }

        if (touching == false)
        {
            if (transform.position.magnitude > maxRange)
            {
                placeable = false;
            }
            else if (transform.position.magnitude < 3f)
            {
                placeable = false;
            } else
            {
                placeable = true;
            }
        } else
        {
            placeable = false;
        }
        

        if (placeable)
        {
            rend.color = placeableColor;

        } else
        {
            rend.color = unplaceableColor;
        }
    }

    Vector3 WorldMousePos()
    {
        Vector3 mouseVector = mainCam.ScreenToWorldPoint(Input.mousePosition);
        return new Vector3(mouseVector.x, mouseVector.y, 0);
    }

    float GetSphereOfInfluence()
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

        //magic number
        return (cachedRad + 3.42f);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Satellite")
        {
            touching = true;
        } 
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Satellite")
        {
            touching = false;
        }
    }
}

using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour
{
    public GameObject targetHook;
    public Vector2 normalVector;
    private Vector2 targetPosition;


    [SerializeField] float rocketSpeed = 1;
    [SerializeField] float attachOffset = 0.105f;
    public Satellite hookProperties;
    private float hookAngSpeed;
    private float hookRotationSpeed;
    private Rigidbody2D rb;

    private float hookRad;
    public float trueDistance;

    private Vector2 startSpot;

    public float distanceTravelled;

    [SerializeField] GameObject debugPrefab;

    private TrailRenderer tr;
    float attachedAngle = 0;
    bool tracking;
    bool attached = false;

    public RocketControl houston;
    public float rocketPrice;
    public float rocketLifetime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
       // StartCoroutine(LateStart());

        hookProperties = targetHook.GetComponent<Satellite>();

        hookProperties.LoadRocket(gameObject);

        hookRad = hookProperties.orbitRadius;
        hookAngSpeed = hookProperties.orbitVelocity / hookRad;
        hookRotationSpeed = hookProperties.rotateVelocity;

        targetPosition = (normalVector * hookRad);
        trueDistance = (hookRad - ((transform.position).magnitude));

        tr = gameObject.GetComponent<TrailRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;


        StartCoroutine(LaunchSequence());

        //Destroy(gameObject, 10);

        if (hookProperties.auto == false)
        {
            houston.nonAutoRockets.Add(gameObject);
        }

    }


    // Update is called once per frame
    void Update()
    {
        //STUFF FOR AIM-HOOKS
        if (hookProperties.auto == false)
        {
            if ((attached) && Input.GetButtonDown("Jump"))
            {
                //doesnt work unless this is nested
                if (houston.nonAutoRockets.IndexOf(gameObject) != 0)
                {
                    return;

                } else
                {
                    //some Hook-Aim logic is in disengage, removes rocket from list after disengaged
                    StartCoroutine(Disengage());
                    
                }

            } else if (attached && targetHook.GetComponent<AimHook>().autoUpgrade == true)
            {
                StartCoroutine(NonAutoDisengageTimer(6f));
            }

        }

    }

    void FixedUpdate()
    {
        if (tracking)
        {
            distanceTravelled = Vector2.Distance(transform.position, startSpot);
        }

    }

    float IdealAngle()
    {
        return ((trueDistance * hookAngSpeed) / (rocketSpeed));

        
    }

    Vector2 IdealPosition()
    {
        //in radians
        float targetAngle = Mathf.Atan2(targetPosition.y, targetPosition.x);

        //turn backwards
        float totalAngle = (targetAngle - IdealAngle());


        Vector2 idealPos = new Vector2(hookRad * Mathf.Cos(totalAngle), hookRad * Mathf.Sin(totalAngle));
        
        
        
        
        



        return idealPos;
    }

    float TimeToIdealPosition()
    {

        //distance between current hook pos and ideal pos
        Vector2 idealPosition = (IdealPosition());
        Vector2 distance = (idealPosition - (Vector2)targetHook.transform.position);

        //GameObject debugObj = GameObject.Instantiate(debugPrefab, idealPosition, Quaternion.identity);
        //Destroy(debugObj, 4f);
        //use law of cosines to figure out time to ideal position
        float lawCosValue = (Square(distance.magnitude) - (2 * Square(hookRad))) / (-2 * Square(hookRad));
        float angleLawCos = Mathf.Acos(lawCosValue);

        float crossCheck = Cross((targetHook.transform.position), idealPosition);
        
        if (crossCheck < 0)
        {
            //Debug.Log("Cross Product is negative, utilizing Major Arc");
            angleLawCos = ((Mathf.PI * 2) - angleLawCos);
        }


        //  Debug.Log("centerAngle opposing Chord CI: " + angleLawCos * Mathf.Rad2Deg);
        float timeToIdeal = (angleLawCos / hookAngSpeed);
        return timeToIdeal;

    }

    float Square(float f)
    {
        return Mathf.Pow(f, 2);
        
    }

    IEnumerator LaunchSequence()
    {

        yield return new WaitForSeconds(TimeToIdealPosition());

        
        float duration = (trueDistance / rocketSpeed);
        Vector2 initPos = transform.position;

        //calc targetPos

        for (float i = 0; i < duration; i += Time.deltaTime)
        {
            float normalizedTime = i / duration;
            yield return null;
            Vector2 lerpPos = Vector2.Lerp(initPos, targetPosition, normalizedTime);
            transform.position = lerpPos;

            

        }

        transform.position = hookProperties.hookPoint.position;
        targetHook.transform.rotation = transform.rotation;
        transform.parent = hookProperties.hookPoint;
        transform.localPosition = new Vector2(0f, -attachOffset);
        attached = true;

        Vector3 angles = (transform.rotation).eulerAngles;
        float initAng = angles.z;

        attachedAngle = initAng;
        Quaternion newRotationQ = Quaternion.Euler(0f, 0f, (initAng - 90));
        transform.rotation = newRotationQ;

        
        if (hookProperties.auto == true)
        {
            yield return new WaitForSeconds((2 * Mathf.PI) / hookRotationSpeed);

            StartCoroutine(Disengage());
        }

        
    }

    //figure out why regular dot product check doesnt work under certain conditions, until then use this
    float Cross(Vector2 v, Vector2 w)
    {
        return ((v.x * w.y) - (v.y * w.x));
        // assuming v is the hook and w is the ideal position, if a positive scalar is returned then use minor , if a negative scalar is returned use major
    }

    IEnumerator EraseRocket(float initDelay)
    {
        yield return new WaitForSeconds(initDelay);

        houston.rockets.Remove(gameObject);
        houston.savedDistance += ((int)distanceTravelled);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Target")
        {
            TargetAttribute check = coll.gameObject.GetComponent<TargetAttribute>();

            if (check.identifier == "Meter Target")
            {
                houston.savedDistance += coll.gameObject.GetComponent<MeterTarget>().targetPoints;
                coll.gameObject.GetComponent<MeterTarget>().TargetHit();

            }
        }
    }

    IEnumerator Disengage()
    {
        attached = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        transform.SetParent(null);

        if (hookProperties.auto == false)
        {
            yield return null;
            houston.nonAutoRockets.Remove(gameObject);
        }

        //Quaternion newRotationQ2 = Quaternion.Euler(0f, 0f, (attachedAngle));
        //transform.rotation = newRotationQ2;

        //Quaternion newRotationQ = Quaternion.Euler(0f, 0f, (attachedAngle - 90));

        rb.AddForce(transform.up * ((hookRotationSpeed + (hookAngSpeed / hookRad)) + (rocketSpeed - 1)), ForceMode2D.Impulse);
        //transform.rotation = newRotationQ;

        hookProperties.UnloadRocket(gameObject);

        StartCoroutine(EraseRocket(rocketLifetime + 0.5f));
        yield return new WaitForSeconds(0.5f);
        tr.emitting = true;

        startSpot = transform.position;
        tracking = true;

        
    }

    IEnumerator NonAutoDisengageTimer(float delay)
    {
        
        yield return new WaitForSeconds(delay);
        if (attached)
        {
            StartCoroutine(Disengage());
        }
    }

}
